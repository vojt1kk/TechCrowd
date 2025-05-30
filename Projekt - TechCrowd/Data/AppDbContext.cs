using Microsoft.EntityFrameworkCore;
using Projekt___TechCrowd.Models;

namespace Projekt___TechCrowd.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Articles> Articles { get; set; }
    }
}
