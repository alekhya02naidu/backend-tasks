using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMS.DB.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }
    public string Uid { get; set; }
    public string FirstName { get; set; }   
    public string LastName { get; set; }
    public DateTime DOB { get; set; }
    public string Email { get; set; }
    public string? MobileNumber { get; set; }
    public DateTime JoiningDate { get; set; }
    public int? LocationId { get; set; }
    public int? DepartmentId { get; set; }
    public int? RoleId { get; set; }
    public bool IsManager { get; set; }
    public int? ManagerId { get; set; }
    public int? ProjectId { get; set; }
    public string? Password { get; set; }
    public virtual Location? Location { get; set; }
    public virtual Department? Department{ get; set; }
    public virtual Role? Role { get; set; }
    public virtual Employee? Manager { get; set; }
    public virtual Project? Project{ get; set; }
}