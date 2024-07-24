using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMS.DB.Models;

public class Location
{
    [Key]
    public int Id { get; set;}
    public string Name { get; set;}
}