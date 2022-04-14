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

    /// <summary>
    /// Инициализируется при вызове метода <see cref="GetCurrentUser"/>
    /// </summary>
    protected User? CurrentUser { get; private set; }
    protected async Task<User> GetCurrentUser(params Expression<Func<User, object>>[] includeExpressions) => 
        CurrentUser ??= await DataContext.GetCurrentUserAsync(UserManager, User, includeExpressions);

    protected PsmControllerBase(AppDbContext dataContext, UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        DataContext = dataContext;
        UserManager = userManager;
        SignInManager = signInManager;
    }
}
