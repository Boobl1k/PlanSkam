using Planscam.Entities;

namespace Planscam.Models;

public class TrackViewModel
{
    public Track Track { get; set; } = null!;
    public List<Playlist> NotAddedPlaylists { get; set; } = null!;
}
