using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;

namespace Planscam.Extensions;

public static class UserManagerExtensions
{
    public static async Task<User> GetCurrent<TProperty>(
        this UserManager<User> userManager,
        ClaimsPrincipal currentClaims,
        AppDbContext appDbContext,
        Expression<Func<User, TProperty>> includeExpression) =>
        (await GetUserByIdOrCurrent(userManager, default, currentClaims, appDbContext, includeExpression))!;

    public static async Task<User?> GetUserByIdOrCurrent<TProperty>(
        this UserManager<User> userManager,
        string? id,
        ClaimsPrincipal currentClaims,
        AppDbContext appDbContext,
        Expression<Func<User, TProperty>> includeExpression) =>
        await appDbContext.Users
            .Include(includeExpression)
            .FirstOrDefaultAsync(user => user.Id == (id ?? userManager.GetUserId(currentClaims)));

    public static async Task<User?> GetUserByIdOrCurrent(
        this UserManager<User> userManager,
        string? id,
        ClaimsPrincipal currentClaims) =>
        id is null ? await userManager.GetUserAsync(currentClaims) : await userManager.FindByIdAsync(id);
}
