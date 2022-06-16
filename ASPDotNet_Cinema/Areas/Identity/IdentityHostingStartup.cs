using System;
using ASPDotNet_Cinema.Data;
using ASPDotNet_Cinema.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(ASPDotNet_Cinema.Areas.Identity.IdentityHostingStartup))]
namespace ASPDotNet_Cinema.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<CinemaIdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("CinemaIdentityContextConnection")));

                services.AddDefaultIdentity<CinemaUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<CinemaIdentityContext>();

                services.AddAuthorization();
            });
        }
    }
}