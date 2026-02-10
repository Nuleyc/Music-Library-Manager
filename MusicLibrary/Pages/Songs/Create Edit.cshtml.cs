using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicLibrary.Data;
using MusicLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace MusicLibrary.Pages.Songs;

    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<Artist> Artists { get; set; } = new();
        public List<Album> Albums { get; set; } = new();

        [BindProperty]
        public int Id { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Song Title is required.")]
            [StringLength(100, ErrorMessage = "Song Title cannot exceed 100 characters.")]
            public string Title { get; set; } = "";

            [Required(ErrorMessage = "Release Year is required.")]
            [Range(1900, 2100, ErrorMessage = "Release Year must be between 1900 and 2100.")]
            public int ReleaseYear { get; set; }

            [Required(ErrorMessage = "Genre is required.")]
            [StringLength(50, ErrorMessage = "Genre cannot exceed 50 characters.")]
            public string Genre { get; set; } = "";

            [Required(ErrorMessage = "Artist is required.")]
            public int ArtistId { get; set; }

            [Required(ErrorMessage = "Album is required.")]
            public int AlbumId { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Artists = await _context.Artists.OrderBy(a => a.Name).ToListAsync();
            Albums = await _context.Albums.OrderBy(a => a.Title).ToListAsync();

            var song = await _context.Songs.FindAsync(id);
            if (song == null) return NotFound();

            Id = song.Id;
            Input = new InputModel
            {
                Title = song.Title,
                ReleaseYear = song.ReleaseYear,
                Genre = song.Genre,
                ArtistId = song.ArtistId,
                AlbumId = song.AlbumId
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Artists = await _context.Artists.OrderBy(a => a.Name).ToListAsync();
            Albums = await _context.Albums.OrderBy(a => a.Title).ToListAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var song = await _context.Songs.FindAsync(Id);
            if (song == null) return NotFound();

            song.Title = Input.Title.Trim();
            song.ReleaseYear = Input.ReleaseYear;
            song.Genre = Input.Genre.Trim();
            song.ArtistId = Input.ArtistId;
            song.AlbumId = Input.AlbumId;

            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"Song '{song.Title}' updated successfully.";
            return RedirectToPage("Details", new { id = song.Id });
        }
    }           