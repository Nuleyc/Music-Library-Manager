using System.ComponentModel.DataAnnotations;

namespace MusicLibrary.Models
{
    public class Song
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Song Title is required.")]
        [StringLength(100, ErrorMessage = "Song Title cannot be longer than 100 characters.")]
        public string Title { get; set; } = "";
    
        [Required(ErrorMessage = "Release Year is required.")]
        [Range(1900, 2100, ErrorMessage = "Release Year must be between 1900 and 2100.")]
        public int ReleaseYear { get; set; }

        [Required(ErrorMessage = "Genre is required.")]
        [StringLength(50, ErrorMessage = "Genre cannot be longer than 50 characters.")]
        public string Genre { get; set; } = "";

        [Required]
        public int ArtistId { get; set; }

        [Required]
        public int AlbumId { get; set; }

        public Artist Artist { get; set; } = new();
        public Album Album { get; set; } = new();
    }
}