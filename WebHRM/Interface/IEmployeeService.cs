using System.Text;
using WebHRM.DTO;
using WebHRM.Models;

namespace WebHRM.Interface
{

    public interface IEmployeeService
    {
        EmployeeInformationDto AddEmployee(EmployeeDto employeeDto);
        ResponseUpdateEmployee UpdateEmployee(UpdateEmployeeDto updateEmployeeDto);
        bool DeleteEmployee(int id);
        //List<Accounts> GetAllEmployees();
        //ResponsePageAccountDto GetEmployees(AccountDto accountDto);
        ListResponseSearchEmployee SearchEmployee(RequestSearchEmployee requestSearchEmployee);
        bool isPhoneNumber(string phoneNumber);
        ResponseSearchEmployee GetEmployeeeById(int id);
        int GetCountEmployeee();
    }
}
