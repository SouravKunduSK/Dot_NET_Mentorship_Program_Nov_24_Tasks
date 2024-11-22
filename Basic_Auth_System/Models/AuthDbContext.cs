using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Basic_Auth_System.Models
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<User> tblUsers { get; set; }
    }
}
