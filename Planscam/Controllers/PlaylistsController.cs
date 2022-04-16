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
            playlistId = await CurrentUserQueryable
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
        if (playlist is null) return NotFound();
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

    //TODO вызовы этого должны быть через ajax, и метод должен возвращать json с инфой об успешности
    [HttpGet, Authorize]
    public async Task<IActionResult> LikePlaylist(int playlistId, string returnUrl)
    {
        var playlist = await DataContext.Playlists
            .AsNoTracking()
            .FirstOrDefaultAsync(playlist => playlist.Id == playlistId);
        if (playlist is null) return BadRequest();
        (await CurrentUserQueryable
            .Include(user => user.Playlists!.Where(_ => false))
            .FirstAsync()).Playlists!.Add(playlist);
        await DataContext.SaveChangesAsync();
        return IsLocalUrl(returnUrl)
            ? Redirect(returnUrl)
            : RedirectToAction("Index", new {playlistId});
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Liked()
    {
        CurrentUser = await CurrentUserQueryable
            .Include(user => user.Playlists!)
            .AsNoTracking()
            .FirstAsync();
        await DataContext.Pictures
            .Where(picture => CurrentUser.Playlists!.Select(playlist => playlist.Picture).Contains(picture))
            .AsNoTracking()
            .LoadAsync();
        return View(CurrentUser);
    }
}
