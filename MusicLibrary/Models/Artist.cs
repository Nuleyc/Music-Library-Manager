using System.ComponentModel.DataAnnotations;


namespace MusicLibrary.Models
{
    public class Artist
    {
public int Id { get; set; }
[Required(ErrorMessage = "Artist Name is required.")]
[StringLength(100, ErrorMessage = "Artist Name cannot be longer than 100 characters.")]
public string Name { get; set; } = "";

public List<Album> Albums { get; set; } = new();
public List<Song> Songs { get; set; } = new();
    }
}