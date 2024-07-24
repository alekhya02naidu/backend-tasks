using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Serilog;
using EMS.ResponseModel;
using EMS.ResponseModel.Enums;
using EMS.ValidationHelper;
using EMS.BAL.Interfaces;
using EMS.BAL.BAL;
using EMS.DAL.DTO;
using EMS.DB.Models;

namespace EMS.Controllers;

[Route("[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMasterDataService _masterDataService;
    private readonly IRolesService _rolesService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeService employeeService, IMasterDataService masterDataService, IRolesService rolesService, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _masterDataService = masterDataService;
        _rolesService = rolesService;
        _logger = logger;
    }

    private ApiResponse<T> GenerateResponse<T>(T data, ResponseStatus status = ResponseStatus.Success, ErrorCode? errorCode = null)
    {
        return new ApiResponse<T>
        (
            status,
            data,
            errorCode
        );
    }

    [HttpGet]
    public async Task<ApiResponse<PaginatedResult<EmployeeDetail>>> GetAllEmployee([FromQuery] int pageIndex, [FromQuery] int pageSize)
    {
        try
        {
            var employees = await _employeeService.GetAll(pageIndex, pageSize);
            return GenerateResponse(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all paginated employees");
            return GenerateResponse<PaginatedResult<EmployeeDetail>>(null, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ApiResponse<IEnumerable<EmployeeDetail>>> GetEmployee(int id)
    {
        try
        {
            var userIdStr = User.FindFirstValue("Id");
            if (!int.TryParse(userIdStr, out int userId))
            {
                return GenerateResponse<IEnumerable<EmployeeDetail>>(null, ResponseStatus.Error, ErrorCode.InternalServerError);
            }
            if (userId == id)
            {
                var employee = await _employeeService.GetEmployeeById(id);
                if (employee == null)
                {
                    return GenerateResponse<IEnumerable<EmployeeDetail>>(null, ResponseStatus.Error, ErrorCode.NotFound);
                }
                return GenerateResponse<IEnumerable<EmployeeDetail>>(new List<EmployeeDetail> { employee });
            }
            else
            {
                return GenerateResponse<IEnumerable<EmployeeDetail>>(null, ResponseStatus.Error, ErrorCode.UnAuthorized);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching employee with ID {Id}", id);
            return GenerateResponse<IEnumerable<EmployeeDetail>>(null, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<ApiResponse<int>> CreateEmployee(EmployeeDetail employeeDetail)
    {
        try
        {
            if (!Validation.ValidateEmployeeData(employeeDetail))
            {
                return GenerateResponse(0, ResponseStatus.Error, ErrorCode.ValidationFailed);
            }
            var empId = await _employeeService.Insert(employeeDetail);
            if(empId > 0)
            {
                return GenerateResponse(empId);
            }
            else
            {
                return GenerateResponse(0, ResponseStatus.Error, ErrorCode.NotFound);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating employee");
            return GenerateResponse(0, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ApiResponse<int>> UpdateEmployee(int id, EmployeeDetail employeeDetail)
    {
        try
        {
            if (id != employeeDetail.Id)
            {
                return GenerateResponse<int>(0, ResponseStatus.Fail, ErrorCode.BadRequest);
            }
            if (!Validation.ValidateEmployeeData(employeeDetail))
            {
                return GenerateResponse(0, ResponseStatus.Error, ErrorCode.ValidationFailed);
            }
            var rowsAffected = await _employeeService.Update(employeeDetail);
            if (rowsAffected > 0)
            {
                return GenerateResponse<int>(rowsAffected);
            }
            else
            {
                return GenerateResponse<int>(0, ResponseStatus.Error, ErrorCode.NotFound);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating employee");
            return GenerateResponse<int>(0, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<ApiResponse<int>> UpdateRow(int id, [FromBody] JsonPatchDocument<EmployeeDetail> patchDocument)
    {
        try
        {
            var rowsAffected = await _employeeService.UpdateRow(id, patchDocument);
            if (rowsAffected > 0)
            {
                return GenerateResponse(rowsAffected);
            }
            else
            {
                return GenerateResponse<int>(0, ResponseStatus.Error, ErrorCode.NotFound);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating row of an employee");
            return GenerateResponse<int>(0, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ApiResponse<int>> DeleteEmployee(int id)
    {
        try
        {
            var employee = await _employeeService.Delete(id);
            if (employee > 0)
            {
                return GenerateResponse(0);
            }
            else
            {
                return GenerateResponse(0, ResponseStatus.Error, ErrorCode.NotFound);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting employee");
            return GenerateResponse(0, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }

    [HttpGet("filter")]
    public async Task<ApiResponse<IEnumerable<EmployeeDetail>>> FilterEmployees([FromQuery] EmployeeFilter filters)
    {
        try
        {
            var filteredEmployees = await _employeeService.Filter(filters);
            return GenerateResponse(filteredEmployees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while filtering employee");
            return GenerateResponse<IEnumerable<EmployeeDetail>>(null, ResponseStatus.Error, ErrorCode.InternalServerError);
        }
    }
}
