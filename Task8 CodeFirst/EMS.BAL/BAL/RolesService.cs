using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EMS.BAL.Interfaces;
using EMS.DAL.Interfaces;
using EMS.DB.Models;

namespace EMS.BAL.BAL;
public class RolesService : IRolesService
{
    private readonly IRolesRepository _rolesRepo;

    public RolesService(IRolesRepository rolesRepo)
    {
        _rolesRepo = rolesRepo;
    }

    public async Task<IEnumerable<Role>> GetAllRoles()
    {
        return await _rolesRepo.GetAllRoles();
    }

    public async Task<Role> GetRoleFromName(string roleName)
    {
        return await _rolesRepo.GetRoleFromName(roleName);
    }

    public async Task<Role> GetRoleById(int id)
    {
        return await _rolesRepo.GetRoleById(id);
    }

    public async Task<int> AddRole(Role role)
    {
        return await _rolesRepo.AddRole(role);
    }
}

