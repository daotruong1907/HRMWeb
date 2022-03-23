using System.Text;
using WebHRM.DTO;
using WebHRM.Models;

namespace WebHRM.Interface
{

    public interface IEmployeeService
    {
        EmployeeInformationDto AddEmployee(EmployeeDto employeeDto);

        ResponseUpdateEmployee UpdateEmployee(UpdateEmployeeDto updateEmployeeDto);

        bool DeleteEmployee(int id, int eraserId);

        ListResponseSearchEmployee SearchEmployee(RequestSearchEmployee requestSearchEmployee);

        bool IsPhoneNumber(string phoneNumber);

        ResponseSearchEmployee GetEmployeeeById(int id);

        int GetCountEmployeee();
        int Age(DateTime birthday);
    }
}
