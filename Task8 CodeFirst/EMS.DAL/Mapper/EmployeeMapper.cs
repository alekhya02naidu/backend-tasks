using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EMS.DAL.Interfaces;
using EMS.DAL.DTO;
using EMS.DB.Models;

namespace EMS.DAL.Mapper;

public class EmployeeMapper
{
    public Employee MapEmployeeDtoToEmployee(Location location, Department department, Role role, Manager manager, Project project, EmployeeDetail employeeDetail)
    {
        var employee = new Employee
        {
            Uid = employeeDetail.Uid,
            FirstName = employeeDetail.FirstName,
            LastName = employeeDetail.LastName,
            DOB = employeeDetail.DOB,
            Email = employeeDetail.Email,
            MobileNumber = employeeDetail.MobileNumber,
            JoiningDate = employeeDetail.JoiningDate,
            LocationId = location?.Id,
            DepartmentId = department?.Id,
            RoleId = role?.Id,
            IsManager = employeeDetail.IsManager,
            ManagerId = manager?.Id,
            ProjectId = project?.Id,
        };
        if (employeeDetail.IsManager)
        {
            employee.ManagerId = null;
        }
        else
        {
            employee.ManagerId = manager?.Id;
        }
        return employee;
    }

    public Employee MapRegisterDtoToEmployee(Location location, Department department, Role role, Manager manager, Project project, RegisterDTO registerEmp, string hashedPassword)
    {
        var employee = new Employee
        {
            Uid = registerEmp.Uid,
            FirstName = registerEmp.FirstName,
            LastName = registerEmp.LastName,
            DOB = registerEmp.DOB,
            Email = registerEmp.Email,
            MobileNumber = registerEmp.MobileNumber,
            JoiningDate = registerEmp.JoiningDate,
            LocationId = location?.Id,
            DepartmentId = department?.Id,
            RoleId = role?.Id,
            IsManager = registerEmp.IsManager,
            ManagerId = manager?.Id,
            ProjectId = project?.Id,
            Password = hashedPassword
        };
        return employee;
    }

    public Employee MapEmployeeDtoToExistingEmployee(Location location, Department department, Role role, Manager manager, Project project, EmployeeDetail employeeDetail, Employee employee)
    {
        employee.Uid = employeeDetail.Uid;
        employee.FirstName = employeeDetail.FirstName;
        employee.LastName = employeeDetail.LastName;
        employee.DOB = employeeDetail.DOB;
        employee.Email = employeeDetail.Email;
        employee.MobileNumber = employeeDetail.MobileNumber;
        employee.JoiningDate = employeeDetail.JoiningDate;
        employee.LocationId = location?.Id;
        employee.DepartmentId = department?.Id;
        employee.RoleId = role?.Id;
        employee.IsManager = employeeDetail.IsManager;
        employee.ManagerId = employeeDetail.IsManager ? null : manager?.Id;
        employee.ProjectId = project?.Id;
        return employee;
    }    
    
    public EmployeeDetail MapEmployeeToEmployeeDTO(Employee employee)
    {
        return new EmployeeDetail
        {
            Id = employee.Id,
            Uid = employee.Uid,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            DOB = employee.DOB,
            Email = employee.Email,
            MobileNumber = employee.MobileNumber,
            JoiningDate = employee.JoiningDate,
            LocationName = employee.Location != null ? employee.Location.Name : null,
            DepartmentName = employee.Department != null ? employee.Department.Name : null,
            RoleName = employee.Role != null ? employee.Role.Name : null,
            IsManager = employee.IsManager,
            ManagerName = employee.Manager != null ? (employee.Manager.FirstName + " " + employee.Manager.LastName) : null,
            ProjectName = employee.Project != null ? employee.Project.Name : null
        };
    }
}