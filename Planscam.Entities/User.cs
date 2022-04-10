using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Planscam.Entities;

public class User : IdentityUser
{
    public Picture? Picture { get; set; }

    [Required]
    public List<Playlist>? Playlists { get; set; }

    [Required]
    public FavouriteTracks? FavouriteTracks { get; set; }
}
