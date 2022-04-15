using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.Extensions;
using Planscam.Models;

namespace Planscam.Controllers;

public class ProfileController : PsmControllerBase
{
    public ProfileController(AppDbContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager)
        : base(dataContext, userManager, signInManager)
    {
    }

    private static UserViewModel GetModel(User user) =>
        new() {Id = user.Id, Name = user.UserName, Email = user.Email, Picture = user.Picture};

    [HttpGet]
    public async Task<IActionResult> Index(string? id) =>
        View(GetModel(id is { }
            ? await DataContext.GetUserByIdAsync(id, user => user.Picture!)
            : await DataContext.GetCurrentUserAsync(UserManager, User, user => user.Picture!)));

    [HttpGet, Authorize]
    public async Task<IActionResult> Edit() =>
        View(GetModel(await DataContext.GetCurrentUserAsync(UserManager, User, user => user.Picture!)));

    [HttpPost, Authorize]
    public async Task<IActionResult> Edit(UserViewModel model, IFormFile? uploadImage)
    {
        var user = uploadImage is { }
            ? await DataContext.GetCurrentUserAsync(UserManager, User)
            : await DataContext.GetCurrentUserAsync(UserManager, User, user => user.Picture!);
        if (uploadImage is { })
            user.Picture = uploadImage.ToPicture();
        user.UserName = model.Name;
        await UserManager.UpdateAsync(user);
        model.Picture = user.Picture;
        return View(model);
    }
}
