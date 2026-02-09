using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicLibrary.Data;
using MusicLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace MusicLibrary.Pages.Albums;

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

        public class InputModel
        {
            [Required(ErrorMessage = "Album Title is required.")]
            [StringLength(100, ErrorMessage = "Album Title cannot exceed 100 characters.")]
            public string Title { get; set; } = "";

            [Required(ErrorMessage = "Release Year is required.")]
            [Range(1900, 2100, ErrorMessage = "Release Year must be between 1900 and 2100.")]
            public int ReleaseYear { get; set; }

            [Required(ErrorMessage = "Artist is required.")]
            public int ArtistId { get; set; }
        }

        public async Task OnGetAsync()
        {
            Artists = await _context.Artists.OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // Reload artists for the dropdown
                return Page();
            }

            var album = new Album
            {
                Title = Input.Title.Trim(),
                ReleaseYear = Input.ReleaseYear,
                ArtistId = Input.ArtistId
            };

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"Album '{album.Title}' created successfully.";
            return RedirectToPage("Index");
        }
    }
