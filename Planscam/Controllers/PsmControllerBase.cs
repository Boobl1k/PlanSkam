using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planscam.DataAccess;
using Planscam.Entities;

namespace Planscam.Controllers;

public abstract class PsmControllerBase : Controller
{
    protected readonly AppDbContext DataContext;
    protected readonly UserManager<User> UserManager;
    protected readonly SignInManager<User> SignInManager;

    protected PsmControllerBase(AppDbContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        DataContext = dataContext;
        UserManager = userManager;
        SignInManager = signInManager;
    }
}
