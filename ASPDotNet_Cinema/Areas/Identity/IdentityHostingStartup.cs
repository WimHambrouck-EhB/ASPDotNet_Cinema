using System;
using ASPDotNet_Cinema.Areas.Identity.Data;
using ASPDotNet_Cinema.Data;
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
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ASPDotNet_CinemaContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ASPDotNet_CinemaContextConnection")));

                services.AddDefaultIdentity<ASPDotNet_Gebruiker>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ASPDotNet_CinemaContext>();
            });
        }
    }
}