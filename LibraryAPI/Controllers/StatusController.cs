using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LibraryAPI.Controllers
{
    public class StatusController : Controller
    {
        ISystemTime Clock;
        IConfiguration Config;

        public StatusController(ISystemTime clock, IConfiguration config)
        {
            Clock = clock;
            Config = config;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("employees")]
        public ActionResult Hire([FromBody]EmployeeCreateRequest employeeToHire)
        {
            //1. Validate it..if not valid...send 400 back to caller.
            //2. Add to database.
            var employeeResponse = new EmployeeResponse
            {
                Id = new Random().Next(10, 10000),
                Name = employeeToHire.Name,
                Department = employeeToHire.Department,
                HireDate = DateTime.Now,
                StartingSalary = employeeToHire.StartingSalary
            };
            //3. Return a 201 Created status code.
            //4. Include in the response a link to the new resource.
            //5. Usually, send them a copy of what they get with a GET request on the above resource.

            return CreatedAtRoute("employees#getemployee", new { employeeId = employeeResponse.Id }, employeeResponse);
        }

        [HttpGet("whoami")]
        public ActionResult whoami([FromHeader(Name = "User-Agent")] string userAgent)
        {
            return Ok($"I see you are running {userAgent})");
        }

        //GET /employees
        //GET /employees?department=DEV
        [HttpGet("employees")]
        public ActionResult GetAllEmplyee([FromQuery] string department = "All")
        {
            return Ok($"All the employees (filtered on {department})");
        }

        //GET /employees/938938
        [HttpGet("employees/{employeeId:int}", Name="employees#getemployee")]
        public ActionResult GetAnEmplyee(int employeeId)
        {
            //goto the database and get employee data
            var employeeResponse = new EmployeeResponse
            {
                Id = employeeId,
                Name = "Bob Smith",
                Department = "DEV",
                HireDate = DateTime.Now.AddDays(-399),
                StartingSalary = 250000
            };

            return Ok(employeeResponse);
        }

        //GET /status -> StatusController#GetStatus
        [HttpGet("status")]
        public ActionResult GetStatus()
        {
            var status = new StatusResponse
            {
                Message = "Looks good on my end. Party On !",
                CheckedBy = Config.GetValue<string>("onCall"),
                WhenChecked = Clock.GetCurrent()
            };

            return Ok(status);
        }
    }

    public class StatusResponse
    {
        public string Message { get; set; }
        public string CheckedBy { get; set; }
        public DateTime WhenChecked { get; set; }
    }

    public class EmployeeCreateRequest
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal StartingSalary { get; set; }
    }

    public class EmployeeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal StartingSalary { get; set; }
        public DateTime HireDate { get; set; }
    }
}
