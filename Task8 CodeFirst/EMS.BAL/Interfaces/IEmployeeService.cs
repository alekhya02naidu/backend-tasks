using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using EMS.DB.Models;
using EMS.DAL.DTO;

namespace EMS.BAL.Interfaces;

public interface IEmployeeService
{
    Task<int> Insert(EmployeeDetail employeeDetail);
    Task<int> Update(EmployeeDetail employeeDetail);
    Task<int> UpdateRow(int id, JsonPatchDocument<EmployeeDetail> patchDocument);
    Task<int> Delete(int id);
    Task<EmployeeDetail> GetEmployeeById(int id);
    Task<PaginatedResult<EmployeeDetail>> GetAll(int pageIndex, int pageSize);
    Task<IEnumerable<EmployeeDetail>> Filter(EmployeeFilter filters);
}