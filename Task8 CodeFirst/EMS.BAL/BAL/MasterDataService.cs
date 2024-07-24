using System.Collections.Generic;
using System.Threading.Tasks;
using EMS.BAL.Interfaces;
using EMS.DAL.Interfaces;
using EMS.DB.Models;

namespace EMS.BAL.BAL;
public class MasterDataService : IMasterDataService
{
    private readonly IMasterDataRepository _masterDataRepo;

    public MasterDataService(IMasterDataRepository masterDataRepo)
    {
        _masterDataRepo = masterDataRepo;
    }

    public async Task<Location> GetLocationFromName(string locationName)
    {
        return await _masterDataRepo.GetLocationFromName(locationName);
    }

    public async Task<Department> GetDepartmentFromName(string departmentName)
    {
        return await _masterDataRepo.GetDepartmentFromName(departmentName);
    }

    public async Task<Manager> GetManagerFromName(string managerName)
    {
        return await _masterDataRepo.GetManagerFromName(managerName);
    }

    public async Task<Project> GetProjectFromName(string projectName)
    {
        return await _masterDataRepo.GetProjectFromName(projectName);
    }
}
