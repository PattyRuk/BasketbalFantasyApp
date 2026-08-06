using BasketbalFantasyApp.DAL;
using BasketbalFantasyApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BasketbalFantasyApp
{
    public class Program
    {
        // Changed 'void' to 'async Task' to use 'await' inside Main
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BasketbalFantasyDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>() // Required to support the Admin/User roles in seeding script
                .AddEntityFrameworkStores<BasketbalFantasyDbContext>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var database = services.GetRequiredService<BasketbalFantasyDbContext>();

                await IdentityData.SeedRolesAndUsersAsync(roleManager, userManager);
                await SampleData.SeedDatabaseAsync(database);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Changed app.Run() to app.RunAsync() 
            await app.RunAsync();
        }
    }
}
