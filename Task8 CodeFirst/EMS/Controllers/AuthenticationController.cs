using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using EMS.ValidationHelper;
using EMS.ResponseModel.Enums;
using EMS.ResponseModel;
using EMS.BAL.BAL;
using EMS.BAL.Interfaces;
using EMS.DB.Models;
using EMS.DAL.DTO;

namespace EMS.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IAuthenticationService authenticationService, IConfiguration configuration, ILogger<AuthenticationController> logger)
    {
        _authenticationService = authenticationService;
        _configuration = configuration;
        _logger = logger;
    }

    private ApiResponse<T> GenerateResponse<T>(T data, ResponseStatus status = ResponseStatus.Success, ErrorCode? errorCode = null)
    {
        return new ApiResponse<T>
        (
            status,
            data,
            errorCode
        );
    }

    [AllowAnonymous]        
    [HttpPost("Login")]
    public async Task<ApiResponse<string>> Login(LoginDTO loginDTO)
    {
        var isAuthenticated = await _authenticationService.Authenticate(loginDTO.Email, loginDTO.Password);
        if (!isAuthenticated)
        {
            _logger.LogError("Invalid email or password");
            return GenerateResponse<string>(null, ResponseStatus.Fail, ErrorCode.UnAuthorized);
        }
        var employeeDetail = await _authenticationService.GetEmployeeByEmail(loginDTO.Email);
        if (employeeDetail == null)
        {
            return GenerateResponse<string>(null, ResponseStatus.Fail, ErrorCode.NotFound);
        }
        var tokenString = await GenerateToken(employeeDetail.Id, employeeDetail.Email, employeeDetail.RoleName);
        return GenerateResponse(tokenString);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<ApiResponse<int>> Register(RegisterDTO registerEmp)
    {
        try
        {
            if(!Validation.ValidateRegisterEmployeeData(registerEmp))
            {
                return GenerateResponse(0, ResponseStatus.Error, ErrorCode.ValidationFailed);
            }
            var id = await _authenticationService.RegisterEmployee(registerEmp);
            return GenerateResponse(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while registering a new employee");
            return GenerateResponse(0, ResponseStatus.Fail, ErrorCode.BadRequest);
        }
    }

    private async Task<string> GenerateToken(int id, string email, string role)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, email),
            new Claim("Id", id.ToString()),
            new Claim("role", role)
        };

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: signingCredentials
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}