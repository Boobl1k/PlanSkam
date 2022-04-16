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
                .AsNoTracking()
                .Select(user => user.FavouriteTracks!.Id)
                .FirstAsync()
        });

    [HttpGet]
    public async Task<IActionResult> Index(int playlistId)
    {
        var playlist = await DataContext.Playlists
            .Include(playlist => playlist.Picture)
            .Include(playlist => playlist.Tracks)
            .AsNoTracking()
            .FirstOrDefaultAsync(playlist => playlist.Id == playlistId);
        if (playlist is null) return BadRequest();
        await DataContext.Pictures
            .Where(picture => DataContext.Playlists
                .First(playlist1 => playlist1.Id == playlistId).Tracks!.Any(track => track.Picture == picture))
            .AsNoTracking()
            .LoadAsync();
        return View(GetModel(playlist));
    }

    [HttpGet]
    public async Task<IActionResult> All() =>
        View(await DataContext.Playlists
            .Where(playlist => DataContext.FavouriteTracks.All(tracks => tracks != playlist))
            .ToListAsync());

    [HttpGet]
    public async Task<IActionResult> LikePlaylist(int playlistId, string returnUrl)
    {
        var playlist = await DataContext.Playlists
            .AsNoTracking()
            .FirstOrDefaultAsync(playlist => playlist.Id == playlistId);
        if (playlist is null)
            return BadRequest();
        (await GetCurrentUserAsync(user => user.Playlists!.Where(_ => false))).Playlists!.Add(playlist);
        await DataContext.SaveChangesAsync();
        return IsLocalUrl(returnUrl)
            ? Redirect(returnUrl)
            : RedirectToAction("Index", new {playlistId});
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Liked()
    {
        await GetCurrentUserAsync(user => user.Playlists!);
        await DataContext.Pictures
            .Where(picture => CurrentUser.Playlists!.Select(playlist => playlist.Picture).Contains(picture))
            .AsNoTracking()
            .LoadAsync();
        DataContext.Update(CurrentUser);
        return View(CurrentUser);
    }
}
