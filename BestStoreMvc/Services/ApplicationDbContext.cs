using BestStoreMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace BestStoreMvc.Services
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
    }
}
