using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicLibrary.Data;
using MusicLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace MusicLibrary.Pages.Songs;

    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<Artist> Artists { get; set; } = new();
        public List<Album> Albums { get; set; } = new();

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

        public async Task OnGetAsync()
        {
            Artists = await _context.Artists.OrderBy(a => a.Name).ToListAsync();
            Albums = await _context.Albums.OrderBy(a => a.Title).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
    {
        Artists = await _context.Artists.OrderBy(a => a.Name).ToListAsync();
        Albums = await _context.Albums.OrderBy(a => a.Title).ToListAsync();

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var song = new Song
        {
            Title = Input.Title.Trim(),
            ReleaseYear = Input.ReleaseYear,
            Genre = Input.Genre.Trim(),
            ArtistId = Input.ArtistId,
            AlbumId = Input.AlbumId
        };

        _context.Songs.Add(song);
        await _context.SaveChangesAsync();

        TempData["StatusMessage"] = $"Song '{song.Title}' created successfully.";
        return RedirectToPage("Details", new { id = song.Id });
    }
    }