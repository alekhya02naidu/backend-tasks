using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using EMS.BAL.Interfaces;
using EMS.DB.Models;
using EMS.ResponseModel.Enums;
using EMS.ResponseModel;
using EMS.DAL.DTO;

namespace EMS.Controllers;

[ApiController]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRolesService _rolesService;
    private readonly ILogger<RolesController> _logger;

    public RolesController(IRolesService rolesService, ILogger<RolesController> logger)
    {
        _rolesService = rolesService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<Role>>> GetAllRoles()
    {
        try
        {
            var roles = await _rolesService.GetAllRoles();
            if (roles == null || !roles.Any())
            {
                return new ApiResponse<IEnumerable<Role>>(ResponseStatus.Error, null, ErrorCode.NotFound);
            }
            return new ApiResponse<IEnumerable<Role>>(ResponseStatus.Success, roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all roles");
            return new ApiResponse<IEnumerable<Role>>(ResponseStatus.Error, null, ErrorCode.InternalServerError);
        }
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<Role>> GetRole(int id)
    {
        try
        {
            var role = await _rolesService.GetRoleById(id);
            if (role == null)
            {
                return new ApiResponse<Role>(ResponseStatus.Error, null, ErrorCode.NotFound);
            }
            return new ApiResponse<Role>(ResponseStatus.Success, role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching a role");
            return new ApiResponse<Role>(ResponseStatus.Error, null, ErrorCode.InternalServerError);
        }
    }

    [Authorize(Roles="Administrator")]
    [HttpPost]
    public async Task<ApiResponse<int>> CreateRole(Role role)
    {
        try
        {
            var roleId = await _rolesService.AddRole(role);
            if (roleId > 0)
            {
                return new ApiResponse<int>(ResponseStatus.Success, roleId);
            }
            else
            {
                return new ApiResponse<int>(ResponseStatus.Error, 0, ErrorCode.BadRequest);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating Role");
            return new ApiResponse<int>(ResponseStatus.Error, 0, ErrorCode.InternalServerError);
        }
    }
}
