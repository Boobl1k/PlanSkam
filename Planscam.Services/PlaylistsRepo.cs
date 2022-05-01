using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Planscam.DataAccess;
using Planscam.Entities;

namespace Planscam.Services;

public class PlaylistsRepo : ServiceBase
{
    public PlaylistsRepo(AppDbContext dataContext, UserManager<User> userManager) : base(dataContext, userManager) { }

    public async Task<List<Playlist>> GetLikedPlaylists(ClaimsPrincipal currentUser) =>
        await DataContext.Users
            .Where(user => user.Id == UserManager.GetUserId(currentUser))
            .Select(user => user.Playlists!)
            .FirstAsync();
}
