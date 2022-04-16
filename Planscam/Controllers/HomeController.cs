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

    public async Task<IActionResult> Index() =>
        View(new HomePageViewModel
        {
            Playlists = DataContext.Playlists.Include(playlist => playlist.Picture).AsNoTracking().ToList(),
            User = SignInManager.IsSignedIn(User)
                ? await DataContext.Users
                    .Include(user => user.Picture)
                    .Include(user => user.FavouriteTracks)
                    .Include(user => user.FavouriteTracks!.Picture)
                    .Include(user => user.FavouriteTracks!.Tracks)!
                    .ThenInclude(track => track.Picture)
                    .AsNoTracking()
                    .FirstAsync(user => user.Id == UserManager.GetUserId(User))
                : null
        });

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
}
