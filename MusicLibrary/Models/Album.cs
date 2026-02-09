using System.ComponentModel.DataAnnotations;

namespace MusicLibrary.Models
{
    public class Album
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Album Title is required.")]
        [StringLength(100, ErrorMessage = "Album Title cannot be longer than 100 characters.")]
        public string Title { get; set; } = "";
        [Required(ErrorMessage = "Release Year is required.")]
        [Range(1900, 2100, ErrorMessage = "Release Year must be between 1900 and 2100.")]
        public int ReleaseYear { get; set; }

[Required]
        public int ArtistId { get; set; }
        public Artist Artist { get; set; } = new();
        public List<Song> Songs { get; set; } = new();
    }
}