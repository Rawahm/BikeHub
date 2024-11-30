using BikeHub.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BikeHub.Repositories;
using OfficeOpenXml;


var builder = WebApplication.CreateBuilder(args);

// Set the LicenseContext globally for EPPlus
ExcelPackage.LicenseContext = LicenseContext.Commercial;  // Or LicenseContext.NonCommercial

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BikeHubDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<BikeHubDBContext>();

builder.Services.AddScoped<CustomerRepository>();

builder.Services.AddScoped<RentalRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


app.Run();
