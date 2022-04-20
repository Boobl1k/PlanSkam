using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Planscam.DataAccess;
using Planscam.Entities;

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
    protected Expression<Func<Playlist, Playlist>> PlaylistSetIsLikedExpression =>
        playlist => new Playlist
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Picture = playlist.Picture,
            Tracks = playlist.Tracks,
            Users = playlist.Users,
            IsLiked = SignInManager.IsSignedIn(User) ? playlist.Users!.Any(user => user.Id == CurrentUserId) : null
        };

    protected Expression<Func<Track, Track>> TrackSetIsLikedExpression =>
        track => new Track
        {
            Id = track.Id,
            Name = track.Name,
            Data = track.Data,
            Time = track.Time,
            Picture = track.Picture,
            Author = track.Author,
            Playlists = track.Playlists,
            Genre = track.Genre,
            IsLiked = SignInManager.IsSignedIn(User)
                ? DataContext.Users
                    .Any(user => user.Id == CurrentUserId && user.FavouriteTracks!.Tracks!.Contains(track))
                : null
        };
}
