using Planscam.Entities;

namespace Planscam.Models;

public class PlaylistsViewModel
{
    public string Name { get; set; } = null!;
    public Picture? Picture { get; set; } = null!;
    public List<Track> Tracks { get; set; } = null!;
}
