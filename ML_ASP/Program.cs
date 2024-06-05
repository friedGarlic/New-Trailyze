using Microsoft.EntityFrameworkCore;
using ML_ASP.DataAccess;
using ML_ASP.DataAccess.Repositories.IRepositories;
using ML_ASP.DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using ML_ASP.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting.Internal;
using ML_ASP.Models;
using ML_ASP.Areas.Identity.Pages.Account;
using Microsoft.ML;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register custom services
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSingleton<MLContext>();

builder.Services.AddHttpContextAccessor();

// Register the DbContext
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AzureConnection")));

// Register Identity services
builder.Services.AddIdentity<Account_Model, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

// Add Razor Pages if using Identity UI
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure authentication middleware is added
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Ensure Razor Pages are mapped

app.Run();
