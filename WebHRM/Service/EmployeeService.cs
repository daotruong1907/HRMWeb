using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebHRM.Constant;
using WebHRM.DTO;
using WebHRM.Interface;
using WebHRM.Models;

namespace WebHRM.Service
{
    /// <summary>
    ///   <br />
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// truongdv 22/03/2022 created
    /// </Modified>
    public class EmployeeService : IEmployeeService
    {
        private readonly HRMContext _hRMWebContext;
        private readonly IAccountsService _accountsService;
        /// <summary>Initializes a new instance of the <see cref="EmployeeService" /> class.</summary>
        /// <param name="hRMContext">The h rm context.</param>
        /// <param name="accountsService">The accounts service.</param>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 24/03/2022 created
        /// </Modified>
        public EmployeeService(HRMContext hRMContext, IAccountsService accountsService)
        {
            _hRMWebContext = hRMContext;
            _accountsService = accountsService;
        }
        /// <summary>Determines whether the specified p value is number.</summary>
        /// <param name="pValue">The p value.</param>
        /// <returns>
        ///   <c>true</c> if the specified p value is number; otherwise, <c>false</c>.</returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 24/03/2022 created
        /// </Modified>
        public bool IsNumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }
        /// <summary>Determines whether [is phone number] [the specified phone number].</summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns>
        ///   <c>true</c> if [is phone number] [the specified phone number]; otherwise, <c>false</c>.</returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 24/03/2022 created
        /// </Modified>
        public bool IsPhoneNumber(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                string trimPhoneNumber = phoneNumber.Trim();
                if (trimPhoneNumber.Length == 11 || trimPhoneNumber.Length == 10)
                {
                    var splitPhoneNumber = trimPhoneNumber.Split(' ');
                    if (splitPhoneNumber.Length > 1)
                    {
                        return false;
                    }
                    else
                    {
                        if (IsNumber(splitPhoneNumber[0]))
                        {
                            if (splitPhoneNumber[0].Length == 11)
                            {
                                string strAreaCode = "84";
                                if (splitPhoneNumber[0].Substring(0, 2).Equals(strAreaCode))
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                string strAreaCode = "0";
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
        /// <summary>Changes the area code.</summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 24/03/2022 created
        /// </Modified>
        public string ChangeAreaCode(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                string strAreaCode = "84";
                if (phoneNumber.Substring(0, 2).Equals(strAreaCode))
                {
                    string ignoreAreaCode = phoneNumber.Substring(2);
                    ignoreAreaCode = "0" + ignoreAreaCode;
                    return ignoreAreaCode;
                }
            }
            return phoneNumber;
        }
        /// <summary>Checks the age.</summary>
        /// <param name="birthday">The birthday.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 24/03/2022 created
        /// </Modified>
        public bool CheckAge(DateTime birthday)
        {
            //fixme: dùng hàm subtract để trừ ngày
            var now = DateTime.Now;
            var age = now.Subtract(birthday);
            if (age.Days > 6570)
            {
                return true;
            }
            return false;
        }
        public int Age(DateTime birthday)
        {
            int age;
            var now = DateTime.Now;
            age = now.Year - birthday.Year;

            if (age > 0)
            {
                age -= Convert.ToInt32(now.Date < birthday.Date.AddYears(age));
            }
            else
            {
                age = 0;
            }

            return age;
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
            StringBuilder responseFromServer = new StringBuilder();
            if (employeeDto.Name != null && employeeDto.PhoneNumber != null && employeeDto.Password != null)
            {
                string numberPhone = employeeDto.PhoneNumber;
                if (IsPhoneNumber(employeeDto.PhoneNumber))
                {
                    numberPhone = ChangeAreaCode(numberPhone);
                }
                else
                {
                    responseFromServer.Append("Số điện thoại không đúng định dạng");
                    employeeInformationDto.isSuccess = false;
                }
                string email = "";
                if (!string.IsNullOrEmpty(employeeDto.Email))
                {
                    if (!ValidateEmail(employeeDto.Email))
                    {
                        //  employeeInformationDto.ResponseFromServer = "Email không đúng định dạng";
                        responseFromServer.Append("Email không đúng định dạng");
                    }
                    else
                    {
                        email = employeeDto.Email.ToLower();
                    }
                }
                if (string.IsNullOrEmpty(email))
                {
                    var checkUnique = _hRMWebContext.EmployeeInformation.Where(x => x.PhoneNumber == numberPhone && x.IsDeleted == false ).FirstOrDefault();
                    if (checkUnique != null)
                    {
                        //employeeInformationDto.ResponseFromServer = "Email hoặc số điện thoại này đã được " + checkUnique.Name + " sử dụng";
                        responseFromServer.Append("Số điện thoại này đã được " + checkUnique.Name + " sử dụng");
                        employeeInformationDto.isSuccess = false;
                    }
                }
                else
                {
                    var checkUnique = _hRMWebContext.EmployeeInformation.Where(x => x.Email == email || x.PhoneNumber == numberPhone && x.IsDeleted == false).FirstOrDefault();
                    if (checkUnique != null)
                    {
                        responseFromServer.Append("Email hoặc số điện thoại này đã được " + checkUnique.Name + " sử dụng");
                        employeeInformationDto.isSuccess = false;
                    }
                }
                if (!CheckAge(employeeDto.BirthDay))
                {
                    responseFromServer.Append("Chưa đủ 18 tuổi");
                    employeeInformationDto.isSuccess = false;
                }
                int? sex = 3;
                if (employeeDto.Sex != null)
                {
                    if (employeeDto.Sex == SexType.FEMALE || employeeDto.Sex == SexType.MALE || employeeDto.Sex == SexType.LGBT)
                    {
                        sex = employeeDto.Sex;
                    }
                    else
                    {
                        responseFromServer.Append("Không có giới tính này");
                    }
                }
                if (employeeInformationDto.isSuccess)
                {
                    var newEmployee = new EmployeeInformation
                    {
                        Name = employeeDto.Name,
                        BirthDay = employeeDto.BirthDay,
                        Sex = sex,
                        PhoneNumber = numberPhone,
                        Email = email,
                        CreateAt = DateTime.Now,
                        UpdateAt = DateTime.Now,
                        CreatorId = employeeDto.CreatorId,
                    };
                    _hRMWebContext.EmployeeInformation.Add(newEmployee);
                    _hRMWebContext.SaveChanges();
                    var employee = _hRMWebContext.EmployeeInformation.Where(x => x.PhoneNumber == numberPhone && x.IsDeleted == false).FirstOrDefault();
                    if (employee != null)
                    {
                        var addAccount = new AddAccountDto
                        {
                            CreateAt = DateTime.Now,
                            CreatorId = employeeDto.CreatorId,
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
            }
            employeeInformationDto.ResponseFromServer = responseFromServer.ToString();
            return employeeInformationDto;
        }

        /// <summary>Updates the employee.</summary>
        /// <param name="updateEmployeeDto">The update employee dto.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 24/03/2022 created
        /// </Modified>
        public ResponseUpdateEmployee UpdateEmployee(UpdateEmployeeDto updateEmployeeDto)
        {
            var responseUpdateEmployee = new ResponseUpdateEmployee();
            StringBuilder responseFromServer = new StringBuilder();
            if (updateEmployeeDto.Name != null && updateEmployeeDto.PhoneNumber != null)
            {
                var oldEmployee = _hRMWebContext.EmployeeInformation.Where(x => x.Id == updateEmployeeDto.Id && x.IsDeleted == false).FirstOrDefault();
                if (oldEmployee != null)
                {
                    oldEmployee.Name = updateEmployeeDto.Name;
                    string numberPhone = updateEmployeeDto.PhoneNumber;
                    if (!oldEmployee.PhoneNumber.Equals(numberPhone))
                    {
                        if (IsPhoneNumber(numberPhone))
                        {
                            numberPhone = ChangeAreaCode(numberPhone);
                            var checkUnique = _hRMWebContext.EmployeeInformation.Where(x => x.PhoneNumber == numberPhone && x.IsDeleted == false).FirstOrDefault();
                            if (checkUnique != null)
                            {
                                responseFromServer.Append("Số điện thoại này đã được " + checkUnique.Name + " sử dụng");
                                responseUpdateEmployee.isSuccess = false;
                            }
                            else
                            {
                                oldEmployee.PhoneNumber = numberPhone;
                            }
                        }
                        else
                        {
                            responseFromServer.Append("Số điện thoại không đúng định dạng");
                            responseUpdateEmployee.isSuccess = false;
                        }
                    }
                    if(!oldEmployee.Email.Equals(updateEmployeeDto.Email))
                    {
                        if (!string.IsNullOrEmpty(updateEmployeeDto.Email))
                        {
                            if (!ValidateEmail(updateEmployeeDto.Email))
                            {
                                responseFromServer.Append("Email không đúng định dạng");
                            }
                            else
                            {
                                oldEmployee.Email = updateEmployeeDto.Email.ToLower();
                            }
                        }
                        else
                        {
                            oldEmployee.Email = updateEmployeeDto.Email.ToLower();
                        }
                    }
                    if (oldEmployee.BirthDay != updateEmployeeDto.BirthDay)
                    {
                        if (!CheckAge(updateEmployeeDto.BirthDay))
                        {
                            responseFromServer.Append("Chưa đủ 18 tuổi");
                            responseUpdateEmployee.isSuccess = false;
                        }
                        else
                        {
                            oldEmployee.BirthDay = updateEmployeeDto.BirthDay;
                        }
                    }
                    if (updateEmployeeDto.Sex != null)
                    {
                        if (updateEmployeeDto.Sex == SexType.FEMALE || updateEmployeeDto.Sex == SexType.MALE || updateEmployeeDto.Sex == SexType.LGBT)
                        {
                            oldEmployee.Sex = updateEmployeeDto.Sex;
                        }
                        else
                        {
                            responseFromServer.Append("Không có giới tính này");
                        }
                    }
                    oldEmployee.RepairerId = updateEmployeeDto.RepairerId;
                    oldEmployee.IsUpdated = true;
                    oldEmployee.UpdateAt = DateTime.Now;
                    if(responseUpdateEmployee.isSuccess == false)
                    {
                        responseUpdateEmployee.UpdateEmployeeDto = updateEmployeeDto;
                        responseUpdateEmployee.ResponseFromServer = responseFromServer.ToString();
                        return responseUpdateEmployee;
                    }    
                    _hRMWebContext.SaveChanges();
                    updateEmployeeDto.UpdateAt = DateTime.Now;
                    updateEmployeeDto.Id = oldEmployee.Id;
                }   
            }
            responseUpdateEmployee.UpdateEmployeeDto = updateEmployeeDto;
            responseUpdateEmployee.ResponseFromServer = responseFromServer.ToString();
            return responseUpdateEmployee;
        }

        /// <summary>Deletes the employee.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 23/03/2022 created
        /// </Modified>
        public bool DeleteEmployee(int id , int eraserId)
        {
            var oldEmployee = _hRMWebContext.EmployeeInformation.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
            if (oldEmployee != null)
            {
                oldEmployee.DeleteAt = DateTime.Now;
                oldEmployee.IsDeleted = true;
                oldEmployee.EraserId = eraserId;
                _accountsService.DeleteAccount(oldEmployee.Id, eraserId);
                _hRMWebContext.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>Validates the email.</summary>
        /// <param name="email">The email.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 24/03/2022 created
        /// </Modified>
        private bool ValidateEmail(string email)
        {

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }

        /// <summary>Searchs the employee.</summary>
        /// <param name="requestSearchEmployee">The request search employee.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 24/03/2022 created
        /// </Modified>
        public ListResponseSearchEmployee SearchEmployee(RequestSearchEmployee requestSearchEmployee)
        {
            ListResponseSearchEmployee responseSearchEmployees = new ListResponseSearchEmployee();
            var listEmployee = new List<ResponseSearchEmployee>();
            if (requestSearchEmployee.ParamSearchEmployee != null)
            {
                if (requestSearchEmployee.ParamSearchEmployee.FromBirthDay > requestSearchEmployee.ParamSearchEmployee.ToBirthDay)
                {
                    responseSearchEmployees.ResponseFromServer = "Từ ngày lớn hơn đến ngày";
                    return responseSearchEmployees;
                }
                var responseEmployee = _hRMWebContext.EmployeeInformation.Where(x => x.IsDeleted == false).Select(x => new ResponseSearchEmployee
                {
                    Name = x.Name,
                    Email = x.Email,
                    BirthDay = x.BirthDay,
                    Id = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    Sex = x.Sex,
                });
                if (requestSearchEmployee.ParamSearchEmployee.Sex == SexType.FEMALE || requestSearchEmployee.ParamSearchEmployee.Sex == SexType.MALE || requestSearchEmployee.ParamSearchEmployee.Sex == SexType.LGBT)
                {
                    responseEmployee = responseEmployee.Where(y => y.Sex == requestSearchEmployee.ParamSearchEmployee.Sex);
                }
                if (!string.IsNullOrEmpty(requestSearchEmployee.ParamSearchEmployee.PhoneNumber))
                {
                    responseEmployee = responseEmployee.Where(x => x.PhoneNumber == requestSearchEmployee.ParamSearchEmployee.PhoneNumber);
                }
                if (requestSearchEmployee.ParamSearchEmployee.FromBirthDay != null || requestSearchEmployee.ParamSearchEmployee.ToBirthDay != null)
                {
                    if (requestSearchEmployee.ParamSearchEmployee.FromBirthDay != null && requestSearchEmployee.ParamSearchEmployee.ToBirthDay != null)
                    {
                        responseEmployee = responseEmployee.Where(x => x.BirthDay >= requestSearchEmployee.ParamSearchEmployee.FromBirthDay && x.BirthDay <= requestSearchEmployee.ParamSearchEmployee.ToBirthDay);
                    }
                    else if (requestSearchEmployee.ParamSearchEmployee.FromBirthDay != null && requestSearchEmployee.ParamSearchEmployee.ToBirthDay == null)
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
                    if (check)
                    {
                        responseEmployee = responseEmployee.Where(x => x.Email == requestSearchEmployee.ParamSearchEmployee.NameOrEmail);
                    }
                    else
                    {
                        responseEmployee = responseEmployee.Where(x => x.Name.Contains(requestSearchEmployee.ParamSearchEmployee.NameOrEmail));
                    }
                }
                var ItemQuantity = (requestSearchEmployee.PageDto.PageQuantity - 1) * requestSearchEmployee.PageDto.ItemQuantityInPage;
                responseEmployee = responseEmployee.Skip(ItemQuantity).Take(requestSearchEmployee.PageDto.ItemQuantityInPage);
                listEmployee = responseEmployee.ToList();
                responseSearchEmployees.ResponseSearchEmployees = responseEmployee.ToList();
            }
            return responseSearchEmployees;
        }

        /// <summary>Gets the employeee by identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 24/03/2022 created
        /// </Modified>
        public ResponseSearchEmployee GetEmployeeeById(int id)
        {
            var response = new ResponseSearchEmployee();
            var employee = _hRMWebContext.EmployeeInformation.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
            if (employee != null)
            {
                response = new ResponseSearchEmployee()
                {
                    Id = id,
                    BirthDay = employee.BirthDay,
                    Email = employee.Email,
                    Name = employee.Name,
                    PhoneNumber = employee.PhoneNumber,
                    Sex = employee.Sex,
                };
            }
            return response;
        }

        //public ResponseSearchEmployee GetEmployeeeByIdUseStoreProcedure(int id)
        //{
        //    try
        //    {
        //        using (var context = new DbContext())
        //        {
        //            var clientIdParameter = new SqlParameter("@id", 4);

        //            var result = context.Database.SqlQuery<ResponseSearchEmployee>("getAllEmployee @id", id)
        //                .ToList();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        /// <summary>Gets the count employeee.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 24/03/2022 created
        /// </Modified>
        public int GetCountEmployeee()
        {
            var employee = _hRMWebContext.EmployeeInformation.Where(x => x.IsDeleted == false).Count();
            return employee;
        }
    }
}
