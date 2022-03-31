using System.ComponentModel.DataAnnotations;

namespace Planscam.Entities;

public class Playlist
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public Picture? Picture { get; set; }

    [Required]
    public IQueryable<Track>? Tracks { get; set; }

    [Required]
    public IQueryable<User>? Users { get; set; }
}
