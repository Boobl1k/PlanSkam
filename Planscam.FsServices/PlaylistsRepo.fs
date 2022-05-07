namespace Planscam.FsServices

open Microsoft.AspNetCore.Identity
open Microsoft.EntityFrameworkCore
open Microsoft.FSharp.Control
open Microsoft.FSharp.Core
open Planscam.DataAccess
open Planscam.Entities
open System.Linq

type PlaylistsRepo(dataContext: AppDbContext, userManager: UserManager<User>, signInManager: SignInManager<User>) =

    let userId user = userManager.GetUserId user
    let mutable _userQueryable = None

    let userQueryable userPrincipal =
        match _userQueryable with
        | Some r -> r
        | None ->
            let value =
                dataContext.Users.Where(fun user -> user.Id = userId userPrincipal)

            _userQueryable <- Some(value)
            value

    member this.GetLikedPlaylists(userPrincipal) =
        userQueryable(userPrincipal)
            .Select(fun user -> user.Playlists)
            .FirstAsync()

    member this.GetFavouriteTracksId(userPrincipal) =
        userQueryable(userPrincipal)
            .AsNoTracking()
            .Select(fun user -> user.FavouriteTracks.Id)
            .FirstAsync()

    member this.GetPlaylistFull(id, userPrincipal) =
        (if signInManager.IsSignedIn(userPrincipal) then
             (query {
                 for playlist in dataContext.Playlists do
                     where (playlist.Id = id)

                     select (
                         Playlist(
                             Id = playlist.Id,
                             Name = playlist.Name,
                             Picture = playlist.Picture,
                             Tracks =
                                 (query {
                                     for track in playlist.Tracks do
                                         select (
                                             Track(
                                                 Id = track.Id,
                                                 Name = track.Name,
                                                 Picture = track.Picture,
                                                 Author = track.Author,
                                                 IsLiked =
                                                     (query {
                                                         for user in userQueryable userPrincipal do
                                                             select (user.FavouriteTracks.Tracks.Contains(track))
                                                      })
                                                         .First()
                                             )
                                         )
                                  })
                                     .ToList(),
                             IsLiked =
                                 (query {
                                     for user in userQueryable userPrincipal do
                                         select (user.Playlists.Any(fun p -> p = playlist))
                                  })
                                     .First(),
                             IsOwned =
                                 (query {
                                     for user in userQueryable userPrincipal do
                                         select (user.OwnedPlaylists.Playlists.Any(fun p -> p = playlist))
                                  })
                                     .First()
                         )
                     )
              })
         else
             (query {
                 for playlist in dataContext.Playlists do
                     where (playlist.Id = id)

                     select (
                         Playlist(
                             Id = playlist.Id,
                             Name = playlist.Name,
                             Picture = playlist.Picture,
                             Tracks =
                                 (query {
                                     for track in playlist.Tracks do
                                         select (
                                             Track(
                                                 Id = track.Id,
                                                 Name = track.Name,
                                                 Picture = track.Picture,
                                                 Author = track.Author
                                             )
                                         )
                                  })
                                     .ToList()
                         )
                     )
              }))
            .AsNoTracking()
            .FirstOrDefaultAsync()

    member this.LikePlaylist(userPrincipal, id) =
        match (query {
                   for playlist in dataContext.Playlists do
                       where (playlist.Id = id)
               })
            .FirstOrDefault() with
        | null -> false
        | playlist ->
            userQueryable(userPrincipal)
                .Include(fun user -> user.Playlists.Where(fun _ -> false))
                .First()
                .Playlists.Add(playlist)

            dataContext.SaveChanges() |> ignore
            true

    member this.UnlikePlaylist(userPrincipal, id) =
        let user =
            userQueryable(userPrincipal)
                .Include(fun user -> user.Playlists.Where(fun playlist -> playlist.Id = id))
                .First()

        if user.Playlists.Any() then
            user.Playlists.Clear()
            dataContext.SaveChanges() |> ignore
            true
        else
            false