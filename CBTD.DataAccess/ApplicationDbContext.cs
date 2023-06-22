using CBTD.ApplicationCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CBTD.DataAccess;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Manufacturer> Manufacturers { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public DbSet<OrderHeader> OrderHeader { get; set; }
    public DbSet<OrderDetails> OrderDetail { get; set; }


    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Category>().HasData(
    //        new Category { Id = 1, Name = "Beverages", DisplayOrder = 1 },
    //        new Category { Id = 2, Name = "Wine", DisplayOrder = 2 },
    //        new Category { Id = 3, Name = "Books", DisplayOrder = 3 }
    //       );

    //    modelBuilder.Entity<Manufacturer>().HasData
    //        (new Manufacturer { Id = 1, Name = "Coca-Cola" },
    //          new Manufacturer { Id = 2, Name = "Vineyard" },
    //          new Manufacturer { Id = 3, Name = "oreilly" }
    //        );
    //}
}

