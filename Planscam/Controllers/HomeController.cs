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

    public async Task<IActionResult> Index()
    {
        var userId = UserManager.GetUserId(User)!;
        await DataContext.Pictures
            .Where(picture =>
                DataContext.Users
                    .First(user => user.Id == userId)
                    .FavouriteTracks!.Tracks!
                    .Any(track => track.Picture == picture))
            .LoadAsync();
        return View(new HomePageViewModel
        {
            Playlists = DataContext.Playlists.Include(playlist => playlist.Picture).ToList(),
            User = SignInManager.IsSignedIn(User)
                ? await DataContext.Users
                    .Include(user => user.Picture)
                    .Include(user => user.FavouriteTracks)
                    .Include(user => user.FavouriteTracks!.Picture)
                    .Include(user => user.FavouriteTracks!.Tracks)
                    //.Include(user => user.Playlists) //нинада потому что выше мы уже получили все плейлисты
                    .FirstAsync(user => user.Id == userId)
                : null
        });
    }

    //TODO боюсь это трогать, зачем вообще это нужно?
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
}
