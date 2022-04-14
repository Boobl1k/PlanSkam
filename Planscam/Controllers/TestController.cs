using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planscam.DataAccess;
using Planscam.Entities;

namespace Planscam.Controllers;

public class TestController : PsmControllerBase
{
    public TestController(AppDbContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager) :
        base(dataContext, userManager, signInManager)
    {
    }

    public async Task<IActionResult> AddTestEntities()
    {
        if (DataContext.Authors.Any())
            return Ok();
        var genrePic = new Picture
        {
            Data = Array.Empty<byte>()
        };
        var genre = new Genre
        {
            Name = "test genre",
            Picture = genrePic,
        };
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
                Author = authors[i / 2],
                Genre = genre
            });
        await DataContext.Pictures.AddAsync(genrePic);
        await DataContext.SaveChangesAsync();
        await DataContext.Genres.AddAsync(genre);
        await DataContext.SaveChangesAsync();
        await DataContext.Authors.AddRangeAsync(authors);
        await DataContext.SaveChangesAsync();
        await DataContext.Tracks.AddRangeAsync(tracks);
        await DataContext.SaveChangesAsync();
        await UserManager.CreateAsync(new User
        {
            UserName = "qwe",
            Email = "qwe@qwe.qwe",
            FavouriteTracks = new FavouriteTracks("qwe's favorite tracks")
            {
                Tracks = tracks.GetRange(0, 6)
            }
        }, "qweQWE123!");
        await DataContext.Playlists.AddAsync(new Playlist
        {
            Name = "test playlist",
            Tracks = tracks.GetRange(4, 3)
        });
        await DataContext.SaveChangesAsync();
        return Ok();
    }
}
