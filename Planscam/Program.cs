using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.FsServices;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection"), action =>
        action.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)));
services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
services.AddSingleton<UsersRepo>();
services.AddScoped<PlaylistsRepo>();

services.AddControllersWithViews();

services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Auth/Login");
    options.AccessDeniedPath = new PathString("/Auth/AccessDenied");//TODO
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    #region migrations

    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    #endregion

    #region roles

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!roleManager.Roles.Any())
        await roleManager.CreateAsync(new IdentityRole("Author"));

    #endregion
}


(app.Environment.IsDevelopment() ? app : app.UseExceptionHandler("/Home/Error").UseHsts())
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.Run();
