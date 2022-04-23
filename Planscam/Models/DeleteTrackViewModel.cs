using Planscam.Entities;

namespace Planscam.Models;

public class DeleteTrackViewModel
{
    public Track Track { get; set; } = null!;
    public string? ReturnUrl { get; set; }
}
