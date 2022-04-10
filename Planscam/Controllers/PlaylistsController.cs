using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.Models;

namespace Planscam.Controllers;

public class PlaylistsController : Controller
{
    private readonly AppDbContext _dataContext;
    private readonly UserManager<User> _userManager;

    public PlaylistsController(AppDbContext dataContext, UserManager<User> userManager)
    {
        _dataContext = dataContext;
        _userManager = userManager;
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
        View(GetModel(await _dataContext.Users
            .Include(user => user.FavouriteTracks!.Tracks)
            .Include(user => user.FavouriteTracks!.Picture)
            .FirstAsync(user => user.Id == _userManager.GetUserId(User))));
}
