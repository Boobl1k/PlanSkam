using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.MobileApi.Models;

namespace Planscam.MobileApi.Controllers;

public class AuthorsController : PsmControllerBase
{
    public AuthorsController(AppDbContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager)
        : base(dataContext, userManager, signInManager)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index(int id) =>
        await DataContext.Authors
                .Select(a => new AuthorPageViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Picture = a.Picture!,
                    Albums = a.User!.Playlists!.Where(p => p.IsAlbum).ToList(),
                    RecentReleases = new Playlist
                    {
                        Name = $"{a.Name}'s last releases",
                        Tracks = a.Tracks!.OrderByDescending(track => track.Id).Take(20).ToList()
                    }
                })
                .FirstOrDefaultAsync(a => a.Id == id) switch
            {
                { } author => Json(author),
                _ => NotFound()
            };

    [HttpGet]
    public async Task<IActionResult> Search(AuthorsSearchViewModel? model)
    {
        if (model is null || !ModelState.IsValid) return BadRequest();
        model.Result = await DataContext.Authors
            .Where(author => author.Name.Contains(model.Query))
            .Skip(10 * (model.Page - 1))
            .Take(10)
            .Include(author => author.Picture)
            .ToListAsync();
        return Json(model);
    }
}
