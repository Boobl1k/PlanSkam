using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;
using Planscam.Models;

namespace Planscam.Controllers;

[Authorize(Roles = "Author")]
public class StudioController : PsmControllerBase
{
    public StudioController(AppDbContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager)
        : base(dataContext, userManager, signInManager)
    {
    }

    [HttpGet]
    public IActionResult Index() =>
        View();

    [HttpGet]
    public async Task<IActionResult> MyTracks() =>
        View(await DataContext.Tracks
            .Where(track =>
                track.Author == DataContext.Authors.First(author => author.User == CurrentUserQueryable.First()))
            .ToListAsync());

    [HttpGet]
    public IActionResult LoadNewTrack() =>
        View(new LoadTrackViewModel
        {
            Genres = DataContext.Genres.ToList()
        });

    [HttpPost]
    public async Task<IActionResult> LoadNewTrack(LoadTrackViewModel model)
    {
        model.Genres = DataContext.Genres.ToList();
        if (!ModelState.IsValid) return View(model);
        var data = new TrackData();
        using (var reader = new BinaryReader(model.Track!.OpenReadStream()))
        {
            data.Data = reader.ReadBytes((int) model.Track.Length);
        }

        var track = new Track
        {
            Name = model.Name!,
            Data = data,
            Author = await DataContext.Authors.FirstAsync(author => author.User == CurrentUserQueryable.First()),
            Genre = await DataContext.Genres.FirstAsync(genre => genre.Id == model.GenreId)
        };
        if (model.Image is { })
        {
            track.Picture = new Picture();
            using var reader = new BinaryReader(model.Image.OpenReadStream());
            track.Picture.Data = reader.ReadBytes((int) model.Image.Length);
        }

        await DataContext.Tracks.AddAsync(track);
        await DataContext.SaveChangesAsync();
        return View(model);
    }

    public async Task<Track?> GetOwnTrackById(int trackId, bool includePic = false) =>
        await (includePic
                ? DataContext.Tracks.Include(track => track.Picture)
                : DataContext.Tracks as IQueryable<Track>)
            .FirstOrDefaultAsync(track =>
                track.Id == trackId &&
                track.Author == DataContext.Authors.First(author => author.User == CurrentUserQueryable.First()));

    [HttpGet]
    public async Task<IActionResult> DeleteTrack(int trackId, string? returnUrl) =>
        await GetOwnTrackById(trackId, true) switch
        {
            { } track => View(new DeleteTrackViewModel
            {
                Track = track,
                ReturnUrl = returnUrl
            }),
            _ => NotFound()
        };

    [HttpPost]
    public async Task<IActionResult> DeleteTrackSure(int trackId, string? returnUrl)
    {
        var track = await GetOwnTrackById(trackId);
        if (track is null) return BadRequest();
        DataContext.Tracks.Remove(track);
        await DataContext.SaveChangesAsync();
        return IsLocalUrl(returnUrl) ? Redirect();
    }
}
