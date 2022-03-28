using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebHRM.DTO;
using WebHRM.Interface;
using WebHRM.Models;

namespace WebHRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeInformationController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeInformationController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("AddEmployee")]
        public EmployeeInformationDto AddEmployee(EmployeeDto employeeDto)
        {
            var result = _employeeService.AddEmployee(employeeDto);
            return result;
        }

        [HttpPost("UpdateEmployee")]
        public ResponseUpdateEmployee UpdateEmployee(UpdateEmployeeDto updateEmployeeDto)
        {
            var result = _employeeService.UpdateEmployee(updateEmployeeDto);
            return result;
        }

        [HttpDelete("DeleteEmployee")]
        public bool DeleteEmployee(int id, int eraserId)
        {
            var result = _employeeService.DeleteEmployee(id, eraserId);
            return result;
        }

        [HttpPost("SearchEmployee")]
        public ListResponseSearchEmployee SearchEmployee(RequestSearchEmployee requestSearchEmployee)
        {
            var result = _employeeService.SearchEmployee(requestSearchEmployee);
            return result;
        }

        [HttpGet("GetEmployeeeById")]
        public ResponseSearchEmployee GetEmployeeeById(int id)
        {
            var result = _employeeService.GetEmployeeeById(id);
            return result;
        }

        [HttpGet("GetAllEmployeee")]
        public ListResponseSearchEmployee GetAllEmployeee()
        {
            var result = _employeeService.GetAllEmployeee();
            return result;
        }

        [HttpGet("GetEmployeeeByIdUseStoreProcedure")]
        public IEnumerable<EmployeeInformation> GetEmployeeeByIdUseStoreProcedure(int id)
        {
            var result = _employeeService.GetEmployeeeByIdUseStoreProcedure (id);
            return result;
        }

        [HttpGet("GetCountEmployeee")]
        public int GetCountEmployeee()
        {
            var result = _employeeService.GetCountEmployeee();
            return result;
        }
        [HttpPost("GetAge")]
        public int Age(DateTime birthday)
        {
            var result = _employeeService.Age(birthday);
            return result;
        }
        
    }
}
