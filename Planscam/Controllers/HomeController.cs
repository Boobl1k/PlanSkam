using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.Models;

namespace Planscam.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _dataContext;
    private readonly UserManager<User> _userManager;

    public HomeController(AppDbContext dataContext, UserManager<User> userManager)
    {
        _dataContext = dataContext;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index() => //TODO view
        View(new HomePageViewModel
        {
            Playlists = _dataContext.Playlists.Include(playlist => playlist.Picture).ToList(),
            User = await _dataContext.Users
                .Include(user => user.Picture)
                .Include(user => user.FavouriteTracks)
                .Include(user => user.FavouriteTracks!.Picture)
                .Include(user => user.FavouriteTracks!.Tracks)
                //.Include(user => user.Playlists)
                .FirstAsync(user => user.Id == _userManager.GetUserId(User))
        });

    //TODO боюсь это трогать, зачем вообще это нужно?
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
}
