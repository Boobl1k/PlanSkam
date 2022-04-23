using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planscam.DataAccess;
using Planscam.Entities;

namespace Planscam.Controllers;

public class TestController : PsmControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public TestController(AppDbContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager) :
        base(dataContext, userManager, signInManager)
    {
        _roleManager = roleManager;
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
        var user = new User
        {
            UserName = "qwe",
            Email = "qwe@qwe.qwe",
            FavouriteTracks = new FavouriteTracks("qwe's favorite tracks")
            {
                Tracks = new List<Track>()
            }
        };
        await UserManager.CreateAsync(user, "qweQWE123!");
        var author = new Author {Name = "Author1", User = user};
        var tracks = new List<Track>();
        for (var i = 0; i < 10; i++)
            tracks.Add(new Track
            {
                Name = $"track{i + 1}",
                Data = new TrackData
                {
                    Data = Array.Empty<byte>()
                },
                Author = author,
                Genre = genre
            });
        await DataContext.Pictures.AddAsync(genrePic);
        await DataContext.SaveChangesAsync();
        await DataContext.Genres.AddAsync(genre);
        await DataContext.SaveChangesAsync();
        await DataContext.Authors.AddAsync(author);
        await DataContext.SaveChangesAsync();
        await DataContext.Tracks.AddRangeAsync(tracks);
        await DataContext.SaveChangesAsync();
        user.FavouriteTracks.Tracks!.AddRange(tracks.GetRange(0, 6));
        await DataContext.SaveChangesAsync();
        await UserManager.AddToRoleAsync(user, "Author");
        await DataContext.Playlists.AddAsync(new Playlist
        {
            Name = "test playlist",
            Tracks = tracks.GetRange(4, 3)
        });
        await DataContext.SaveChangesAsync();
        return Ok();
    }
}
