using CBTD.ApplicationCore.Interfaces;
using CBTD.ApplicationCore.Models;
using CBTD.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTD.DataAccess
{
	public class DbInitializer : IDbInitializer
	{
		private readonly ApplicationDbContext _db;

		private readonly UserManager<IdentityUser> _userManager;

		private readonly RoleManager<IdentityRole> _roleManager;
		public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager)
		{
			_db = db;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public void Initialize()
		{
			_db.Database.EnsureCreated();

			//migrations if they are not applied
			try
			{
				if (_db.Database.GetPendingMigrations().Any())
				{
					_db.Database.Migrate();
				}
			}
			catch (Exception)
			{

			}

			if (_db.Categories.Any())
			{
				return; //DB has been seeded
			}

			//create roles if they are not created

			_roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
			_roleManager.CreateAsync(new IdentityRole(SD.ShipRole)).GetAwaiter().GetResult();
			_roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

			//Create "Super Admin"

			_userManager.CreateAsync(new ApplicationUser
			{
				UserName = "rfry@weber.edu",
				Email = "rfry@weber.edu",
				FirstName = "Richard",
				LastName = "Fry",
				PhoneNumber = "8015556919",
				StreetAddress = "123 Main Street",
				State = "UT",
				PostalCode = "84408",
				City = "Ogden"
			}, "Admin123*").GetAwaiter().GetResult();

			ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "rfry@weber.edu");

			_userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();


			var Categories = new List<Category>
			{

			new Category { Name = "Non-Alcoholic Beverages", DisplayOrder = 1 },
			new Category { Name = "Wine", DisplayOrder = 2 },
			new Category { Name = "Snacks", DisplayOrder = 3 }
			};

			foreach (var c in Categories)
			{
				_db.Categories.Add(c);
			}
			_db.SaveChanges();

			var Manufacturers = new List<Manufacturer>
			{
			new Manufacturer { Name = "Coca Cola" },
			new Manufacturer { Name = "Yellow Tail"},
			new Manufacturer { Name = "Trinchero Family Estates" },
			new Manufacturer { Name = "Frito Lay" }
			};

			foreach (var m in Manufacturers)
			{
				_db.Manufacturers.Add(m);
			}
			_db.SaveChanges();

			var Products = new List<Product>
			{
			new Product {
					Name = "Coca Cola Classic",
					Description = "The primary taste of Coca-Cola is thought to come from vanilla and cinnamon, with trace amounts of essential oils, and spices such as nutmeg.",
					ListPrice = 1.99M,
					UnitPrice = 1.49M,
					HalfDozenPrice = 1.24M,
					DozenPrice = .99M,
					Size = "12 oz",
					UPC = "4894034",
					ImageUrl = "/images/products/Coke.jpg",
					CategoryId = 1,
					ManufacturerId =1
				},
				new Product
				{

					Name = "Yellow Tail Shiraz",
					Description = "<p>The Yellow Tail Shiraz has a deep red color with bright purple hues, characteristic of fine young wines. It displays impressive <strong>spice, licorice, and black currant aromas</strong>. The palate is perfectly balanced with soft tannins and fine French Oak, further complemented by ripe fruit flavors.</p>",
					ListPrice = 9.99M,
					UnitPrice = 8.99M,
					HalfDozenPrice = 7.99M,
					DozenPrice = 6.99M,
					Size = "750 ml",
					UPC = "031259008943",
					ImageUrl = "/images/products/YellowTail.png",
					CategoryId = 2,
					ManufacturerId = 2
				},
				new Product
				{

					Name = "Menage a Trois Merlot",
					Description = "Menage a Trois California Red Blend exposes the fresh, ripe, jam-like fruit that is the calling card of California wine. Forward, spicy and soft, this delicious dalliance makes the perfect accompaniment for grilled meats or chicken.",
					ListPrice = 12.99M,
					UnitPrice = 11.49M,
					HalfDozenPrice = 10.75M,
					DozenPrice = 9.99M,
					Size = "750 ml",
					UPC = "099988071096",
					ImageUrl = "/images/products/menage.jpg",
					CategoryId = 2,
					ManufacturerId = 3
				},
				new Product
				{

					Name = "Doritos",
					Description = "The chip that packs a flavorful punch with the classic crunch. Boldly seasoned with three cheeses, tomatoes, onions, and a savory blend of spices. Indulge yourself or share with large gatherings",
					ListPrice = 1.99M,
					UnitPrice = 1.49M,
					HalfDozenPrice = 1.05M,
					DozenPrice = .69M,
					Size = "1.75 oz",
					UPC = "028400443753",
					ImageUrl = "/images/products/doritos.jpg",
					CategoryId = 3,
					ManufacturerId = 4
				},
				new Product
				{

					Name = "Cheetos",
					Description = "The fun, crunchy snack that is made with real cheese. Packed with flavor that satisfies. Always a crowd favorite.",
					ListPrice = 1.99M,
					UnitPrice = 1.49M,
					HalfDozenPrice = 1.05M,
					DozenPrice = .69M,
					Size = "2 oz",
					UPC = "028400443661",
					ImageUrl = "/images/products/cheetos.jpg",
					CategoryId = 3,
					ManufacturerId = 4
				}
			};

			foreach (var p in Products)
			{
				_db.Products.Add(p);
			}
			_db.SaveChanges();

		}

	}
}
