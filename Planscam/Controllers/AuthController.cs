using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.Models;

namespace Planscam.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly AppDbContext _dataContext;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext dataContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dataContext = dataContext;
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
        var user = new User
        {
            UserName = model.Email,
            Email = model.Email,
        };
        var result = await _userManager.CreateAsync(user, model.Password);
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
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        if (result.Succeeded)
        {
            Console.WriteLine($"{model.Email} is authenticated");
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
        Console.WriteLine("aaa\n\n\n\n\n");
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
