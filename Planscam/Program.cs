using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection"), action =>
        action.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)));
services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

services.AddControllersWithViews();

var app = builder.Build();

#region migrations

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

#endregion

(app.Environment.IsDevelopment() ? app : app.UseExceptionHandler("/Home/Error").UseHsts())
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
