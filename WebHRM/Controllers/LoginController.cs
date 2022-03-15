using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHRM.DTO;
using WebHRM.Interface;

namespace WebHRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService  = loginService;
        }

        [HttpPost("Login")]
        public bool Login(LoginDto loginDto)
        {
            var result = _loginService.Login(loginDto);
            return result;
        }
    }
}
