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
        playlist.Tracks is { }
            ? new PlaylistsViewModel
            {
                Playlist = playlist
            }
            : throw new Exception("Include tracks into your playlist");

    [HttpGet, Route(nameof(FavoriteTracks)), Authorize]
    public async Task<IActionResult> FavoriteTracks() =>
        RedirectToAction("Index", new
        {
            playlistId = await DataContext.Users
                .Where(user => user.Id == UserManager.GetUserId(User))
                .Select(user => user.FavouriteTracks!.Id)
                .FirstAsync()
        });

    [HttpGet]
    public async Task<IActionResult> Index(int playlistId)
    {
        var playlist = await DataContext.Playlists
            .Include(playlist => playlist.Picture)
            .Include(playlist => playlist.Tracks)
            .FirstOrDefaultAsync(playlist => playlist.Id == playlistId);
        if (playlist is null) return BadRequest();
        await DataContext.Pictures
            .Where(picture => DataContext.Playlists
                .First(playlist1 => playlist1.Id == playlistId).Tracks!.Any(track => track.Picture == picture))
            .LoadAsync();
        return View(GetModel(playlist));
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> AddTrackToFavourite(int trackId)
    {
        var track = await DataContext.Tracks.Where(track => track.Id == trackId).FirstOrDefaultAsync();
        if (track is null) return BadRequest();
        var user = await base.GetCurrentUserAsync(user => user.FavouriteTracks!);
        //TODO спросить у тимера
        if (DataContext.Tracks
            .Where(track1 =>
                DataContext.Users
                    .First(user1 => user1 == user).FavouriteTracks!.Tracks!
                    .Contains(track1))
            .Contains(track))
            return BadRequest();
        user.FavouriteTracks!.Tracks!.Add(track);
        await DataContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> All() =>
        View(await DataContext.Playlists
            .Where(playlist => DataContext.FavouriteTracks.All(tracks => tracks != playlist))
            .ToListAsync());
}
