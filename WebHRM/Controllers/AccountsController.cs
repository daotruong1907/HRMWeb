using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHRM.DTO;
using WebHRM.Interface;
using WebHRM.Models;

namespace WebHRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;
        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        [HttpPost("DeleteAccount")]
        public Accounts DeleteAccount(int id)
        {
            var result = _accountsService.DeleteAccount(id);
            return result;
        }

        [HttpPost("AddAccount")]
        public Accounts AddAccount(AddAccountDto accounts)
        {
            var result = _accountsService.AddAccount(accounts);
            return result;
        }

        [HttpPost("UpdateAccount")]
        public Accounts UpdateAccount(UpdateAccountDto accounts)
        {
            var result = _accountsService.UpdateAccount(accounts);
            return result;
        }

        [HttpGet("GetAllAccounts")]
        public List<Accounts> GetAllAccounts()
        {
            var result = _accountsService.GetAllAccounts();
            return result;
        }

        [HttpGet("GetAccounts")]
        public ResponsePageAccountDto GetAccounts(PageDto pageDto)
        {
            var result = _accountsService.GetAccounts(pageDto);
            return result;
        }

    }
}
