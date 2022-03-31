using System.ComponentModel.DataAnnotations;

namespace Planscam.Entities;

public class Track
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public byte[]? Data { get; set; }
    
    [Required]
    public DateTime Time { get; set; }
    
    public Picture? Picture { get; set; }
    
    [Required]
    public Author? Author { get; set; }
    
    [Required]
    public IQueryable<Playlist>? Playlists { get; set; }
}
