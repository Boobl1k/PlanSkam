using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.Extensions;
using Planscam.Models;

namespace Planscam.Controllers;

public class ProfileController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _dataContext;

    public ProfileController(UserManager<User> userManager, AppDbContext dataContext)
    {
        _userManager = userManager;
        _dataContext = dataContext;
    }

    private static UserViewModel GetModel(User user) =>
        new() {Id = user.Id, Name = user.UserName, Email = user.Email, Picture = user.Picture};

    [HttpGet]
    public async Task<IActionResult> Index(string? id)
    {
        var user = await _userManager.GetUserByIdOrCurrent(id, User, _dataContext, user1 => user1.Picture);
        return user is null
            ? NotFound()
            : View(GetModel(user));
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> Edit() =>
        View(GetModel(await _userManager.GetCurrent(User, _dataContext, user => user.Picture)));

    [HttpPost, Authorize]
    public async Task<IActionResult> Edit(UserViewModel model, IFormFile? uploadImage)
    {
        var user = await _userManager.GetCurrent(User, _dataContext, user1 => user1.Picture);
        if (uploadImage is { })
            user.Picture = uploadImage.ToPicture();
        user.UserName = model.Name;
        await _userManager.UpdateAsync(user);
        model.Picture = user.Picture;
        return View(model);
    }
}
