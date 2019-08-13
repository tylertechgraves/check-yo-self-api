using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using check_yo_self_api.Server.Entities;
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

    public EmployeesController(IOptionsSnapshot<AppConfig> appConfig, ApplicationDbContext context, ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory)
    {
      _context = context;
      _logger = loggerFactory.CreateLogger<EmployeesController>();
      _appConfig = appConfig.Value;
      _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<check_yo_self_api.Server.Entities.Employee>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          var employees = await _context.Employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToAsyncEnumerable().ToList();
          return Ok(employees);
        }
        catch (Exception ex)
        {
          _logger.LogError(1, ex, "Unable to get all employees");
          return StatusCode(StatusCodes.Status500InternalServerError);
        }
      }
    }

    [HttpGet("{employeeId}")]
    [ProducesResponseType(typeof(check_yo_self_api.Server.Entities.Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int employeeId)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          var employee = await _context.Employees.FindAsync(employeeId);
          return Ok(employee);
        }
        catch (Exception ex)
        {
          _logger.LogError(1, ex, "Unable to get employee by id");
          return StatusCode(StatusCodes.Status500InternalServerError);
        }
      }
    }

    [HttpGet("SalaryGreaterEqualTo/{salary:decimal}")]
    [ProducesResponseType(typeof(List<check_yo_self_api.Server.Entities.Employee>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBySalary(decimal salary)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          // Because Sqlite does not support the decimal data type,
          // we run an explicit conversion on the TEXT value that's coming back from Sqllite
          // in order to ensure we're going a decimal comparison instead of a text comparison.
          var employees = await _context.Employees.Where(e => Convert.ToDecimal(e.Salary) >= salary).ToAsyncEnumerable().ToList();
          return Ok(employees);
        }
        catch (Exception ex)
        {
          _logger.LogError(1, ex, "Unable to query employees by salary");
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
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          var employees = await _context.Employees.Where(e => e.LastName.ToLower() == lastName.ToLower()).ToAsyncEnumerable().ToList();
          return Ok(employees);
        }
        catch (Exception ex)
        {
          _logger.LogError(1, ex, "Unable to get query employees by last name");
          return StatusCode(StatusCodes.Status500InternalServerError);
        }
      }
    }

    [HttpGet("GetByFirstName/{firstName}")]
    [ProducesResponseType(typeof(List<check_yo_self_api.Server.Entities.Employee>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByFirstName(string firstName)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          var employees = await _context.Employees.Where(e => e.FirstName.ToLower() == firstName.ToLower()).ToAsyncEnumerable().ToList();
          return Ok(employees);
        }
        catch (Exception ex)
        {
          _logger.LogError(1, ex, "Unable to query employees by first name");
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
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          var employees = await _context.Employees.Where(e => e.FirstName.ToLower() == firstName.ToLower() && e.LastName.ToLower() == lastName.ToLower()).ToAsyncEnumerable().ToList();

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

    [HttpDelete("{employeeId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int employeeId)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }
      else
      {
        try
        {
          var employee = await _context.Employees.FindAsync(employeeId);

          if (employee == null)
            return NotFound();

          _context.Employees.Remove(employee);
          await _context.SaveChangesAsync();

          // Remove deleted item from the index
          var indexerClient = new check_yo_self_indexer_client.EmployeesClient(_appConfig.IndexerBaseUri, _httpClient);
          
          try
          {
            await indexerClient.DeleteAsync(employeeId);
            return NoContent();
          }
          catch(SwaggerException swaggerException)
          {
            return StatusCode(swaggerException.StatusCode);
          }
        }
        catch (Exception ex)
        {
          _logger.LogError(1, ex, "Unable to delete the specified employee with id: " + employeeId);
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
        var indexMgmtClient = new check_yo_self_indexer_client.IndexManagementClient(_appConfig.IndexerBaseUri, _httpClient);
        await indexMgmtClient.DeleteAsync();

        // Recreate the employees index
        await indexMgmtClient.PostAsync();

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
      var indexerClient = new check_yo_self_indexer_client.EmployeesClient(_appConfig.IndexerBaseUri, _httpClient);
      var clientEmployee = employee.Adapt<check_yo_self_indexer_client.Employee>();
      var clientList = new List<check_yo_self_indexer_client.Employee>()
      {
        clientEmployee
      };

      try 
      {
        await indexerClient.BulkPostAsync(clientList);
        return 0;
      }
      catch(SwaggerException swaggerException)
      {
        return swaggerException.StatusCode;
      }
    }

    private async Task<int> IndexEmployees (List<check_yo_self_api.Server.Entities.Employee> employees)
    {
      var indexerClient = new check_yo_self_indexer_client.EmployeesClient(_appConfig.IndexerBaseUri, _httpClient);

      var clientEmployees = employees.Adapt<List<check_yo_self_indexer_client.Employee>>();

      try 
      {
        await indexerClient.BulkPostAsync(clientEmployees);
        return 0;
      }
      catch(SwaggerException swaggerException)
      {
        return swaggerException.StatusCode;
      }
    }
  }
}