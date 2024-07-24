using System;
using System.Linq;  
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using EMS.BAL.Interfaces;
using EMS.DAL.Interfaces;
using EMS.DAL.DTO;
using EMS.DB.Models;

namespace EMS.BAL.BAL;
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepo;

    public EmployeeService(IEmployeeRepository employeeRepo)
    {
        _employeeRepo = employeeRepo;
    }

    public async Task<int> Insert(EmployeeDetail employeeDetail)
    {
        return await _employeeRepo.Insert(employeeDetail);
    }

    public async Task<int> Update (EmployeeDetail employeeDetail)   
    {
        return await _employeeRepo.Update(employeeDetail);
    }

    public async Task<int> UpdateRow(int id, JsonPatchDocument<EmployeeDetail> patchDocument)
    {
        return await _employeeRepo.UpdateRow(id, patchDocument);
    }

    public async Task<int> Delete (int id)
    {
        return await _employeeRepo.Delete(id);
    }

    public async Task<EmployeeDetail> GetEmployeeById(int id)
    {
        return await _employeeRepo.GetEmployeeById(id);
    }
    
    public async Task<PaginatedResult<EmployeeDetail>> GetAll(int pageIndex, int pageSize)
    {
        return await _employeeRepo.GetAll(pageIndex, pageSize);
    }

    public async Task<IEnumerable<EmployeeDetail>> Filter(EmployeeFilter filters)
    {
        return await _employeeRepo.Filter(filters);
    }
}