using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE VIEW EmployeeDetail AS
                SELECT e.Id, e.Uid, e.FirstName, e.LastName, 
                    e.DOB, e.Email, e.MobileNumber, e.JoiningDate,
                    l.Name AS LocationName,
                    d.Name AS DepartmentName,
                    r.Name AS RoleName,
                    e.IsManager,
                    m.FirstName AS ManagerName,
                    p.Name AS ProjectName
                FROM Employees e
                LEFT JOIN Locations l ON e.LocationId = l.Id
                LEFT JOIN Departments d ON e.DepartmentId = d.Id
                LEFT JOIN Roles r ON e.RoleId = r.Id
                LEFT JOIN Projects p ON e.ProjectId = p.Id
                LEFT JOIN Employees m ON e.ManagerId = m.Id;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
