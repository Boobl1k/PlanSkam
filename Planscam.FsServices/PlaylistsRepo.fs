namespace Planscam.FsServices

open System.Security.Claims
open Microsoft.AspNetCore.Identity
open Microsoft.EntityFrameworkCore
open Planscam.DataAccess
open Planscam.Entities
open System.Linq

type PlaylistsRepo(dataContext: AppDbContext, userManager: UserManager<User>) =

    member this.GetLikedPlaylists(currentUser: ClaimsPrincipal) =
        EntityFrameworkQueryableExtensions.FirstAsync(
            dataContext
                .Users
                .Where(fun user -> user.Id = userManager.GetUserId(currentUser))
                .Select(fun user -> user.Playlists)
        )