using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EMS.DB.Models;
using EMS.DAL.Interfaces;

namespace EMS.DAL.Repository;
public class RolesRepository : IRolesRepository
{
    private readonly EMSDbContext _dbContext;

    public RolesRepository(EMSDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Role>> GetAllRoles()
    {
        return await _dbContext.Roles.ToListAsync();
    }

    public async Task<Role> GetRoleFromName(string roleName)
    {
        return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
    }

    public async Task<Role> GetRoleById(int id)
    {
        return await _dbContext.Roles.FindAsync(id);
    }

    public async Task<int> AddRole(Role role)
    {
        _dbContext.Roles.Add(role);
        await _dbContext.SaveChangesAsync();
        return role.Id;
    }
}

