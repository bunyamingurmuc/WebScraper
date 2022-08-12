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

    }
}
