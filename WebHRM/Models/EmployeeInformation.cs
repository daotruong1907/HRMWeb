using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebHRM.Models
{
    public class EmployeeInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public int? Sex { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        [Required]
        public string? Email { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreateAt { get; set; }
        public int? RepairerId { get; set; }
        public bool IsUpdated { get; set; }
        public DateTime UpdateAt { get; set; }
        public int? EraserId { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public DateTime? DeleteAt { get; set; }
        public virtual Accounts Accounts { get; set; }
    }
}
