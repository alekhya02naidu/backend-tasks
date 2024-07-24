using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using EMS.BAL.Interfaces;
using EMS.DAL.Interfaces;
using EMS.DAL.DTO;
using EMS.DB.Models;

namespace EMS.BAL.BAL;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationRepository _authenticationRepo;

    public AuthenticationService(IAuthenticationRepository authenticationRepo)
    {
        _authenticationRepo = authenticationRepo;
    }

    public async Task<int> RegisterEmployee(RegisterDTO registerEmp)
    {
        return await _authenticationRepo.RegisterEmployee(registerEmp);
    }

    public async Task<EmployeeDetail> GetEmployeeByEmail(string email)
    {
        return await _authenticationRepo.GetEmployeeByEmail(email);
    }

    public async Task<bool> Authenticate(string email, string password)
    {
        return await _authenticationRepo.Authenticate(email, password);
    }
}

