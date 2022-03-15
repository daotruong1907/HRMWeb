using WebHRM.DTO;
using WebHRM.Interface;
using WebHRM.Models;

namespace WebHRM.Service
{
    public class LoginService : ILoginService
    {
        private readonly HRMContext _hRMWebContext;
        private readonly IAccountsService _accountService;
        public LoginService(HRMContext hRMContext, IAccountsService accountService)
        {
            _hRMWebContext = hRMContext;
            _accountService = accountService;
        }
        public bool Login(LoginDto loginDto)
        {
            if(loginDto != null)
            {
                var checkUser = _hRMWebContext.EmployeeInformation.Where(x => x.PhoneNumber == loginDto.UserName || x.Email == loginDto.UserName).FirstOrDefault();
                if(checkUser != null)
                {
                    var checkPass = _hRMWebContext.Accounts.Where(x => x.Id == checkUser.Id).FirstOrDefault();
                    if(checkPass != null)
                    {
                        var hashPassword = _accountService.CreateMD5(loginDto.Password);
                        return hashPassword == checkPass.PassWord? true: false;
                    }
                }
            }
            return false;
        }
    }
}
