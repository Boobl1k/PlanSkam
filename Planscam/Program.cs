using System.IO.Compression;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.FsServices;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<AppDbContext>(options=>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MsSqlConnection"), action =>
        action.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
    options.UseOpenIddict();
});
services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
services.AddSingleton<UsersRepo>();
services.AddScoped<PlaylistsRepo>();
services.AddScoped<AuthorsRepo>();

services.AddResponseCompression( option => option.EnableForHttps = true);
services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});
services.AddCors();
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
    .UseCors(builder => builder.AllowAnyOrigin())
    .UseAuthentication()
    .UseAuthorization();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.UseResponseCompression();
app.Run(async context =>
{
    string loremIpsum = "Lorem Ipsum";
    context.Response.ContentType = "text/plain";
    await context.Response.WriteAsync(loremIpsum);
});
