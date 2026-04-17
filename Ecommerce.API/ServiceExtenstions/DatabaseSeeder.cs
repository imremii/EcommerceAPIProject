using Ecommerce.DAL;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.API
{
    public static class DatabaseSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roles = { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            const string adminEmail = "admin@ecommerce.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser is null)
            {
                adminUser = new AppUser
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@12345");
                if (!result.Succeeded)
                    return;
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
