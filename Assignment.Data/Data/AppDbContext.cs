using Assignment.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Setting> Setting { get; set; }
    }
}
