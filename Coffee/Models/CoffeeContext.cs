using Microsoft.EntityFrameworkCore;

namespace Coffee.Models
{
    public class CoffeeContext : DbContext
    {
        public CoffeeContext(DbContextOptions<CoffeeContext> options): base(options){}
        public DbSet<CoffeeItem> CoffeeItems { get; set; }
        public DbSet<Registration> User { get; set; }
    }
} 
    