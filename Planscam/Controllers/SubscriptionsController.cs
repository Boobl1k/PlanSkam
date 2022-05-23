using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;

namespace Planscam.Controllers;

[Authorize]
public class SubscriptionsController : PsmControllerBase
{
    public SubscriptionsController(AppDbContext dataContext, UserManager<User> userManager,
        SignInManager<User> signInManager) : base(dataContext, userManager, signInManager)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index() =>
        View(await DataContext.Subscriptions.ToListAsync());

    [HttpPost]
    public async Task<IActionResult> BuySub(int subId)
    {
        var sub = await DataContext.Subscriptions.FindAsync(subId);
        if (sub is null) return BadRequest();
        await UserManager.AddToRoleAsync(CurrentUser, "Sub");
        CurrentUser.SubExpires = DateTime.Now + Subscription.SubscriptionDurationToTimeSpan(sub.Duration);
        await DataContext.SaveChangesAsync();
        return Ok();
    }
}
