using Microsoft.EntityFrameworkCore;

namespace WebHRM.Models
{
    public class HRMContext:DbContext
    {
        public HRMContext(DbContextOptions<HRMContext> options):base(options)
        {

        }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<EmployeeInformation> EmployeeInformation { get; set; }
    }
}
