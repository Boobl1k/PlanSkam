using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.Models;

namespace Planscam.Controllers;

public class HomeController : PsmControllerBase
{
    public HomeController(AppDbContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager) :
        base(dataContext, userManager, signInManager)
    {
    }

    //todo переписать полностью
    public async Task<IActionResult> Index()
    {
        var playlists = await DataContext.Playlists
            .Include(playlist => playlist.Picture)
            .AsNoTracking()
            .ToListAsync();
        if (!SignInManager.IsSignedIn(User)) return View(new HomePageViewModel {Playlists = playlists});
        CurrentUser = await CurrentUserQueryable
            .Include(user => user.Picture)
            .Include(user => user.FavouriteTracks)
            .Include(user => user.FavouriteTracks!.Picture)
            .AsNoTracking()
            .FirstAsync();
        CurrentUser.FavouriteTracks!.Tracks = DataContext.Tracks
            .Where(track => track.Playlists!.Contains(CurrentUser.FavouriteTracks!))
            .Include(track => track.Picture)
            .AsNoTracking()
            .ToList();
        return View(new HomePageViewModel
        {
            Playlists = playlists,
            User = CurrentUser
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});

    [HttpGet]
    public async Task<IActionResult> Search(string query)
    {
        var playlists = await DataContext.Playlists
            .Include(playlist => playlist.Picture)
            .Where(playlist => playlist.Name.Contains(query))
            .ToListAsync();
        var tracks = new Playlist
        {
            Name = $"search result, query = {query}",
            Tracks = await DataContext.Tracks
                .Where(track => track.Name.Contains(query))
                .Select(track => new Track
                {
                    Id = track.Id,
                    Name = track.Name,
                    Picture = track.Picture,
                    Author = track.Author,
                    IsLiked = CurrentUserQueryable.Select(user => user.FavouriteTracks!.Tracks!.Contains(track)).First()
                })
                .ToListAsync()
        };
        var authors = await DataContext.Authors
            .Include(author => author.Picture)
            .Where(author => author.Name.Contains(query))
            .ToListAsync();
        return View("SearchResult", new SearchAllViewModel
        {
            Playlists = playlists,
            Tracks = tracks,
            Authors = authors
        });
    }
}
