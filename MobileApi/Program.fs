namespace MobileApi

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Planscam.DataAccess
open System.Reflection
open Microsoft.AspNetCore.Identity
open Microsoft.EntityFrameworkCore
open Planscam.Entities
open Planscam.Services

module Program =
    [<EntryPoint>]
    let main args =
        let builder =
            WebApplication.CreateBuilder(args)

        builder.Services.AddControllers() |> ignore

        builder.Services.AddDbContext<AppDbContext> (fun options ->
            options.UseSqlServer(
                builder.Configuration.GetSection("ConnectionStrings")["MsSqlConnection"],
                fun action ->
                    action.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
                    |> ignore
            )
            |> ignore)
        |> ignore

        builder
            .Services
            .AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
        |> ignore

        builder.Services.AddSingleton<UsersRepo>()
        |> ignore

        builder.Services.AddScoped<PlaylistsRepo>()
        |> ignore

        let app = builder.Build()
        app.UseHttpsRedirection() |> ignore
        app.UseAuthorization() |> ignore
        app.MapControllers() |> ignore
        app.Run()
        0
