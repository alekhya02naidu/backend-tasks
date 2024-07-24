using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMS.DB.Models;

public class EmployeeDetail
{
    public int Id { get; set; }

    [Required(ErrorMessage = "UID is required")]
    [RegularExpression(@"^TZ\d{4}$", ErrorMessage = "UID must start with 'TZ' followed by four numeric characters")]
    public string Uid { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; }   
    
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Date of Birth is required")]
    [DataType(DataType.Date)]
    public DateTime DOB { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? MobileNumber { get; set; }

    [Required(ErrorMessage = "Joining date is required")]
    [DataType(DataType.Date)]
    public DateTime JoiningDate { get; set; }
    public string? LocationName { get; set; }
    public string? DepartmentName { get; set; }
    public string? RoleName { get; set; }
    public bool IsManager { get; set; }
    public string? ManagerName { get; set; }
    public string? ProjectName { get; set; }
}