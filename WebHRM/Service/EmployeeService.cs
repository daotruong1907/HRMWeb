using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        /// <summary>Adds the employee.</summary>
        /// <param name="employeeDto">The employee dto.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 10/03/2022 created
        /// </Modified>
        public EmployeeInformationDto AddEmployee(EmployeeDto employeeDto)
        {
            var employeeInformationDto = new EmployeeInformationDto();
            if (employeeDto != null)
            {
                var checkUnique = _hRMWebContext.EmployeeInformation.Where(x => x.Email == employeeDto.Email || x.PhoneNumber == employeeDto.PhoneNumber).FirstOrDefault();
                if (checkUnique != null)
                {
                    employeeInformationDto.ResponseFromServer = "Email hoặc số điện thoại này đã được " + checkUnique.Name + " sử dụng";
                    return employeeInformationDto;
                }
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
                    employeeInformationDto = new EmployeeInformationDto
                    {
                        Employee = employeeDto,
                        Account = addAccount,
                    };
                }
            }
            return employeeInformationDto;
        }

        public string UpdateEmployee(UpdateEmployeeDto updateEmployeeDto)
        {
            if (updateEmployeeDto != null)
            {
                var checkUnique = _hRMWebContext.EmployeeInformation.Where(x => x.Email == updateEmployeeDto.Email || x.PhoneNumber == updateEmployeeDto.PhoneNumber).FirstOrDefault();
                if (checkUnique != null)
                {
                    return "Email hoặc số điện thoại này đã được " + checkUnique.Name + " sử dụng";
                }
                var oldEmployee = _hRMWebContext.EmployeeInformation.Where(x => x.Id == updateEmployeeDto.Id && x.DeleteAt == null).FirstOrDefault();
                if (oldEmployee != null)
                {
                    oldEmployee.Name = updateEmployeeDto.Name;
                    oldEmployee.Email = updateEmployeeDto.Email;
                    oldEmployee.PhoneNumber = updateEmployeeDto.PhoneNumber;
                    oldEmployee.Sex = updateEmployeeDto.Sex;
                    oldEmployee.BirthDay = updateEmployeeDto.BirthDay;
                    oldEmployee.Repairer = updateEmployeeDto.Repairer;
                    oldEmployee.UpdateAt = DateTime.Now;
                }
                _hRMWebContext.SaveChanges();
                return "Sửa thành công";
            }
            return "Sửa thất bại";
        }

        public bool DeleteEmployee(int id)
        {
            var oldEmployee = _hRMWebContext.EmployeeInformation.Where(x => x.Id == id && x.DeleteAt == null).FirstOrDefault();
            if (oldEmployee != null)
            {
                oldEmployee.DeleteAt = DateTime.Now;
                var account = _hRMWebContext.Accounts.Where(x => x.Id == id && x.DeleteAt == null).FirstOrDefault();
                if (account != null)
                {
                    account.DeleteAt = DateTime.Now;
                }
                _hRMWebContext.SaveChanges();
                return true;
            }
            return false;
        }

        private bool ValidateEmail(string email)
        {

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }

        public List<ResponseSearchEmployee> SearchEmployee(RequestSearchEmployee requestSearchEmployee)
        {
            var listEmployee = new List<ResponseSearchEmployee>();
            if(requestSearchEmployee.ParamSearchEmployee != null)
            {
                var responseEmployee = _hRMWebContext.EmployeeInformation.Select(x=> new ResponseSearchEmployee
                {
                    Name = x.Name,
                    Email = x.Email,
                    BirthDay = x.BirthDay,
                    Id = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    Sex = x.Sex,
                });
                if(!string.IsNullOrEmpty(requestSearchEmployee.ParamSearchEmployee.Sex))
                {
                    responseEmployee =  responseEmployee.Where(y=> y.Sex == requestSearchEmployee.ParamSearchEmployee.Sex);
                }
                if (!string.IsNullOrEmpty(requestSearchEmployee.ParamSearchEmployee.PhoneNumber))
                {
                    responseEmployee = responseEmployee.Where(x => x.PhoneNumber == requestSearchEmployee.ParamSearchEmployee.PhoneNumber);
                }
                if (requestSearchEmployee.ParamSearchEmployee.FromBirthDay != null && requestSearchEmployee.ParamSearchEmployee.ToBirthDay != null)
                {
                    responseEmployee = responseEmployee.Where(x => x.BirthDay >= requestSearchEmployee.ParamSearchEmployee.FromBirthDay && x.BirthDay <= requestSearchEmployee.ParamSearchEmployee.ToBirthDay);
                }
                if (!string.IsNullOrEmpty(requestSearchEmployee.ParamSearchEmployee.NameOrEmail))
                {
                    var check = ValidateEmail(requestSearchEmployee.ParamSearchEmployee.NameOrEmail);
                    if(check)
                    {
                        responseEmployee = responseEmployee.Where(x => x.Email == requestSearchEmployee.ParamSearchEmployee.NameOrEmail);
                    }
                    else
                    {
                        responseEmployee = responseEmployee.Where(x => x.Name == requestSearchEmployee.ParamSearchEmployee.NameOrEmail);
                    }
                }
                var ItemQuantity = (requestSearchEmployee.PageDto.PageQuantity - 1) * requestSearchEmployee.PageDto.ItemQuantityInPage;
                responseEmployee = responseEmployee.Skip(ItemQuantity).Take(requestSearchEmployee.PageDto.ItemQuantityInPage);
                listEmployee = responseEmployee.ToList();
            }
            return listEmployee;
        }
    }
}
