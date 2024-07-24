using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace EMS.DB.Models;

public class EMSDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<EmployeeDetail> EmployeeDetails { get; set; }

    public EMSDbContext()
    {
    }    

    public EMSDbContext(DbContextOptions<EMSDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
                .HasKey(e => e.Id);
        
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Uid)
            .IsUnique();

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany()
            .HasForeignKey(e => e.DepartmentId)
            .IsRequired(false);
        
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Role)
            .WithMany()
            .HasForeignKey(e => e.RoleId)
            .IsRequired(false);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .IsRequired(false);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Project)
            .WithMany()
            .HasForeignKey(e => e.ProjectId)
            .IsRequired(false);

        modelBuilder.Entity<Location>()
            .HasKey(l => l.Id);
        
        modelBuilder.Entity<Department>()
            .HasKey(d => d.Id);

        modelBuilder.Entity<Role>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Project>()
            .HasKey(p => p.Id);  

        modelBuilder.Entity<Location>().HasData(
                new Location { Id = 1, Name = "Hyderabad" },
                new Location { Id = 2, Name = "US" },
                new Location { Id = 3, Name = "UK" }
            );

        modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "HR" },
                new Department { Id = 2, Name = "PE" },
                new Department { Id = 3, Name = "QA"}

            );

        modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, DepartmentId = 1, Name = "Administrator" },
                new Role { Id = 2, DepartmentId = 2, Name = "Manager" },
                new Role { Id = 3, DepartmentId = 2, Name = "Developer" },
                new Role { Id = 4, DepartmentId = 2, Name = "Intern" },
                new Role { Id = 5, DepartmentId = 3, Name = "Manager" },
                new Role { Id = 6, DepartmentId = 3, Name = "Tester" },
                new Role { Id = 7, DepartmentId = 3, Name = "Intern" }
            );

        modelBuilder.Entity<Project>().HasData(
                new Project { Id = 1, Name = "p1" },
                new Project { Id = 2, Name = "p2" },
                new Project { Id = 3, Name = "p3" }
            );
             
        modelBuilder.Entity<EmployeeDetail>() //mapping to the view
            .ToView("EmployeeDetail");
    }
}
