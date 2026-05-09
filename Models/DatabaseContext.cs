using Microsoft.EntityFrameworkCore;

namespace App_practical.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<Variant> Variants { get; set; }
    }
}