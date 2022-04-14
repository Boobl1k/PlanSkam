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

    private static PlaylistsViewModel GetModel(User user) =>
        user.FavouriteTracks is {Tracks: { }}
            ? new PlaylistsViewModel
            {
                Name = user.FavouriteTracks.Name,
                Picture = user.FavouriteTracks.Picture,
                Tracks = user.FavouriteTracks.Tracks
            }
            : throw new Exception();

    [HttpGet, Route(nameof(FavoriteTracks)), Authorize]
    public async Task<IActionResult> FavoriteTracks() =>
        View(GetModel(await DataContext.Users
            .Include(user => user.FavouriteTracks!.Tracks)
            .Include(user => user.FavouriteTracks!.Picture)
            .FirstAsync(user => user.Id == UserManager.GetUserId(User))));
}
