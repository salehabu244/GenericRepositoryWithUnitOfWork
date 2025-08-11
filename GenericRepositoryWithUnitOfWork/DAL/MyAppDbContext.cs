using GenericRepositoryWithUnitOfWork.Entity;
using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryWithUnitOfWork.DAL
{
    public class MyAppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public MyAppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId);
        }
    }
}
