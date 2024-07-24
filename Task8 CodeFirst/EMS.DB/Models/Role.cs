using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMS.DB.Models;

public partial class Role
{
    [Key]
    public int Id { get; set; }
    public int? DepartmentId { get; set; }
    public string Name { get; set; } = null!;
    // public virtual Department? Department { get; set; }
}
