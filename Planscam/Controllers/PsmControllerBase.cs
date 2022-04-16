using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.Extensions;

namespace Planscam.Controllers;

public abstract class PsmControllerBase : Controller
{
    protected readonly AppDbContext DataContext;
    protected readonly UserManager<User> UserManager;
    protected readonly SignInManager<User> SignInManager;

    private User? _currentUser;

    /// <summary>
    /// Инициализируется при вызове метода <see cref="GetCurrentUserAsync()"/>
    /// </summary>
    protected User CurrentUser => _currentUser ?? GetCurrentUserAsync().Result;

    protected async Task<User> GetCurrentUserAsync() =>
        _currentUser ??= await UserManager.GetUserAsync(User);

    protected async Task<User> GetCurrentUserAsync(params Expression<Func<User, object>>[] includeExpressions) =>
        _currentUser ??= await DataContext.GetCurrentUserAsync(UserManager, User, includeExpressions);

    protected PsmControllerBase(AppDbContext dataContext, UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        DataContext = dataContext;
        UserManager = userManager;
        SignInManager = signInManager;
    }

    protected bool IsLocalUrl(string? url) =>
        !string.IsNullOrEmpty(url) && Url.IsLocalUrl(url);
}
