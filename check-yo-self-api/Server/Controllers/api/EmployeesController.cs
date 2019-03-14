using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using check_yo_self_api.Server.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace check_yo_self_api.Server.Controllers.api
{
  [Route("api/[controller]")]
  [AllowAnonymous]
  public class EmployeesController : BaseController
  {
    private readonly ApplicationDbContext _context;

    private readonly ILogger _logger;

    public EmployeesController(ApplicationDbContext context, ILoggerFactory loggerFactory)
    {
      _context = context;
      _logger = loggerFactory.CreateLogger<EmployeesController>();
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Employee>), StatusCodes.Status200OK)]
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
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
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
    [ProducesResponseType(typeof(List<Employee>), StatusCodes.Status200OK)]
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

    [HttpGet("QueryByLastName/{lastName}")]
    [ProducesResponseType(typeof(List<Employee>), StatusCodes.Status200OK)]
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

    [HttpGet("QueryByFirstName/{firstName}")]
    [ProducesResponseType(typeof(List<Employee>), StatusCodes.Status200OK)]
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

    [HttpPost]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody]Employee employee)
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
          return Created("/api/Employees", employee);
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
          
          return NoContent();
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int employeeId, [FromBody]Employee employee)
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

          _context.Entry(employee).State = EntityState.Modified;

          await _context.SaveChangesAsync();

          return NoContent();
        }
        catch (Exception ex)
        {
          _logger.LogError(1, ex, "Unable to update the specified employee with id: " + employee.EmployeeId);
          return StatusCode(StatusCodes.Status500InternalServerError);
        }
      }
    }
  }
}