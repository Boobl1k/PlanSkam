using System.ComponentModel.DataAnnotations;
using Planscam.Entities;

namespace Planscam.Models;

public class TrackSearchViewModel
{
    [Required] public string Query { get; set; } = null!;
    public bool ByAuthors { get; set; }
    public int Page { get; set; }

    public Playlist? Result { get; set; }
}
