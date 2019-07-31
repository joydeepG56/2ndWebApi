using EmployeeDataAccess2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2ndWebApi
{
    public class EmployeeSecurity
    {
        public static bool Login(string username, string password)
        {
            using (Test2Entities _db = new Test2Entities())
            {
                return _db.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
            }
        }
    }
}