using FastFoodAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FastFoodAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Definición de la tabla
        public DbSet<Order> Orders { get; set; }
    }
}