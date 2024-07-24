using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Linq;
using BCrypt.Net;
using EMS.DAL.Interfaces;
using EMS.DAL.DTO;
using EMS.DAL.Mapper; 
using EMS.DB.Models;

namespace EMS.DAL.Repository;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly EMSDbContext _dbContext;
    private readonly IMasterDataRepository _masterDataRepo;
    private readonly IRolesRepository _rolesRepo;
    private readonly EmployeeMapper _mapper;

    public AuthenticationRepository(EMSDbContext dbContext,IMasterDataRepository masterDataRepo, IRolesRepository rolesRepo, EmployeeMapper mapper)
    {
        _dbContext = dbContext;
        _masterDataRepo = masterDataRepo;
        _rolesRepo = rolesRepo;
        _mapper = mapper;
    }

    public async Task<int> RegisterEmployee(RegisterDTO registerEmp)
    {   
        var location = await _masterDataRepo.GetLocationFromName(registerEmp.LocationName);
        var department = await _masterDataRepo.GetDepartmentFromName(registerEmp.DepartmentName);
        var role = await _rolesRepo.GetRoleFromName(registerEmp.RoleName);
        var manager = await _masterDataRepo.GetManagerFromName(registerEmp.ManagerName);
        var project = await _masterDataRepo.GetProjectFromName(registerEmp.ProjectName);
        string hashedPassword = HashPassword(registerEmp.Password);

        var employee = _mapper.MapRegisterDtoToEmployee(location,department, role, manager, project, registerEmp, hashedPassword);
        
        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();
        return employee.Id;
    }

    public async Task<EmployeeDetail> GetEmployeeByEmail(string email)
    {
        var employee = await _dbContext.Employees
                .Include(e => e.Location)
                .Include(e => e.Department)
                .Include(e => e.Role)
                .Include(e => e.Manager)
                .Include(e => e.Project)
                .FirstOrDefaultAsync(e => e.Email == email);

        return employee != null ? _mapper.MapEmployeeToEmployeeDTO(employee) : null;
    }

    public async Task<bool> Authenticate(string email, string password)
    {
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);
        if (employee == null)
        {
            return false;
        }
        return VerifyPassword(password, employee.Password);
    }
    
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}

