using CBTDWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace CBTDWeb.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Beverages", DisplayOrder = 1 },
            new Category { Id = 2, Name = "Wine", DisplayOrder = 2 },
            new Category { Id = 3, Name = "Books", DisplayOrder = 3 }
           );
    }
}

