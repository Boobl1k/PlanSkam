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
    public IActionResult Search() =>
        View();

    [HttpPost]
    public async Task<IActionResult> Search(TrackSearchViewModel model)
    {
        var tracks = DataContext.Tracks
            .Where(model.ByAuthors
                ? track => track.Author!.Name.Contains(model.Query)
                : track => track.Name.Contains(model.Query));
        model.Result = new Playlist
        {
            Name = $"Search result, query: '{model.Query}'",
            Picture = tracks.Select(track => track.Picture).FirstOrDefault(picture => picture != null),
            Tracks = await tracks.ToListAsync()
        };
        
        return View(model);
    }
    
    [HttpPost, Authorize]
    public async Task<IActionResult> AddTrackToFavourite(int trackId, string returnUrl)
    {
        var track = await DataContext.Tracks.Where(track => track.Id == trackId).FirstOrDefaultAsync();
        if (track is null ||
            (await base.GetCurrentUserAsync(user => user.FavouriteTracks!.Tracks!))
            .FavouriteTracks!.Tracks!.Contains(track))
            return BadRequest();
        CurrentUser!.FavouriteTracks!.Tracks!.Add(track);
        await DataContext.SaveChangesAsync();
        return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? Redirect(returnUrl)
            : RedirectToAction("Index", "Home");
    }
}
