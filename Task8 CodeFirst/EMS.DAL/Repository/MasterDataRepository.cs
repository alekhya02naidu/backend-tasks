using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EMS.DB.Models;
using EMS.DAL.Interfaces;

namespace EMS.DAL.Repository;

public class MasterDataRepository : IMasterDataRepository
{
    private readonly EMSDbContext _dbContext;

    public MasterDataRepository(EMSDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Location> GetLocationFromName(string locationName)
    {
        return await _dbContext.Locations.FirstOrDefaultAsync(l => l.Name == locationName);
    }

    public async Task<Department> GetDepartmentFromName(string departmentName)
    {
        return await _dbContext.Departments.FirstOrDefaultAsync(d => d.Name == departmentName);
    }

    public async Task<Manager> GetManagerFromName(string managerName)
    {
        var managerEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.FirstName == managerName && e.IsManager);
        if (managerEmployee != null)
        {
            return new Manager { Id = managerEmployee.Id, FirstName = managerEmployee.FirstName };
        }
        else
        {
            return null; 
        };
    }

    public async Task<Project> GetProjectFromName(string projectName)
    {
        return await _dbContext.Projects.FirstOrDefaultAsync(p => p.Name == projectName);
    }
}
