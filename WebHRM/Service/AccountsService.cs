using WebHRM.DTO;
using WebHRM.Interface;
using WebHRM.Models;

namespace WebHRM.Service
{
    public class AccountsService : IAccountsService
    {
        private readonly HRMContext _hRMWebContext;
        public AccountsService(HRMContext hRMContext)
        {
            _hRMWebContext = hRMContext;
        }
        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +

                // Convert the byte array to hexadecimal string prior to .NET 5
                // StringBuilder sb = new System.Text.StringBuilder();
                // for (int i = 0; i < hashBytes.Length; i++)
                // {
                //     sb.Append(hashBytes[i].ToString("X2"));
                // }
                // return sb.ToString();
            }
        }
        public Accounts AddAccount(AddAccountDto account)
        {
            var newAccount = new Accounts();
            if (account != null)
            {
                var hashPassword = CreateMD5(account.PassWord);
                newAccount = new Accounts()
                {
                    Id = account.Id,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    Creator = account.Creator,
                    PassWord = hashPassword,
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
                if (oldaccount != null)
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
            var listAccount = _hRMWebContext.Accounts.Where(x => x.DeleteAt == null).OrderByDescending(x => x.UpdateAt).ToList();
            return listAccount;
        }
        public ResponsePageAccountDto GetAccounts(PageDto pageDto)
        {
            if (pageDto != null)
            {
                var ItemQuantity = (pageDto.PageQuantity - 1) * pageDto.ItemQuantityInPage;
                var listAccount = _hRMWebContext.Accounts.Where(x => x.DeleteAt == null).OrderByDescending(x => x.UpdateAt).Skip(ItemQuantity).Take(pageDto.ItemQuantityInPage).ToList();
                var allAccount = new AllAccountDto
                {
                    ItemQuantityInPage = pageDto.ItemQuantityInPage,
                    PageQuantity = pageDto.PageQuantity,
                    TotalItem = _hRMWebContext.Accounts.Count(),
                    TotalPage = Math.Ceiling((decimal)_hRMWebContext.Accounts.Count() / pageDto.ItemQuantityInPage)
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
