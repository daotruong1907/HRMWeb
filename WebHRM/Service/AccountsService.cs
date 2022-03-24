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
        /// <summary>Creates the m d5.</summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <br />
        ///   mã hóa mật khẩu theo md5
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 16/03/2022 created
        /// Mã hóa MD5
        /// </Modified>
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
        /// <summary>Adds the account.</summary>
        /// <param name="account">The account.</param>
        /// <returns>
        ///   <br />
        ///   thêm account
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 16/03/2022 created
        /// </Modified>
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
                    CreatorId = account.CreatorId,
                    PassWord = hashPassword,
                };
                _hRMWebContext.Add(newAccount);
                _hRMWebContext.SaveChanges();
                return newAccount;
            }
            return newAccount;
        }

        /// <summary>Updates the account.</summary>
        /// <param name="account">The account.</param>
        /// <returns>
        ///   <br />
        ///   sửa account
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 16/03/2022 created
        /// </Modified>
        public Accounts UpdateAccount(UpdateAccountDto account)
        {
            var oldAccount = new Accounts();
            if (account != null)
            {
                var oldaccount = _hRMWebContext.Accounts.Where(x => x.Id == account.Id && x.IsDeleted == false).FirstOrDefault();
                if (oldaccount != null)
                {
                    oldaccount.UpdateAt = DateTime.Now;
                    oldaccount.RepairerId = account.RepairerId;
                    oldaccount.PassWord = account.PassWord;
                }
                _hRMWebContext.SaveChanges();
            }
            return oldAccount;
        }
        /// <summary>Deletes the account.</summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>
        ///   <br />
        ///   xóa một account
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 16/03/2022 created
        /// </Modified>
        public Accounts DeleteAccount(int Id, int eraserId)
        {
            var oldaccount = _hRMWebContext.Accounts.Where(x => x.Id == Id && x.IsDeleted == false).FirstOrDefault();
            if (oldaccount != null)
            {
                oldaccount.DeleteAt = DateTime.Now;
                oldaccount.IsDeleted = true;
                oldaccount.EraserId = eraserId;
            }
            _hRMWebContext.SaveChanges();
            return oldaccount;
        }
        /// <summary>Gets all accounts.</summary>
        /// <returns>
        ///   <br />
        ///   trả về all account 
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 16/03/2022 created
        /// </Modified>
        public List<Accounts> GetAllAccounts()
        {
            var listAccount = _hRMWebContext.Accounts.Where(x => x.IsDeleted == false).OrderByDescending(x => x.UpdateAt).ToList();
            return listAccount;
        }
        /// <summary>Gets the accounts.</summary>
        /// <param name="pageDto">The page dto.</param>
        /// <returns>
        ///   <br />
        ///   trả về account có phân trang
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// truongdv 16/03/2022 created
        /// </Modified>
        public ResponsePageAccountDto GetAccounts(PageDto pageDto)
        {
            var respone = new ResponsePageAccountDto();
            if (pageDto != null)
            {
                var ItemQuantity = (pageDto.PageQuantity - 1) * pageDto.ItemQuantityInPage;
                var listAccount = _hRMWebContext.Accounts.Where(x => x.IsDeleted == false).OrderByDescending(x => x.UpdateAt).Skip(ItemQuantity).Take(pageDto.ItemQuantityInPage).ToList();
                var allAccount = new AllAccountDto
                {
                    ItemQuantityInPage = pageDto.ItemQuantityInPage,
                    PageQuantity = pageDto.PageQuantity,
                    TotalItem = _hRMWebContext.Accounts.Count(),
                    TotalPage = Math.Ceiling((decimal)_hRMWebContext.Accounts.Count() / pageDto.ItemQuantityInPage)
                };
                 respone = new ResponsePageAccountDto
                {
                    Accounts = listAccount,
                    AllAccount = allAccount,
                };
            }
            return respone;
        }
    }
}
