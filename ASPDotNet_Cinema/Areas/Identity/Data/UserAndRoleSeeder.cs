using ASPDotNet_Cinema.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ASPDotNet_Cinema.Areas.Identity.Data
{
    public class UserAndRoleSeeder
    {
        // code gebaseerd op: https://docs.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-3.1#create-the-test-accounts-and-update-the-contacts-1
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<CinemaUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            var context = serviceProvider.GetRequiredService<CinemaIdentityContext>();
            context.Database.Migrate();

            var newUser = new CinemaUser
            {
                UserName = "staff@cinema.com",
                Email = "staff@cinema.com",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(newUser, "&KMPcA7k");

            if (!await roleManager.RoleExistsAsync(CinemaUser.STAFF_ROLE))
            {
                await roleManager.CreateAsync(new IdentityRole(CinemaUser.STAFF_ROLE));
            }

            await userManager.AddToRoleAsync(newUser, CinemaUser.STAFF_ROLE);
        }
    }
}
