using System;
using EMS.DAL.DTO;
using EMS.DB.Models;

namespace EMS.ValidationHelper;

public static class Validation
{
    public static bool ValidateEmployeeData(EmployeeDetail employeeDetail)
    {   
        if (string.IsNullOrEmpty(employeeDetail.Uid) || !employeeDetail.Uid.StartsWith("TZ") || !employeeDetail.Uid.Substring(2).All(char.IsDigit))
        {
            return false;
        }
        if(string.IsNullOrEmpty(employeeDetail.FirstName))
        {
            return false;
        }
        if(string.IsNullOrEmpty(employeeDetail.LastName))
        {
            return false;
        }
        if (!string.IsNullOrEmpty(employeeDetail.MobileNumber))
        {
            if(employeeDetail.MobileNumber.Length != 10 || !employeeDetail.MobileNumber.All(char.IsDigit))
            {
                return false;
            }
        }
        if (string.IsNullOrEmpty(employeeDetail.Email) || !employeeDetail.Email.Contains("@"))
        {
            return false;
        }
        return true;
    }

    public static bool ValidateRegisterEmployeeData(RegisterDTO registerEmp)
    {   
        if (string.IsNullOrEmpty(registerEmp.Uid) || !registerEmp.Uid.StartsWith("TZ") || !registerEmp.Uid.Substring(2).All(char.IsDigit))
        {
            return false;
        }
        if(string.IsNullOrEmpty(registerEmp.FirstName))
        {
            return false;
        }
        if(string.IsNullOrEmpty(registerEmp.LastName))
        {
            return false;
        }
        if (!string.IsNullOrEmpty(registerEmp.MobileNumber))
        {
            if(registerEmp.MobileNumber.Length != 10 || !registerEmp.MobileNumber.All(char.IsDigit))
            {
                return false;
            }
        }
        if (string.IsNullOrEmpty(registerEmp.Email) || !registerEmp.Email.Contains("@"))
        {
            return false;
        }
        return true;
    }   
}