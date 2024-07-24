using System;
using System.Collections.Generic;
using EMS.DAL.DTO;
using EMS.DB.Models;

namespace EMS.DAL.Interfaces;

public interface IMasterDataRepository
{
    Task<Location> GetLocationFromName(string locationName);
    Task<Department> GetDepartmentFromName(string departmentName);
    Task<Manager> GetManagerFromName(string managerName);
    Task<Project> GetProjectFromName(string projectName);
}