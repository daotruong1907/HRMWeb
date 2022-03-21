using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebHRM.DTO;
using WebHRM.Interface;
using WebHRM.Models;

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
        public ResponseLogin Login(LoginDto loginDto)
        {
            var responseLogin = new ResponseLogin();
            if(loginDto != null)
            {
                var checkUser = _hRMWebContext.EmployeeInformation.Where(x => (x.PhoneNumber == loginDto.UserName || x.Email == loginDto.UserName) && x.DeleteAt == null ).FirstOrDefault();
                if(checkUser != null)
                {
                    var checkPass = _hRMWebContext.Accounts.Where(x => x.Id == checkUser.Id).FirstOrDefault();
                    if(checkPass != null)
                    {
                        var hashPassword = _accountService.CreateMD5(loginDto.Password);
                        if(hashPassword == checkPass.PassWord)
                        {
                            //GenerateJSONWebToken(loginDto);
                           responseLogin.IsSuccess =  true;
                           responseLogin.Name = checkUser.Name;
                        }
                        else
                        {
                            responseLogin.IsSuccess = false;
                        }
                    }
                }
                //Thêm thông báo tài khoản không tồn tại
            }
            return responseLogin;
        }

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
