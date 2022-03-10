using WebHRM.DTO;
using WebHRM.Models;

namespace WebHRM.Interface
{
    public interface IAccountsService
    {
        Accounts AddAccount(AddAccountDto accounts);
        Accounts UpdateAccount(UpdateAccountDto accounts);
        Accounts DeleteAccount(int Id);
        List<Accounts> GetAllAccounts();
        ResponsePageAccountDto GetAccounts(PageDto pageDto);
    }
}
