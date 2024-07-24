using System.Collections.Generic;
using System.Threading.Tasks;
using EMS.DB.Models;

namespace EMS.BAL.Interfaces;

public interface IRolesService
{
    Task<IEnumerable<Role>> GetAllRoles();
    Task<Role> GetRoleFromName(string roleName);
    Task<Role> GetRoleById(int id);
    Task<int> AddRole(Role role);
}
