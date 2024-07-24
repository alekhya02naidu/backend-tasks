using System;
using System.Collections.Generic;
using EMS.DAL.DTO;
using EMS.DB.Models;

namespace EMS.BAL.Interfaces;

public interface IAuthenticationService
{
    Task<int> RegisterEmployee(RegisterDTO registerEmp);
    Task<EmployeeDetail> GetEmployeeByEmail(string email);
    Task<bool> Authenticate(string email, string password);
}