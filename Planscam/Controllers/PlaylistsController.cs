using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.Models;

namespace Planscam.Controllers;

public class PlaylistsController : PsmControllerBase
{
    public PlaylistsController(AppDbContext dataContext, UserManager<User> userManager,
        SignInManager<User> signInManager) : base(dataContext, userManager, signInManager)
    {
    }

    private static PlaylistsViewModel GetModel(Playlist playlist) =>
        playlist is {Tracks: { }}
            ? new PlaylistsViewModel
            {
                Name = playlist.Name,
                Picture = playlist.Picture,
                Tracks = playlist.Tracks
            }
            : throw new Exception(); //TODO

    [HttpGet, Route(nameof(FavoriteTracks)), Authorize]
    public async Task<IActionResult> FavoriteTracks()
    {
        await GetCurrentUserAsync(user => user.FavouriteTracks!.Tracks!, user => user.FavouriteTracks!.Picture!);
        //для подгрузки картинок нужных треков в оперативу
        await DataContext.Pictures
            .Where(picture => CurrentUser!.FavouriteTracks!.Tracks!.Any(track => track.Picture == picture))
            .LoadAsync();
        return View(GetModel(CurrentUser!.FavouriteTracks!));
    }

    [HttpGet]
    public async Task<IActionResult> Index(int playlistId)
    {
        var playlist = await DataContext.Playlists.FirstOrDefaultAsync(playlist => playlist.Id == playlistId);
        if (playlist is null) return BadRequest();
        return View(GetModel(playlist));
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> AddTrackToFavourite(int trackId)
    {
        var track = await DataContext.Tracks.Where(track => track.Id == trackId).FirstOrDefaultAsync();
        if (track is null) return BadRequest();
        var user = await base.GetCurrentUserAsync(user => user.FavouriteTracks!.Tracks!);
        user.FavouriteTracks!.Tracks!.Add(track);
        DataContext.Users.Update(user);
        return Ok();
    }
}
