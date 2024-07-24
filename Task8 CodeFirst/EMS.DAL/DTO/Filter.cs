using System;
using System.Collections.Generic;
using EMS.DB.Models;

namespace EMS.DAL.DTO;

public partial class EmployeeFilter
{
    public int? Id { get; set; }
    public string? FirstName { get; set; } = null!;
    public string? LocationName { get; set; }
    public string? DepartmentName { get; set; }
}
