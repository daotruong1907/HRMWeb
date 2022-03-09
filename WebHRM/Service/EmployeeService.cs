using WebHRM.DTO;
using WebHRM.Interface;
using WebHRM.Models;

namespace WebHRM.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HRMContext _hRMWebContext;
        private readonly IAccountsService _accountsService;
        public EmployeeService(HRMContext hRMContext, IAccountsService accountsService)
        {
            _hRMWebContext = hRMContext;
            _accountsService = accountsService;
        }

        public EmployeeInformation AddEmployee(EmployeeDto employeeDto)
        {
            var employeeInformation = new EmployeeInformation();
            if (employeeDto != null)
            {
                var newEmployee = new EmployeeInformation
                {
                    Name = employeeDto.Name,
                    BirthDay = employeeDto.BirthDay,
                    Sex = employeeDto.Sex,
                    PhoneNumber = employeeDto.PhoneNumber,
                    Email = employeeDto.Email,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    Creator = employeeDto.Creator,
                };
                _hRMWebContext.EmployeeInformation.Add(newEmployee);
                _hRMWebContext.SaveChanges();
                var employee = _hRMWebContext.EmployeeInformation.Where(x => x.PhoneNumber == employeeDto.PhoneNumber && x.DeleteAt == null).FirstOrDefault();
                if (employee != null)
                {
                    var addAccount = new AddAccountDto
                    {
                        CreateAt = DateTime.Now,
                        Creator = employeeDto.Creator,
                        Id = employee.Id,
                        PassWord = employeeDto.Password,
                        UpdateAt = DateTime.Now,
                    };
                    _accountsService.AddAccount(addAccount);
                    //employeeInformation = new EmployeeInformation
                    //{
                    //    Id = employee.Id,
                    //    Name = employeeDto.Name,
                    //    BirthDay = employee.BirthDay,
                    //    Sex = employee.Sex,
                    //    Email = employee.Email,
                    //    PhoneNumber = employee.PhoneNumber,
                    //    Password = employeeDto.Password,
                    //    Repairer = employee.Repairer,
                    //    CreateAt = employee.CreateAt,
                    //    UpdateAt = employee.UpdateAt,
                    //    Creator = employee.Creator,
                    //    DeleteAt = employee.DeleteAt,
                    //    Accounts = addAccount,
                    //}
                }
            }
            return employeeInformation;
        }
    }
}
