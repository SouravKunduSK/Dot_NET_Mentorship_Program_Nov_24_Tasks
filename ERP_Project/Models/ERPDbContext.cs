using Microsoft.EntityFrameworkCore;

namespace ERP_Project.Models
{
    public class ERPDbContext:DbContext
    {
        public ERPDbContext(DbContextOptions<ERPDbContext> options):
            base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(p => p.Product)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.ProductId);
        }

        public DbSet<Product> tblProducts { get; set; }
        public DbSet<Order> tblOrders { get; set; }
    }
}
