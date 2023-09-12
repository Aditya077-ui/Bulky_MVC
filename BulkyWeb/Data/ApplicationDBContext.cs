using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Data
{
    public class ApplicationDBContext : DbContext   //a key component that represents the database session
                                                    //and allows you to interact with the database.
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base (options) 
        {
            
        }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)       //seed category table
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Action",
                    DisplayOrder = 1

                },
                new Category
                {
                    Id = 2,
                    Name = "Scifi",
                    DisplayOrder = 2

                },
                new Category
                {
                    Id = 3,
                    Name = "Historic",
                    DisplayOrder = 3

                }

                );
        }
    } 
}
