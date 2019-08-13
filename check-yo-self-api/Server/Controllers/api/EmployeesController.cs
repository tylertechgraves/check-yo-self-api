using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using check_yo_self_api.Server.Entities.Config;
using check_yo_self_indexer_client;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace check_yo_self_api.Server.Controllers.api
{
  [Route("api/[controller]")]
  [AllowAnonymous]
  public class EmployeesController : BaseController
  {
    private readonly ApplicationDbContext _context;
    private readonly ILogger _logger;
    private readonly AppConfig _appConfig;
    private readonly HttpClient _httpClient;
    private EmployeesClient _indexerClient;
    private IndexManagementClient _indexMgmtClient;

    public EmployeesController(IOptionsSnapshot<AppConfig> appConfig, ApplicationDbContext context, ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory)
    {
      _context = context;
      _logger = loggerFactory.CreateLogger<EmployeesController>();
      _appConfig = appConfig.Value;
      _httpClient = httpClientFactory.CreateClient();
      _indexerClient = new check_yo_self_indexer_client.EmployeesClient(_appConfig.IndexerBaseUri, _httpClient);
      _indexMgmtClient = new check_yo_self_indexer_client.IndexManagementClient(_appConfig.IndexerBaseUri, _httpClient);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<check_yo_self_api.Server.Entities.Employee>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
      IEnumerable<check_yo_self_indexer_client.Employee> clientEmployees;

      try
      {
        try
        {
          clientEmployees = await _indexerClient.GetAllAsync();
        }
        catch(SwaggerException swaggerException)
        {
          return StatusCode(swaggerException.StatusCode);
        }

        var employees = clientEmployees.Adapt<List<check_yo_self_api.Server.Entities.Employee>>();

        return Ok(employees);
      }
      catch (Exception ex)
      {
        _logger.LogError(1, ex, "Unable to get all employees");
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    [HttpGet("{employeeId}")]
    [ProducesResponseType(typeof(check_yo_self_api.Server.Entities.Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int employeeId)
    {
      check_yo_self_indexer_client.Employee clientEmployee;

      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          try
          {
            clientEmployee = await _indexerClient.GetByIdAsync(employeeId);
          }
          catch(SwaggerException swaggerException)
          {
            return StatusCode(swaggerException.StatusCode);
          }

          var employee = clientEmployee.Adapt<check_yo_self_api.Server.Entities.Employee>();
          return Ok(employee);
        }
        catch (Exception ex)
        {
          _logger.LogError(1, ex, "Unable to get employee by id");
          return StatusCode(StatusCodes.Status500InternalServerError);
        }
      }
    }

    [HttpGet("GetByLastName/{lastName}")]
    [ProducesResponseType(typeof(List<check_yo_self_api.Server.Entities.Employee>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByLastName(string lastName)
    {
      IEnumerable<check_yo_self_indexer_client.Employee> clientEmployees;

      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          try
          {
            clientEmployees = await _indexerClient.GetByLastNameAsync(lastName);
          }
          catch(SwaggerException swaggerException)
          {
            return StatusCode(swaggerException.StatusCode);
          }

          var employees = clientEmployees.Adapt<List<check_yo_self_indexer_client.Employee>>();
          return Ok(employees);
        }
        catch (Exception ex)
        {
          _logger.LogError(1, ex, "Unable to get query employees by last name");
          return StatusCode(StatusCodes.Status500InternalServerError);
        }
      }
    }

    [HttpGet("GetByFullName/{firstName}/{lastName}")]
    [ProducesResponseType(typeof(List<check_yo_self_api.Server.Entities.Employee>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByFullName(string firstName, string lastName)
    {
      IEnumerable<check_yo_self_indexer_client.Employee> clientEmployees;

      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          try
          {
            clientEmployees = await _indexerClient.GetByFirstAndLastNameAsync(firstName, lastName);
          }
          catch(SwaggerException swaggerException)
          {
            return StatusCode(swaggerException.StatusCode);
          }

          var employees = clientEmployees.Adapt<List<check_yo_self_indexer_client.Employee>>();
          return Ok(employees);
        }
        catch (Exception ex)
        {
          _logger.LogError(1, ex, "Unable to query employees by first name");
          return StatusCode(StatusCodes.Status500InternalServerError);
        }
      }
    }

    [HttpPost]
    [ProducesResponseType(typeof(check_yo_self_api.Server.Entities.Employee), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody]check_yo_self_api.Server.Entities.Employee employee)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          _context.Employees.Add(employee);
          await _context.SaveChangesAsync();

          // Index the newly-added employee
          int result = await IndexEmployee(employee);
          
          if (result == 0)
          {
            return Created("/api/Employees", employee);
          }
          else
          {
            return StatusCode(result);
          }
        }
        catch (Exception ex)
        {
          _logger.LogError(1, ex, "Unable to add new employee: " + employee.LastName + ", " + employee.FirstName);
          return StatusCode(StatusCodes.Status500InternalServerError);
        }
      }
    }

    [HttpPut("{employeeId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int employeeId, [FromBody]check_yo_self_api.Server.Entities.Employee employee)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          if (employeeId != employee.EmployeeId)
            return BadRequest();

          var foundEmployee = _context.Employees.Find(employeeId);

          if (foundEmployee == null)
            return NotFound();

          foundEmployee.EmployeeId = employee.EmployeeId;
          foundEmployee.FirstName = employee.FirstName;
          foundEmployee.LastName = employee.LastName;
          foundEmployee.Salary = employee.Salary;
          foundEmployee.FirstPaycheckDate = employee.FirstPaycheckDate;

          _context.Entry(foundEmployee).State = EntityState.Modified;

          await _context.SaveChangesAsync();

          // Reindex the updated item
          int result = await IndexEmployee(employee);
          
          if (result == 0)
          {
            return NoContent();
          }
          else
          {
            return StatusCode(result);
          }
        }
        catch (Exception ex)
        {
          _logger.LogError(1, ex, "Unable to update the specified employee with id: " + employee.EmployeeId);
          return StatusCode(StatusCodes.Status500InternalServerError);
        }
      }
    }

    [HttpPost("ReindexAllEmployees")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReindexAllEmployees()
    {      
      try
      {
        // Load all employees in the database
        var employees = await _context.Employees.ToListAsync();

        // Delete the existing index
        await _indexMgmtClient.DeleteAsync();

        // Recreate the employees index
        await _indexMgmtClient.PostAsync();

        // Index the loaded employees
        await IndexEmployees(employees);

        return Ok();
      }
      catch (Exception ex)
      {
        _logger.LogError(1, ex, "Unable to re-index all employee records");
        return StatusCode(StatusCodes.Status500InternalServerError);
      }
    }

    private async Task<int> IndexEmployee (check_yo_self_api.Server.Entities.Employee employee)
    {
      // Delete the existing entry
      try
      {
        await _indexerClient.DeleteAsync(employee.EmployeeId);
      }
      catch
      {
        // We don't care of the delete fails.
      }

      var clientEmployee = employee.Adapt<check_yo_self_indexer_client.Employee>();
      var clientList = new List<check_yo_self_indexer_client.Employee>()
      {
        clientEmployee
      };

      try 
      {
        await _indexerClient.BulkPostAsync(clientList);
        return 0;
      }
      catch(SwaggerException swaggerException)
      {
        return swaggerException.StatusCode;
      }
    }

    private async Task<int> IndexEmployees (List<check_yo_self_api.Server.Entities.Employee> employees)
    {
      var clientEmployees = employees.Adapt<List<check_yo_self_indexer_client.Employee>>();

      try 
      {
        await _indexerClient.BulkPostAsync(clientEmployees);
        return 0;
      }
      catch(SwaggerException swaggerException)
      {
        return swaggerException.StatusCode;
      }
    }
  }
}