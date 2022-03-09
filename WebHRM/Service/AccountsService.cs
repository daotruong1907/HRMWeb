using WebHRM.DTO;
using WebHRM.Interface;
using WebHRM.Models;

namespace WebHRM.Service
{
    public class AccountsService:IAccountsService
    {
        private readonly HRMContext _hRMWebContext;
        public AccountsService(HRMContext hRMContext)
        {
            _hRMWebContext = hRMContext;
        }
        public Accounts AddAccount (AddAccountDto account)
        {
            var newAccount = new Accounts();
            if (account != null)
            {
                 newAccount = new Accounts()
                {
                    Id = account.Id,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    Creator = account.Creator,
                    PassWord = account.PassWord,
                };
                //var newEmployee = new EmployeeInformation
                //{
                //    CreateAt = DateTime.Now,
                //    UpdateAt= DateTime.Now,
                //    BirthDay = account.EmployeeInformation.BirthDay,
                //    Creator = account.Creator,
                //    PhoneNumber = account.EmployeeInformation.PhoneNumber,
                //    Email = account.EmployeeInformation.Email,  
                //    Name = account.EmployeeInformation.Name,    
                //    PassWord = account.EmployeeInformation?.PassWord,
                //    Sex = account.EmployeeInformation.Sex,
                //};
                _hRMWebContext.Add(newAccount);
                _hRMWebContext.SaveChanges();
                return newAccount;
            }
            return newAccount;
        }

        public Accounts UpdateAccount(UpdateAccountDto account)
        {
            var oldAccount = new Accounts();
            if (account != null)
            {
                var oldaccount = _hRMWebContext.Accounts.Where(x => x.Id == account.Id && x.DeleteAt != null).FirstOrDefault();
                if(oldaccount != null)
                {
                    oldaccount.UpdateAt = DateTime.Now;
                    oldaccount.Repairer = account.Repairer;
                    oldaccount.PassWord = account.PassWord;
                }
                _hRMWebContext.SaveChanges();
            }
            return oldAccount;
        }
        public Accounts DeleteAccount(int Id)
        {
                var oldaccount = _hRMWebContext.Accounts.Where(x => x.Id == Id && x.DeleteAt != null).FirstOrDefault();
                if (oldaccount != null)
                {
                    oldaccount.DeleteAt = DateTime.Now;
                }
            _hRMWebContext.SaveChanges();
            return oldaccount;
        }
        public List<Accounts> GetAllAccounts()
        {
            var listAccount = _hRMWebContext.Accounts.Where(x=> x.DeleteAt == null).OrderByDescending(x=>x.UpdateAt).ToList();
            return listAccount;
        }
        public ResponsePageAccountDto GetAccounts(AccountDto accountDto)
        {
            if(accountDto != null)
            {
                var ItemQuantity = (accountDto.PageQuantity - 1) * accountDto.ItemQuantityInPage;
                var listAccount = _hRMWebContext.Accounts.Where(x => x.DeleteAt == null).OrderByDescending(x=>x.UpdateAt).Skip(ItemQuantity).Take(accountDto.ItemQuantityInPage).ToList();
                var allAccount = new AllAccountDto
                {
                    ItemQuantityInPage = accountDto.ItemQuantityInPage,
                    PageQuantity = accountDto.PageQuantity,
                    TotalItem = _hRMWebContext.Accounts.Count(),
                    TotalPage = Math.Ceiling((decimal)_hRMWebContext.Accounts.Count() / accountDto.ItemQuantityInPage)
                };
                var respone = new ResponsePageAccountDto
                {
                    Accounts = listAccount,
                    AllAccount = allAccount,
                };
                return respone;
            }
            
            
            return null;
        }
    }
}
