using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EMS.DAL.DTO;
using EMS.DB.Models;

namespace EMS.BAL.Interfaces;

public interface IMasterDataService
{
    Task<Location> GetLocationFromName(string locationName);
    Task<Department> GetDepartmentFromName(string departmentName);
    Task<Manager> GetManagerFromName(string managerName);
    Task<Project> GetProjectFromName(string projectName);
}