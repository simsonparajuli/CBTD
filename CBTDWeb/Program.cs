using CBTD.DataAccess;
using Microsoft.EntityFrameworkCore;

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

            builder.Services.AddScoped<UnitOfWork, UnitOfWork>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.MigrationsAssembly("CBTD.DataAccess")));



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

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}