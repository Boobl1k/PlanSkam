﻿using System.Diagnostics;
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
        //TODO view
        var playlists = DataContext.Playlists.Include(playlist => playlist.Picture).ToList();
        var user = SignInManager.IsSignedIn(User)
            ? await DataContext.Users
                .Include(user => user.Picture)
                .Include(user => user.FavouriteTracks)
                .Include(user => user.FavouriteTracks!.Picture)
                .Include(user => user.FavouriteTracks!.Tracks)
                //.Include(user => user.Playlists)
                .FirstAsync(user => user.Id == UserManager.GetUserId(User))
            : null;
        return View(new HomePageViewModel
        {
            Playlists = playlists,
            User = user
        });
    }

    //TODO боюсь это трогать, зачем вообще это нужно?
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
}
