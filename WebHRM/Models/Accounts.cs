using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebHRM.Models
{
    public class Accounts
    {

        [ForeignKey("EmployeeInformation")]
        public int Id { get; set; }
        public string PassWord { get; set; }
        public string Creator { get; set; }
        public DateTime CreateAt { get; set; }
        public string? Repairer { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public virtual EmployeeInformation EmployeeInformation { get; set; }

    }
}
