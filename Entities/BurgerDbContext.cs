using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class BurgerDbContext : IdentityDbContext
    {
        public BurgerDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Burger> Burgers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        //Seed data to Database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Burger>().ToTable("Burgers");


            modelBuilder.Entity<Burger>().HasData(
                new Burger
                {
                    Id = 1,
                    Name = "Cheeseburger",
                    Price = 12,
                    Ingredients = "Beef,Cheese,Tomato"
                },
                new Burger
                {
                    Id = 2,
                    Name = "Special",
                    Price = 15,
                    Ingredients = "Beef,Cheese,Bacon,Caramelized_Onion,Mushroom"
                },
                new Burger
                {
                    Id = 3,
                    Name = "Egger",
                    Price = 13,
                    Ingredients = "Beef,Cheese,Tomato,Egg,Letucce"
                });

        }
    }
}
