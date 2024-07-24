using System;
using System.Collections.Generic;
using EMS.DB.Models;

namespace EMS.DAL.Interfaces;

public interface IRolesRepository
{
    Task<IEnumerable<Role>> GetAllRoles();
    Task<Role> GetRoleFromName(string rolenNme);
    Task<Role> GetRoleById(int id);
    Task<int> AddRole(Role role);
}