using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using System.Security.Cryptography;
using System.Linq;
using EMS.DAL.Interfaces;
using EMS.DAL.DTO;
using EMS.DAL.Mapper;
using EMS.DB.Models;

namespace EMS.DAL.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EMSDbContext _dbContext;
    private readonly IMasterDataRepository _masterDataRepo;
    private readonly IRolesRepository _rolesRepo;
    private readonly EmployeeMapper _mapper;

    public EmployeeRepository(EMSDbContext dbContext, IMasterDataRepository masterDataRepo, IRolesRepository rolesRepo, EmployeeMapper mapper)
    {
        _dbContext = dbContext;
        _masterDataRepo = masterDataRepo;
        _rolesRepo = rolesRepo;
        _mapper = mapper;
    }

    public async Task<int> Insert(EmployeeDetail employeeDetail)
    {
        var location = await _masterDataRepo.GetLocationFromName(employeeDetail.LocationName);
        var department = await _masterDataRepo.GetDepartmentFromName(employeeDetail.DepartmentName);
        var role = await _rolesRepo.GetRoleFromName(employeeDetail.RoleName);
        var manager = await _masterDataRepo.GetManagerFromName(employeeDetail.ManagerName);
        var project = await _masterDataRepo.GetProjectFromName(employeeDetail.ProjectName);

        var employee = _mapper.MapEmployeeDtoToEmployee(location, department, role, manager, project, employeeDetail);
        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();
        return employee.Id;
    }

    public async Task<int> Update(EmployeeDetail employeeDetail)
    {
        var existingEmployee = await _dbContext.Employees.FindAsync(employeeDetail.Id);
        if (existingEmployee == null)
        {
            return -1;
        }
        var location = await _masterDataRepo.GetLocationFromName(employeeDetail.LocationName);
        var department = await _masterDataRepo.GetDepartmentFromName(employeeDetail.DepartmentName);
        var role = await _rolesRepo.GetRoleFromName(employeeDetail.RoleName);
        var manager = await _masterDataRepo.GetManagerFromName(employeeDetail.ManagerName);
        var project = await _masterDataRepo.GetProjectFromName(employeeDetail.ProjectName);
        
        var updatedEmployee = _mapper.MapEmployeeDtoToExistingEmployee(location, department, role, manager, project, employeeDetail, existingEmployee);
        await _dbContext.SaveChangesAsync();
        return updatedEmployee.Id;
    }

    public async Task<int> UpdateRow(int id, JsonPatchDocument<EmployeeDetail> patchDocument)
    {
        var existingEmployee = await _dbContext.Employees.FindAsync(id);
        if (existingEmployee == null)
        {
            return 0; 
        }
        var employeeDetail = _mapper.MapEmployeeToEmployeeDTO(existingEmployee);

        patchDocument.ApplyTo(employeeDetail);

        var location = await _masterDataRepo.GetLocationFromName(employeeDetail.LocationName);
        var department = await _masterDataRepo.GetDepartmentFromName(employeeDetail.DepartmentName);
        var role = await _rolesRepo.GetRoleFromName(employeeDetail.RoleName);
        var manager = await _masterDataRepo.GetManagerFromName(employeeDetail.ManagerName);
        var project = await _masterDataRepo.GetProjectFromName(employeeDetail.ProjectName);

        var updatedEmployee = _mapper.MapEmployeeDtoToExistingEmployee(location, department, role, manager, project, employeeDetail, existingEmployee);
        await _dbContext.SaveChangesAsync();
        return updatedEmployee.Id;
    }

    public async Task<int> Delete(int id)
    {
        var employeeToBeDeleted = await _dbContext.Employees.FindAsync(id);
        if(employeeToBeDeleted == null)
        {
            return 0;
        }
        _dbContext.Employees.Remove(employeeToBeDeleted);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<EmployeeDetail> GetEmployeeById(int id)
    {
        var existingEmployee = await _dbContext.EmployeeDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
        if (existingEmployee == null)
        {
            return null;
        }
        return existingEmployee; 
    }

    public async Task<PaginatedResult<EmployeeDetail>> GetAll(int pageIndex, int pageSize)
    {
        var query = _dbContext.EmployeeDetails.AsQueryable().AsNoTracking();
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();

        return new PaginatedResult<EmployeeDetail>(items, pageIndex, pageSize, totalCount);
    }
    
    public async Task<IEnumerable<EmployeeDetail>> Filter(EmployeeFilter filter)
    {
        var query = _dbContext.Employees.AsQueryable();
        if (filter.Id.HasValue)
        {
            query = query.Where(e => e.Id == filter.Id);
        }
        if (!string.IsNullOrEmpty(filter.FirstName))
        {
            query = query.Where(e => e.FirstName.Contains(filter.FirstName));
        }
        if (!string.IsNullOrEmpty(filter.LocationName))
        {
            var location = await _masterDataRepo.GetLocationFromName(filter.LocationName);
            if (location != null)
            {
                query = query.Where(e => e.LocationId == location.Id);
            }
        }
        if (!string.IsNullOrEmpty(filter.DepartmentName))
        {
            var department = await _masterDataRepo.GetDepartmentFromName(filter.DepartmentName);
            if (department != null)
            {
                query = query.Where(e => e.DepartmentId == department.Id);
            }
        }
        var employees = await query.Include(e => e.Location)
                             .Include(e => e.Department)
                             .Include(e => e.Role)
                             .Include(e => e.Project)
                             .Include(e => e.Manager)
                             .ToListAsync();

        return employees
            .Select(emp => _mapper.MapEmployeeToEmployeeDTO(emp)).ToList();
    }
}