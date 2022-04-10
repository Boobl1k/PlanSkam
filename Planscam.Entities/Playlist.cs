using System.ComponentModel.DataAnnotations;

namespace Planscam.Entities;

public class Playlist
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public Picture? Picture { get; set; }

    [Required]
    public List<Track>? Tracks { get; set; }

    [Required]
    public List<User>? Users { get; set; }
}
