using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Services;
using UserManagement.Domain.Entities;
using UserManagement.Persistence.Data;
using UserManagement.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Connection String
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not found.");

// Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlOptions =>
            sqlOptions.MigrationsAssembly(
                "UserManagement.Persistence")));

// Database Interface
builder.Services.AddScoped<IDb>(serviceProvider =>
    serviceProvider.GetRequiredService<AppDbContext>());

// User CRUD Services
builder.Services.AddScoped<IAddUser, AddUser>();
builder.Services.AddScoped<IDeleteUser, DeleteUser>();
builder.Services.AddScoped<IUpdateUser, UpdateUser>();

builder.Services.AddScoped<IGetAllUsers, GetAllUsers>();
builder.Services.AddScoped<IGetUserById, GetUserById>();
builder.Services.AddScoped<IGetUserByUsername, GetUserByUsername>();
builder.Services.AddScoped<IUsernameExists, UsernameExists>();

// Authentication and Registration Services
builder.Services.AddScoped<IRegisterUser, RegisterUser>();
builder.Services.AddScoped<IAuthenticateUser, AuthenticateUser>();

// Password Services
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Authentication
builder.Services
    .AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";

        options.Cookie.Name = "UserManagement.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;

        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("Admin");
    });
});

var app = builder.Build();

// HTTP Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();