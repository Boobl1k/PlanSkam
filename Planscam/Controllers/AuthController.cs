using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.FsServices;
using Planscam.Models;

namespace Planscam.Controllers;

public class AuthController : PsmControllerBase
{
    private readonly UsersRepo _usersRepo;

    public AuthController(AppDbContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager,
        UsersRepo usersRepo) :
        base(dataContext, userManager, signInManager) =>
        _usersRepo = usersRepo;

    [HttpGet]
    public IActionResult Register() =>
        View();

    [HttpGet]
    public IActionResult Login(string? returnUrl) =>
        View(new LoginViewModel {ReturnUrl = returnUrl});

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var (name, email, pass) = model;
        var user = _usersRepo.CreateNewUser(name, email);
        var result = await UserManager.CreateAsync(user, pass);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View();
        }

        await SignInManager.SignInAsync(user, false);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View();
        if ((await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false))
            .Succeeded)
            return IsLocalUrl(model.ReturnUrl) ? Redirect(model.ReturnUrl!) : RedirectToAction("Index", "Home");
        ModelState.AddModelError(string.Empty, "wrong email or password");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logoff()
    {
        await SignInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();
}
