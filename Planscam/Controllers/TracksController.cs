using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.Models;

namespace Planscam.Controllers;

public class TracksController : PsmControllerBase
{
    public TracksController(AppDbContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager)
        : base(dataContext, userManager, signInManager)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? id) =>
        id is null
            ? View()
            : await DataContext.Tracks
                .Include(t => t.Picture)
                .Include(t => t.Author)
                .FirstOrDefaultAsync(t => t.Id == id) is { } track
                ? View(new TrackIndexViewModel
                {
                    Track = track,
                    NotAddedPlaylists = await DataContext.Playlists
                        .Where(playlist =>
                            CurrentUserQueryable
                                .Include(user => user.OwnedPlaylists!.Playlists)
                                .First()
                                .OwnedPlaylists!.Playlists!.Contains(playlist)
                            && !playlist.Tracks!.Contains(track))
                        .ToListAsync()
                })
                : NotFound();

    [HttpGet]
    public async Task<IActionResult> Search(TrackSearchViewModel? model)
    {
        if (model is null || !ModelState.IsValid) return View();
        var tracks = DataContext.Tracks
            .Where(model.ByAuthors
                ? track => track.Author!.Name.Contains(model.Query)
                : track => track.Name.Contains(model.Query));
        model.Result = new Playlist
        {
            Name = $"Search result, query: '{model.Query}'",
            Picture = tracks.Select(track => track.Picture).FirstOrDefault(picture => picture != null),
            Tracks = await tracks
                .Include(track => track.Picture)
                .Include(track => track.Author)
                .Select(TrackSetIsLikedExpression)
                .ToListAsync()
        };
        return View(model);
    }

    //TODO вызовы этого должны быть через ajax, и метод должен возвращать json с инфой об успешности
    [HttpPost, Authorize]
    public async Task<IActionResult> AddTrackToFavourite(int id, string? returnUrl)
    {
        var track = await DataContext.Tracks.Where(track => track.Id == id).FirstOrDefaultAsync();
        if (track is null) return BadRequest();
        CurrentUser = await CurrentUserQueryable
            .Include(user =>
                user.FavouriteTracks!.Tracks!.Where(track1 => track1.Id == id))
            .FirstAsync();
        if (CurrentUser.FavouriteTracks!.Tracks!.Contains(track)) return BadRequest();
        CurrentUser.FavouriteTracks!.Tracks!.Add(track);
        await DataContext.SaveChangesAsync();
        return IsLocalUrl(returnUrl)
            ? Redirect(returnUrl!)
            : RedirectToAction("Index", "Home");
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> RemoveTrackFromFavourite(int id, string? returnUrl)
    {
        var favTracks = await DataContext.Users
            .Where(user => user.Id == CurrentUserId)
            .Include(user => user.FavouriteTracks!.Tracks!.Where(track => track.Id == id))
            .Select(user => user.FavouriteTracks!)
            .FirstAsync();
        if (!favTracks.Tracks!.Any()) return BadRequest();
        favTracks.Tracks!.Remove(favTracks.Tracks.First());
        await DataContext.SaveChangesAsync();
        return IsLocalUrl(returnUrl)
            ? Redirect(returnUrl!)
            : RedirectToAction("Index", "Playlists", new {favTracks.Id});
    }

    [HttpGet]
    public async Task<IActionResult> GetTrackData(int id) =>
        await DataContext.Tracks
                .Include(track => track.Data)
                .Select(track => new
                {
                    Id = track.Id,
                    Author = track.Author!.Name,
                    Name = track.Name,
                    IsLiked = track.IsLiked,
                    Picture = track.Picture!.Data,
                    Data = track.Data!.Data
                })
                .FirstOrDefaultAsync(track => track.Id == id) switch
            {
                { } track => Json(track),
                _ => NotFound()
            };
}
