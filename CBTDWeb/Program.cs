using CBTD.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using CBTD.Utility;
using Stripe;

namespace CBTDWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
     
           // builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
           //      builder.Configuration.GetConnectionString("DefaultConnection")
           //      ));

            builder.Services.AddScoped<UnitOfWork>();

			builder.Services.AddScoped<DbInitializer>();

			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.MigrationsAssembly("CBTD.DataAccess")));

			builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
	              .AddEntityFrameworkStores<ApplicationDbContext>();

			builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


			builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });


			builder.Services.AddSingleton<IEmailSender, EmailSender>();


			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            SeedDatabase();

			StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

			app.UseAuthorization();

            app.MapRazorPages();

            app.Run();

			void SeedDatabase()
			{
				using var scope = app.Services.CreateScope();
				var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
				dbInitializer.Initialize();
			}

		}
	}
}