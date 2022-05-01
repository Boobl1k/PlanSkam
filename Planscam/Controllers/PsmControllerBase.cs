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

    private User? _currentUser;

    /// <summary>
    /// при первом получении долбит бд
    /// </summary>
    protected User CurrentUser
    {
        get => _currentUser ??= UserManager.GetUserAsync(User).Result;
        set => _currentUser = value;
    }

    private string? _currentUserId;

    /// <summary>
    /// работает быстро
    /// </summary>
    protected string CurrentUserId =>
        _currentUserId ??= UserManager.GetUserId(User);

    private IQueryable<User>? _currentUserQueryable;

    protected IQueryable<User> CurrentUserQueryable =>
        _currentUserQueryable ??= DataContext.Users.Where(user => user.Id == CurrentUserId);

    protected PsmControllerBase(AppDbContext dataContext, UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        DataContext = dataContext;
        UserManager = userManager;
        SignInManager = signInManager;
    }

    protected bool IsLocalUrl(string? url) =>
        !string.IsNullOrEmpty(url) && Url.IsLocalUrl(url);

    //выглядит как говнокод, но так будет только 1 запрос к базе
    private Expression<Func<Playlist, Playlist>>? _playlistSetIsLikedAndIsOwnedExpression;

    protected Expression<Func<Playlist, Playlist>> PlaylistSetIsLikedAndIsOwnedExpression =>
        _playlistSetIsLikedAndIsOwnedExpression ??= playlist => new Playlist
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Picture = playlist.Picture,
            Tracks = playlist.Tracks,
            Users = playlist.Users,
            OwnedBy = playlist.OwnedBy,
            IsAlbum = playlist.IsAlbum,
            IsLiked = SignInManager.IsSignedIn(User)
                ? playlist.Users!.Any(user => user.Id == CurrentUserId)
                : null,
            IsOwned = SignInManager.IsSignedIn(User)
                ? CurrentUserQueryable
                    .Select(user => user.OwnedPlaylists!.Playlists!)
                    .Any(playlists => playlists.Any(playlist1 => playlist1 == playlist))
                : null
        };

    private Expression<Func<Track, Track>>? _trackSetIsLikedExpression;

    protected Expression<Func<Track, Track>> TrackSetIsLikedExpression =>
        _trackSetIsLikedExpression ??= track => new Track
        {
            Id = track.Id,
            Name = track.Name,
            Data = track.Data,
            Picture = track.Picture,
            Author = track.Author,
            Playlists = track.Playlists,
            Genre = track.Genre,
            IsLiked = SignInManager.IsSignedIn(User)
                ? DataContext.Users
                    .Any(user => user.Id == CurrentUserId && user.FavouriteTracks!.Tracks!.Contains(track))
                : null
        };

    [NonAction]
    protected new IActionResult Unauthorized() =>
        RedirectToAction("Login", "Auth", new {returnUrl = HttpContext.GetCurrentUrl()});
}
