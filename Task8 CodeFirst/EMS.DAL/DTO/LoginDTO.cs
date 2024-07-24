using System;
using System.ComponentModel.DataAnnotations;

namespace EMS.DAL.DTO;

public class LoginDTO
{
    [Required(ErrorMessage = "Password is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public required string Email { get; set;}

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set;}
}