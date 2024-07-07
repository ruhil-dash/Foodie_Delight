using System;
using Microsoft.EntityFrameworkCore;

namespace FoodieDelight_Api.Data
{
	public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) :base(options){ }

		public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.MenuItems)
                .WithOne(mi => mi.Restaurant)
                .HasForeignKey(mi => mi.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if a Restaurant is deleted
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "server=localhost;port=3306;database=mydb;user=myuser;password=mypassword";
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

    }
}

