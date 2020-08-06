using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_Role_Based_Authorization.Constant;

namespace WebAPI_Role_Based_Authorization.Data.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.Roles.ADMINISTRATORS));

            string adminUserName = "admin@demo.com";
            var adminUser = new ApplicationUser { Id = "admin1", UserName = adminUserName, FirstName = "Admin", LastName = "Demo", Email = "admin@demo.com", EmailConfirmed = true };
            await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
            adminUser = await userManager.FindByNameAsync(adminUserName);
            await userManager.AddToRoleAsync(adminUser, AuthorizationConstants.Roles.ADMINISTRATORS);
        }
    }
}
