using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.FsServices;
using Planscam.MobileApi.Models;

namespace Planscam.MobileApi.Controllers;

public class AuthController : PsmControllerBase
{
    private readonly UsersRepo _usersRepo;

    public AuthController(AppDbContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager,
        UsersRepo usersRepo) :
        base(dataContext, userManager, signInManager) =>
        _usersRepo = usersRepo;

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest();
        var (name, email, pass) = model;
        var user = _usersRepo.CreateNewUser(name, email);
        var result = await UserManager.CreateAsync(user, pass);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return BadRequest();
        }

        await SignInManager.SignInAsync(user, false);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return BadRequest();
        if ((await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false))
            .Succeeded)
            return Ok();
        ModelState.AddModelError(string.Empty, "wrong email or password");
        return BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> Logoff()
    {
        await SignInManager.SignOutAsync();
        return Ok();
    }
}
