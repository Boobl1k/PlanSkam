using System.ComponentModel.DataAnnotations;

namespace Planscam.Entities;

public class Track
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public byte[] Data { get; set; } = null!;
    
    [Required]
    public TimeSpan Time { get; set; }
    
    public Picture? Picture { get; set; }
    
    [Required]
    public Author? Author { get; set; }
    
    [Required]
    public List<Playlist>? Playlists { get; set; }
    
    [Required]
    public Genre? Genre { get; set; }
}
