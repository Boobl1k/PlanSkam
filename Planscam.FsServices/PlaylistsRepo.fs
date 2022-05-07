namespace Planscam.FsServices

open Microsoft.AspNetCore.Identity
open Microsoft.EntityFrameworkCore
open Microsoft.FSharp.Core
open Planscam.DataAccess
open Planscam.Entities
open System.Linq

type PlaylistsRepo(dataContext: AppDbContext, userManager: UserManager<User>) =

    let userId user = userManager.GetUserId user
    let mutable currentUserQueryable = None

    let CurrentUserQueryable userPrincipal =
        match currentUserQueryable with
        | Some r -> r
        | None ->
            let value =
                dataContext.Users.Where(fun user -> user.Id = userId userPrincipal)

            currentUserQueryable <- Some(value)
            value

    member this.GetLikedPlaylists(userPrincipal) =
        CurrentUserQueryable(userPrincipal)
            .Select(fun user -> user.Playlists)
        |> EntityFrameworkQueryableExtensions.FirstAsync

    member this.GetFavouriteTracksId(userPrincipal) =
        CurrentUserQueryable(userPrincipal)
            .AsNoTracking()
            .Select(fun user -> user.FavouriteTracks.Id)
        |> EntityFrameworkQueryableExtensions.FirstAsync