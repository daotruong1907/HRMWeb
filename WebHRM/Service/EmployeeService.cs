using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebHRM.Constant;
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
        public bool IsNumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }
        public bool isPhoneNumber(string phoneNumber)
        {
            if(!string.IsNullOrEmpty(phoneNumber))
            {
                var trimPhoneNumber = phoneNumber.Trim();
                if(trimPhoneNumber.Length == 11 || trimPhoneNumber.Length == 10)
                {
                    var splitPhoneNumber = trimPhoneNumber.Split(' ');
                    if(splitPhoneNumber.Length > 1)
                    {
                        return false;
                    }
                    else
                    {
                        if(IsNumber(splitPhoneNumber[0]))
                        {
                            if (splitPhoneNumber[0].Length ==11)
                            {
                                var strAreaCode = "84";
                                if ( splitPhoneNumber[0].Substring(0, 2).Equals(strAreaCode))
                                {
                                   // var ignoreAreaCode = splitPhoneNumber[0].Substring(2);
                                   // ignoreAreaCode = "0" + ignoreAreaCode;
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                var strAreaCode = "0";
                                if (splitPhoneNumber[0].Substring(0, 1).Equals(strAreaCode))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        { 
                            return false;
                        }
                    }
                }
                return false;
            }
            return false;
        }
        public string ChangeAreaCode(string phoneNumber)
        {
            if(!string.IsNullOrEmpty(phoneNumber))
            {
                var strAreaCode = "84";
                if (phoneNumber.Substring(0, 2).Equals(strAreaCode))
                {
                    var ignoreAreaCode = phoneNumber.Substring(2);
                    ignoreAreaCode = "0" + ignoreAreaCode;
                    return ignoreAreaCode;
                }
            }
            return phoneNumber;
        }
        public bool CheckAge(DateTime birthday)
        {
            var now = DateTime.Now;
            TimeSpan age = now - birthday;
            if (age.Days > 6570)
            {
                return true;
            }
            return false;
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
            if (employeeDto.Name != null && employeeDto.BirthDay != null && employeeDto.PhoneNumber != null && employeeDto.Password != null)
            {
                var numberPhone = employeeDto.PhoneNumber;
                if (isPhoneNumber(employeeDto.PhoneNumber))
                {
                    numberPhone = ChangeAreaCode(numberPhone);
                }
                if (employeeDto.Email != null)
                {
                    var checkUnique = _hRMWebContext.EmployeeInformation.Where(x => x.Email == employeeDto.Email || x.PhoneNumber == numberPhone).FirstOrDefault();
                    if (checkUnique != null)
                    {
                        employeeInformationDto.ResponseFromServer = "Email hoặc số điện thoại này đã được " + checkUnique.Name + " sử dụng";
                        return employeeInformationDto;
                    }
                }
                if(!CheckAge(employeeDto.BirthDay))
                {
                        employeeInformationDto.ResponseFromServer = "Chưa đủ 18 tuổi";
                        return employeeInformationDto;
                }
                var sex = "";
                if (employeeDto.Sex != null)
                {
                    if(sex != SexType.MALE || sex != SexType.FEMALE)
                    {
                        employeeInformationDto.ResponseFromServer = "Không có giới tính này";
                        return employeeInformationDto;
                    }
                    sex = employeeDto.Sex.ToLower();
                }
                var email = "";
                if(email != null)
                {
                    if(!ValidateEmail(email))
                    {
                        employeeInformationDto.ResponseFromServer = "Email không đúng định dạng";
                        return employeeInformationDto;
                    }
                    email = employeeDto.Email.ToLower();
                }    
                var newEmployee = new EmployeeInformation
                {
                    Name = employeeDto.Name,
                    BirthDay = employeeDto.BirthDay,
                    Sex = sex,
                    PhoneNumber = numberPhone,
                    Email = email,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    Creator = employeeDto.Creator,
                };
                _hRMWebContext.EmployeeInformation.Add(newEmployee);
                _hRMWebContext.SaveChanges();
                var employee = _hRMWebContext.EmployeeInformation.Where(x => x.PhoneNumber == numberPhone && x.DeleteAt == null).FirstOrDefault();
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
            if (updateEmployeeDto.Name != null && updateEmployeeDto.BirthDay != null && updateEmployeeDto.PhoneNumber != null)
            {
                var numberPhone = updateEmployeeDto.PhoneNumber;
                if (isPhoneNumber(updateEmployeeDto.PhoneNumber))
                {
                    numberPhone = ChangeAreaCode(numberPhone);
                }
                if (updateEmployeeDto.Email != null)
                {
                    var checkUnique = _hRMWebContext.EmployeeInformation.Where(x => x.Email == updateEmployeeDto.Email || x.PhoneNumber == numberPhone).FirstOrDefault();
                    if (checkUnique != null)
                    {
                        return "Email hoặc số điện thoại này đã được " + checkUnique.Name + " sử dụng";
                    }
                }
                if (!CheckAge(updateEmployeeDto.BirthDay))
                {
                    return "Chưa đủ 18 tuổi";
                }
                var email = updateEmployeeDto.Email.ToLower();
                if (email != null)
                {
                    if (!ValidateEmail(email))
                    {
                        return "Email không đúng định dạng";
                    }
                }
                var sex = updateEmployeeDto.Sex.ToLower();
                if (updateEmployeeDto.Sex != null)
                {
                    if (sex != SexType.MALE || sex != SexType.FEMALE)
                    {
                        return "Không có giới tính này";
                    }
                }
                var oldEmployee = _hRMWebContext.EmployeeInformation.Where(x => x.Id == updateEmployeeDto.Id && x.DeleteAt == null).FirstOrDefault();
                if (oldEmployee != null)
                {
                    oldEmployee.Name = updateEmployeeDto.Name;
                    oldEmployee.Email = updateEmployeeDto.Email;
                    oldEmployee.PhoneNumber = numberPhone;
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

        public ListResponseSearchEmployee SearchEmployee(RequestSearchEmployee requestSearchEmployee)
        {
            ListResponseSearchEmployee responseSearchEmployees = new ListResponseSearchEmployee();
            var listEmployee = new List<ResponseSearchEmployee>();
            if(requestSearchEmployee.ParamSearchEmployee != null)
            {
                if (requestSearchEmployee.ParamSearchEmployee.FromBirthDay > requestSearchEmployee.ParamSearchEmployee.ToBirthDay)
                {
                    responseSearchEmployees.ResponseFromServer = "Từ ngày lớn hơn đến ngày";
                    return responseSearchEmployees;
                }
                var responseEmployee = _hRMWebContext.EmployeeInformation.Where(x=> x.DeleteAt == null).Select(x=> new ResponseSearchEmployee
                {
                    Name = x.Name,
                    Email = x.Email,
                    BirthDay = x.BirthDay,
                    Id = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    Sex = x.Sex,
                });
                if(!string.IsNullOrEmpty(requestSearchEmployee.ParamSearchEmployee.Sex) && !requestSearchEmployee.ParamSearchEmployee.Sex.Equals("ALL"))
                {
                    responseEmployee =  responseEmployee.Where(y=> y.Sex == requestSearchEmployee.ParamSearchEmployee.Sex);
                }
                if (!string.IsNullOrEmpty(requestSearchEmployee.ParamSearchEmployee.PhoneNumber))
                {
                    responseEmployee = responseEmployee.Where(x => x.PhoneNumber == requestSearchEmployee.ParamSearchEmployee.PhoneNumber);
                }
                if (requestSearchEmployee.ParamSearchEmployee.FromBirthDay != null || requestSearchEmployee.ParamSearchEmployee.ToBirthDay != null)
                {
                    if(requestSearchEmployee.ParamSearchEmployee.FromBirthDay != null && requestSearchEmployee.ParamSearchEmployee.ToBirthDay != null)
                    {
                        responseEmployee = responseEmployee.Where(x => x.BirthDay >= requestSearchEmployee.ParamSearchEmployee.FromBirthDay && x.BirthDay <= requestSearchEmployee.ParamSearchEmployee.ToBirthDay);
                    }
                    else if(requestSearchEmployee.ParamSearchEmployee.FromBirthDay != null && requestSearchEmployee.ParamSearchEmployee.ToBirthDay == null)
                    {
                        responseEmployee = responseEmployee.Where(x => x.BirthDay >= requestSearchEmployee.ParamSearchEmployee.FromBirthDay);
                    }
                    else
                    {
                        responseEmployee = responseEmployee.Where(x => x.BirthDay <= requestSearchEmployee.ParamSearchEmployee.ToBirthDay);

                    }
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
                responseSearchEmployees.ResponseSearchEmployees = responseEmployee.ToList(); 
            }
            return responseSearchEmployees;
        }
    }
}
