using Microsoft.AspNetCore.Identity;

namespace BasketbalFantasyApp.Data
{
    public static class IdentityData
    {
        public static async Task SeedRolesAndUsersAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            string[] roleNames = { "Admin", "StandardUser" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            string adminEmail = "admin@fantasyleague.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(adminUser, "SecureAdmin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            string userEmail = "player@fantasyleague.com";
            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var standardUser = new IdentityUser { UserName = userEmail, Email = userEmail, EmailConfirmed = true };
                var result = await userManager.CreateAsync(standardUser, "PlayerPassword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(standardUser, "StandardUser");
                }
            }
        }
    }
}
