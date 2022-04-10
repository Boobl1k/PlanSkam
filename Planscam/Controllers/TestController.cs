using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planscam.DataAccess;
using Planscam.Entities;

namespace Planscam.Controllers;

public class TestController : Controller
{
    private readonly AppDbContext _dataContext;
    private readonly UserManager<User> _userManager;

    public TestController(AppDbContext dataContext, UserManager<User> userManager)
    {
        _dataContext = dataContext;
        _userManager = userManager;
    }

    public async Task<IActionResult> AddTestEntities()
    {
        if (_dataContext.Authors.Any())
            return Ok();
        var authors = new List<Author>();
        for (var i = 0; i < 5; i++)
            authors.Add(new Author {Name = $"Author{i + 1}"});
        var tracks = new List<Track>();
        for (var i = 0; i < 10; i++)
            tracks.Add(new Track
            {
                Name = $"track{i + 1}",
                Data = Array.Empty<byte>(),
                Time = new TimeSpan(0, 1, 20),
                Author = authors[i / 2]
            });
        await _dataContext.Authors.AddRangeAsync(authors);
        await _dataContext.SaveChangesAsync();
        await _dataContext.Tracks.AddRangeAsync(tracks);
        await _dataContext.SaveChangesAsync();
        await _userManager.CreateAsync(new User
        {
            UserName = "qwe",
            Email = "qwe@qwe.qwe",
            FavouriteTracks = new FavouriteTracks("qwe's favorite tracks")
            {
                Tracks = tracks.GetRange(0, 6)
            }
        }, "qweQWE123!");
        await _dataContext.Playlists.AddAsync(new Playlist
        {
            Name = "test playlist",
            Tracks = tracks.GetRange(4, 3)
        });
        await _dataContext.SaveChangesAsync();
        return Ok();
    }
}
