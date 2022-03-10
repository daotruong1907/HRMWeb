using WebHRM.Models;

namespace WebHRM.DTO
{
    public class PageDto
    {
        public int PageQuantity { get; set; }
        public int ItemQuantityInPage { get; set; }
    }

    public class AddAccountDto
    {
        public int Id { get; set; }
        public string PassWord { get; set; }
        public string Creator { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    public class UpdateAccountDto
    {
        public int Id { get; set; }  
        public string PassWord { get; set; }
        public DateTime UpdateAt { get; set; }
        public string Repairer { get; set; }
    }

    public class AllAccountDto
    {
        public int PageQuantity { get; set; }
        public int ItemQuantityInPage { get; set; }
        public decimal TotalPage { get; set; }
        public int TotalItem { get; set; }
    }

    public class ResponsePageAccountDto
    {
        public AllAccountDto AllAccount { get; set; }
        public List<Accounts> Accounts { get; set; }
    }

}
