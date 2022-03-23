using WebHRM.DTO;
using WebHRM.Models;

namespace WebHRM.Interface
{
    public interface IAccountsService
    {
        string CreateMD5(string input);
        Accounts AddAccount(AddAccountDto accounts);
        Accounts UpdateAccount(UpdateAccountDto accounts);
        Accounts DeleteAccount(int Id, int eraserid);
        List<Accounts> GetAllAccounts();
        ResponsePageAccountDto GetAccounts(PageDto pageDto);
    }
}
