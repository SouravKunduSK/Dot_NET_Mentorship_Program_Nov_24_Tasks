using Basic_Authentication_System.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Basic_Authentication_System.Models
{
    public class AuthDbContext:DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options):
            base(options)
        {
            
        }

        public DbSet<User> tblUsers { get; set; }
    }
}
