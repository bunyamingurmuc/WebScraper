using Microsoft.EntityFrameworkCore;
using WebScraperAPI.Model;

namespace WebScraperAPI.Context
{
    public class ScrapperContext : DbContext
    {
        public ScrapperContext(DbContextOptions<ScrapperContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Filter> Filters{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Category>()
                .HasMany(x => x.Categories)
                .WithOne(x => x.ParentCategory)
                .HasForeignKey(x => x.ParentId);
            
            modelBuilder.Entity<Category>()
                .HasMany(x => x.Filters)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
