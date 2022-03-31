using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Planscam.Entities;

namespace Planscam.DataAccess;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<FavouriteTracks> FavouriteTracks { get; set; } = null!;
    public DbSet<Picture> Pictures { get; set; } = null!;
    public DbSet<Playlist> Playlists { get; set; } = null!;
    public DbSet<Track> Tracks { get; set; } = null!;
}
