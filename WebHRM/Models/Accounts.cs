using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebHRM.Models
{
    public class Accounts
    {

        [ForeignKey("EmployeeInformation")]
        public int Id { get; set; }

        public string PassWord { get; set; }
        
        public int CreatorId { get; set; }

        public DateTime CreateAt { get; set; }

        public int? RepairerId { get; set; }

        public bool? IsUpdated { get; set; }

        public DateTime UpdateAt { get; set; }

        public int? EraserId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeleteAt { get; set; }

        public virtual EmployeeInformation EmployeeInformation { get; set; }

    }
}
