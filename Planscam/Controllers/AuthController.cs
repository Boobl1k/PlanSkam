using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planscam.Entities;
using Planscam.Models;

namespace Planscam.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

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

        var user = new User
        {
            UserName = name,
            Email = email,
            FavouriteTracks = new FavouriteTracks($"{name}'s favorite tracks")
        };
        var result = await _userManager.CreateAsync(user, pass);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View();
        }

        await _signInManager.SignInAsync(user, false);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View();
        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
        if (result.Succeeded)
        {
            Console.WriteLine($"{model.UserName} is authenticated");
            return !string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl)
                ? Redirect(model.ReturnUrl)
                : RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "wrong email or password");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logoff()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
