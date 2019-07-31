using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using EmployeeDataAccess2;

namespace _2ndWebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        [BasicAuthenticationAttributes]
        public HttpResponseMessage GetEmployees(string gender="All")
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            using (Test2Entities _db = new Test2Entities())
            {
                switch(username.ToLower())
                {
                    case "all" :
                        return Request.CreateResponse(HttpStatusCode.OK, _db.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, _db.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, _db.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateResponse(HttpStatusCode.BadGateway, "Value of Gender must be All, Male or Female. " + gender + " is invalid");
                }
            }
        }

        public HttpResponseMessage GetEmployee(int id)
        {
            using (Test2Entities _db = new Test2Entities())
            {
                var data = _db.Employees.FirstOrDefault(e => e.id == id);

                if (data != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id    " + id + " is not found");
                }
            }
        }

        public HttpResponseMessage Post([FromBody]Employee emp)
        {
            try
            {
                using (Test2Entities _db = new Test2Entities())
                {
                    _db.Employees.Add(emp);
                    _db.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, emp);
                    message.Headers.Location = new Uri(Request.RequestUri + emp.id.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex); 
            }
        }

        public HttpResponseMessage PutEmployee(int id,[FromBody]Employee emp)
        {
            try
            {
                using (Test2Entities _db = new Test2Entities())
                {
                    var data = _db.Employees.Where(e => e.id == id).SingleOrDefault();

                    if (data != null)
                    {
                        data.id = emp.id;
                        data.Name = emp.Name;
                        data.Salary = emp.Salary;
                        data.Gender = emp.Gender;
                        data.City = emp.City;

                        _db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id " + id.ToString() + " is not found");
                    }

                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }

        public HttpResponseMessage DeleteEmployee(int id)
        {
            try
            {
                using (Test2Entities _db = new Test2Entities())
                {
                    var data = _db.Employees.FirstOrDefault(e => e.id == id);
                    if (data != null)
                    {
                        _db.Employees.Remove(data);
                        _db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with id " + id.ToString() + " is not found");
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }
    }
}
