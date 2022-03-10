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
        public bool UpdateEmployee(UpdateEmployeeDto updateEmployeeDto)
        {
            var result = _employeeService.UpdateEmployee(updateEmployeeDto);
            return result;
        }

        [HttpPost("DeleteEmployee")]
        public bool DeleteEmployee(int id)
        {
            var result = _employeeService.DeleteEmployee(id);
            return result;
        }

        [HttpPost("SearchEmployee")]
        public List<ResponseSearchEmployee> SearchEmployee(RequestSearchEmployee requestSearchEmployee)
        {
            var result = _employeeService.SearchEmployee(requestSearchEmployee);
            return result;
        }
    }
}
