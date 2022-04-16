using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;

namespace Planscam.Extensions;

//TODO это говнокод, необходимо снести к херам
public static class UserExtensions
{
    public static async Task<User> GetCurrentUserAsync(
        this AppDbContext appDbContext,
        UserManager<User> userManager,
        ClaimsPrincipal currentClaims,
        params Expression<Func<User, object>>[] includeExpressions) =>
        await appDbContext.GetUserByIdOrCurrentAsync(default, currentClaims, userManager, includeExpressions);

    public static async Task<User> GetUserByIdAsync(
        this AppDbContext appDbContext,
        string? id,
        params Expression<Func<User, object>>[] includeExpressions) =>
        await appDbContext.GetUserByIdOrCurrentAsync(id, default, default, includeExpressions);

    private static async Task<User> GetUserByIdOrCurrentAsync(
        this AppDbContext appDbContext,
        string? id,
        ClaimsPrincipal? currentClaims,
        UserManager<User>? userManager,
        params Expression<Func<User, object>>[] includeExpressions) =>
        await includeExpressions
            .Aggregate(appDbContext.Users as IQueryable<User>, (current, includeExpression) =>
                current.Include(includeExpression))
            .FirstAsync(user => user.Id == (id ?? userManager!.GetUserId(currentClaims)));

    public static async Task<User?> GetUserByIdOrCurrentAsync(
        this UserManager<User> userManager,
        string? id,
        ClaimsPrincipal currentClaims) =>
        id is null ? await userManager.GetUserAsync(currentClaims) : await userManager.FindByIdAsync(id);
}
