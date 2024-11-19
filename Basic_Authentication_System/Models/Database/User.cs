using System.ComponentModel.DataAnnotations;

namespace Basic_Authentication_System.Models.Database
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string PasswordHash { get; set; }
        public DateTime? LockOutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; } = 0;
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}
