using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;

namespace Planscam.Controllers;

public class PlaylistsController : PsmControllerBase
{
    public PlaylistsController(AppDbContext dataContext, UserManager<User> userManager,
        SignInManager<User> signInManager) : base(dataContext, userManager, signInManager)
    {
    }

    [HttpGet, Route(nameof(FavoriteTracks)), Authorize]
    public async Task<IActionResult> FavoriteTracks() =>
        RedirectToAction("Index", new
        {
            Id = await CurrentUserQueryable
                .AsNoTracking()
                .Select(user => user.FavouriteTracks!.Id)
                .FirstAsync()
        });

    [HttpGet]
    public async Task<IActionResult> Index(int id)
    {
        //TODO по хорошему надо найти способ сделать это одним запросом, как я понимаю это делается с помощью хранимых процедур
        var playlist = await DataContext.Playlists
            .Include(playlist => playlist.Picture)
            .Include(playlist => playlist.Tracks)!
            .ThenInclude(track => track.Picture)
            .AsNoTracking()
            .Select(PlaylistSetIsLikedExpression)
            .FirstOrDefaultAsync(playlist => playlist.Id == id);
        if (playlist is null) return NotFound();
        playlist.Tracks = await DataContext.Tracks
            .Where(track => playlist.Tracks!.Contains(track))
            //.Include(track => track.Picture)
            .AsNoTracking()
            .Select(TrackSetIsLikedExpression)
            .ToListAsync();
        return View(playlist);
    }

    [HttpGet]
    public async Task<IActionResult> All() =>
        View(await DataContext.Playlists
            .Where(playlist => DataContext.FavouriteTracks.All(tracks => tracks != playlist))
            .Include(playlist => playlist.Picture)
            .AsNoTracking()
            .Select(PlaylistSetIsLikedExpression)
            .ToListAsync());

    //TODO вызовы этого должны быть через ajax, и метод должен возвращать json с инфой об успешности
    [HttpPost, Authorize]
    public async Task<IActionResult> LikePlaylist(int id, string? returnUrl)
    {
        var playlist = await DataContext.Playlists
            .AsNoTracking()
            .FirstOrDefaultAsync(playlist => playlist.Id == id);
        if (playlist is null) return BadRequest();
        (await CurrentUserQueryable
            .Include(user => user.Playlists!.Where(_ => false))
            .FirstAsync()).Playlists!.Add(playlist);
        await DataContext.SaveChangesAsync();
        return IsLocalUrl(returnUrl)
            ? Redirect(returnUrl!)
            : RedirectToAction("Index", new {id});
    }

    //TODO ajax
    [HttpPost, Authorize]
    public async Task<IActionResult> UnlikePlaylist(int id, string? returnUrl)
    {
        CurrentUser = await CurrentUserQueryable
            .Include(user => user.Playlists!.Where(playlist => playlist.Id == id))
            .FirstAsync();
        if (!CurrentUser.Playlists!.Any()) return BadRequest();
        CurrentUser.Playlists!.Clear();
        await DataContext.SaveChangesAsync();
        return IsLocalUrl(returnUrl)
            ? Redirect(returnUrl!)
            : RedirectToAction("Index", new {id});
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Liked()
    {
        CurrentUser = await CurrentUserQueryable
            .Include(user => user.Playlists!)
            .ThenInclude(playlist => playlist.Picture)
            .AsNoTracking()
            .FirstAsync();
        CurrentUser.Playlists!.ForEach(playlist => playlist.IsLiked = true);
        return View(CurrentUser);
    }
}
