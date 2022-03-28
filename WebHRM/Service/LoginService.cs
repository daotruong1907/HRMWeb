using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebHRM.DTO;
using WebHRM.Interface;
using WebHRM.Models;
using WebHRM.Constant;

namespace WebHRM.Service
{
    public class LoginService : ILoginService
    {
        private readonly HRMContext _hRMWebContext;
        private readonly IAccountsService _accountService;
        private readonly IConfiguration _config;
        public LoginService(HRMContext hRMContext, IAccountsService accountService, IConfiguration config)
        {
            _hRMWebContext = hRMContext;
            _accountService = accountService;
            _config = config;
        }
        /// <summary>Logins the specified login dto.</summary>
        /// <param name="loginDto">The login dto.</param>
        /// <returns>
        ///   <br />
        ///   trả về id, username, và thông báo từ server để cho fontend xử dụng
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 24/03/2022 created
        /// </Modified>
        //public ResponseLogin Login(LoginDto loginDto)
        //{
        //    var responseLogin = new ResponseLogin();
        //    if(loginDto != null)
        //    {
        //        var checkUser = _hRMWebContext.EmployeeInformation.Where(x => (x.PhoneNumber == loginDto.UserName || x.Email == loginDto.UserName) && x.IsDeleted == false).FirstOrDefault();
        //        if(checkUser != null)
        //        {
        //            var checkPass = _hRMWebContext.Accounts.Where(x => x.Id == checkUser.Id && x.IsDeleted == false).FirstOrDefault();
        //            if(checkPass != null)
        //            {
        //                if(checkPass.LoginFailCount <= LoginConstants.LOGIN_LIMIT)
        //                {
        //                    var hashPassword = _accountService.CreateMD5(loginDto.Password);
        //                    if (hashPassword == checkPass.PassWord)
        //                    {
        //                        checkPass.LoginFailCount = 0;
        //                        _hRMWebContext.SaveChanges();
        //                        //GenerateJSONWebToken(loginDto);
        //                        responseLogin.IsSuccess = true;
        //                        responseLogin.Id = checkUser.Id;
        //                        responseLogin.ResponseFromServer = "Đang nhập thành công";
        //                    }
        //                    else
        //                    {
        //                        checkPass.LoginFailCount = checkPass.LoginFailCount + 1;
        //                        _hRMWebContext.SaveChanges();
        //                        responseLogin.IsSuccess = false;
        //                        if(checkPass.LoginFailCount>=3)
        //                        {
        //                            responseLogin.ResponseFromServer = "Mật khẩu không đúng, bạn đã nhập sai " +checkPass.LoginFailCount+" lần, quá 5 lần tài khoản sẽ bị khóa";
        //                        }
        //                        else
        //                        {
        //                            responseLogin.ResponseFromServer = "Mật khẩu không đúng";
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    responseLogin.ResponseFromServer = "Tài khoản đã bị khóa do đăng nhập sai quá nhiều, vui lòng liên hệ phòng IT để được hỗ trợ";
        //                    responseLogin.IsSuccess = false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            responseLogin.ResponseFromServer = "Tài khoản hoặc mật khẩu không hợp lệ";
        //            responseLogin.IsSuccess = false;
        //        }

        //        //Thêm thông báo tài khoản không tồn tại
        //    }
        //    return responseLogin;
        //}


        public ResponseLoginClone Login(LoginDto loginDto)
        {
            var responseLogin = new ResponseLoginClone();
            var data = new Data();
            if (loginDto != null)
            {
                var checkUser = _hRMWebContext.EmployeeInformation.Where(x => (x.PhoneNumber == loginDto.UserName || x.Email == loginDto.UserName) && x.IsDeleted == false).FirstOrDefault();
                if (checkUser != null)
                {
                    var checkPass = _hRMWebContext.Accounts.Where(x => x.Id == checkUser.Id && x.IsDeleted == false).FirstOrDefault();
                    if (checkPass != null)
                    {
                        if (checkPass.LoginFailCount <= LoginConstants.LOGIN_LIMIT)
                        {
                            var hashPassword = _accountService.CreateMD5(loginDto.Password);
                            if (hashPassword == checkPass.PassWord)
                            {
                                checkPass.LoginFailCount = 0;
                                _hRMWebContext.SaveChanges();
                                //GenerateJSONWebToken(loginDto);
                                responseLogin.ResponseCode = 0;
                                responseLogin.Id = checkUser.Id;
                                responseLogin.ResponseFromServer = "Đang nhập thành công";
                                responseLogin.Status = 1;
                                responseLogin.UserMessage = "";
                                data.ResponseFromServerr = "OK";
                                data.Id = checkUser.Id;
                                data.IsSuccess = true;
                                data.Status = 1;
                                responseLogin.Data = data;
                            }
                            else
                            {
                                checkPass.LoginFailCount = checkPass.LoginFailCount + 1;
                                _hRMWebContext.SaveChanges();
                                responseLogin.ResponseCode = 1;
                                responseLogin.Status = 0;
                                data.ResponseFromServerr = "Fail";
                                data.Id = checkUser.Id;
                                data.IsSuccess = true;
                                data.Status = 0;
                                responseLogin.Data = data;
                                if (checkPass.LoginFailCount >= 3)
                                {
                                    responseLogin.ResponseFromServer = "Mật khẩu không đúng, bạn đã nhập sai " + checkPass.LoginFailCount + " lần, quá 5 lần tài khoản sẽ bị khóa";
                                }
                                else
                                {
                                    responseLogin.ResponseFromServer = "Mật khẩu không đúng";
                                }
                            }
                        }
                        else
                        {
                            responseLogin.ResponseFromServer = "Tài khoản đã bị khóa do đăng nhập sai quá nhiều, vui lòng liên hệ phòng IT để được hỗ trợ";
                            responseLogin.ResponseCode = 1;
                            responseLogin.Status = 0;
                            data.ResponseFromServerr = "Fail";
                            data.Id = checkUser.Id;
                            data.IsSuccess = true;
                            data.Status = 0;

                            responseLogin.Data = data;
                        }
                    }
                }
                else
                {
                    responseLogin.ResponseFromServer = "Tài khoản hoặc mật khẩu không hợp lệ";
                    responseLogin.ResponseCode = 1;
                    responseLogin.Status = 0;
                    data.ResponseFromServerr = "Fail";
                    data.Id = checkUser.Id;
                    data.IsSuccess = true;
                    data.Status = 0;

                    responseLogin.Data = data;
                }

                //Thêm thông báo tài khoản không tồn tại
            }
            return responseLogin;
        }

        /// <summary>Generates the json web token.</summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns>
        ///   <br />
        ///   /Trả về token để đăng nhập
        ///   chưa sử dụng đến, sẽ dùng ở phần sau
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 24/03/2022 created
        /// </Modified>
        private string GenerateJSONWebToken(LoginDto userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
    };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
