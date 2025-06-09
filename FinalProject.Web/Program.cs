using Microsoft.AspNetCore.Authentication.Cookies;
using FinalProject.Repository.Interfaces.User;
using FinalProject.Repository.Implementations;
using FinalProject.Repository.Interfaces.BankAccount;
using FinalProject.Repository.Interfaces.Payment;
using FinalProject.Repository.Interfaces.User_BankAccount;
using FinalProject.Services.Interfaces;
using FinalProject.Services.Implementations;
using FinalProject.Repository;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Set default culture to Bulgarian (bg-BG)
var cultureInfo = new CultureInfo("bg-BG");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUser_BankAccountRepository, User_BankAccountRepository>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
ConnectionFactory.Initialize(
    builder.Configuration.GetConnectionString("DefaultConnection"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
