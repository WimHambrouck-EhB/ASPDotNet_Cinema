using ASPDotNet_Cinema.Data;
using ASPDotNet_Cinema.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ASPDotNet_Cinema.Areas.Identity.Data
{
    public static class UserAndRoleSeeder
    {
        // code based on: https://docs.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-3.1#create-the-test-accounts-and-update-the-contacts-1
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<CinemaUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            var user = await userManager.FindByNameAsync("staff@cinema.com");
            if (user == null)
            {
                user = new CinemaUser
                {
                    UserName = "staff@cinema.com",
                    Email = "staff@cinema.com",
                    EmailConfirmed = true
                };

                // Wachtwoord in de broncode is uiteraard onveilig, maar is nu om het voorbeeld niet te complex te maken
                // Normaal maak je gebruik van secrets:
                // https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows 
                await userManager.CreateAsync(user, "&KMPcA7k");


                if (!await roleManager.RoleExistsAsync(CinemaUser.STAFF_ROLE))
                {
                    await roleManager.CreateAsync(new IdentityRole(CinemaUser.STAFF_ROLE));
                }

                await userManager.AddToRoleAsync(user, CinemaUser.STAFF_ROLE);
            }
        }
    }
}
