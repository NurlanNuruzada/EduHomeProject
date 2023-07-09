using HomeEdu.Core.Entities;
using HomeEdu.DataAccess.Context;
using HomeEdu.UI.Services.Concretes;
using HomeEdu.UI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(identityOptions =>
{
    identityOptions.User.RequireUniqueEmail = true;
    identityOptions.Password.RequireNonAlphanumeric = true;
    identityOptions.Password.RequiredLength = 8;    
    identityOptions.Password.RequireDigit = true;
    identityOptions.Password.RequireUppercase = true;

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IBlogService, BlogService>();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization(); 
app.UseStaticFiles();
app.MapControllerRoute(
    name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{action=Index}/{Id?}");
app.Run();
