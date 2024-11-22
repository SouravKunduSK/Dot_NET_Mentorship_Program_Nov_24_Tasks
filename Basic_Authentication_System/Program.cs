using Basic_Authentication_System.Helpers;
using Basic_Authentication_System.Models;
using Basic_Authentication_System.Models.Database;
using Basic_Authentication_System.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Configure

builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
    

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine($"Token validated for user: {context.Principal?.Identity?.Name}");
                    return Task.CompletedTask;
                }
            };

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = "Basic_Authentication_System",
                ValidAudience = "all",
                ClockSkew = TimeSpan.Zero
            };
        });

// Add services to the container.
builder.Services.AddControllersWithViews();


//Database Configuration
builder.Services.AddDbContext<AuthDbContext>(options=>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
});

//Configure  helpers
builder.Services.AddTransient<PasswordHelper>();
builder.Services.AddScoped<JWTServiceHelper>();

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
