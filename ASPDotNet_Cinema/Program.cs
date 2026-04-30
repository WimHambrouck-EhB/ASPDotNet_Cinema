using ASPDotNet_Cinema.Areas.Identity.Data;
using ASPDotNet_Cinema.Data;
using ASPDotNet_Cinema.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<CinemaIdentityContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CinemaIdentityContextConnection")));

builder.Services.AddDefaultIdentity<CinemaUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CinemaIdentityContext>();

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CinemaIdentityContext>();
        await MovieSeeder.Initialize(context);
        await UserAndRoleSeeder.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Workaround om er voor te zorgen dat decimal 1.5 accepteert i.p.v. 1,5 (wegens LocalCulture nl-BE)
app.Use((context, next) =>
{
    var currentThreadCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
    currentThreadCulture.NumberFormat = NumberFormatInfo.InvariantInfo;

    Thread.CurrentThread.CurrentCulture = currentThreadCulture;
    Thread.CurrentThread.CurrentUICulture = currentThreadCulture;

    return next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
