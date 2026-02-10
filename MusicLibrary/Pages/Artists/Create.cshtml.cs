using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicLibrary.Data;
using MusicLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace MusicLibrary.Pages.Artists;

    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]

        public InputModel Input { get; set; } = new();
    public class InputModel
    {

        [Required(ErrorMessage = "Artist Name is required.")]
        [StringLength(100, ErrorMessage = "Artist Name cannot exceed 100 characters.")]

        public string Name { get; set; } = string.Empty;
    }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var artist = new Artist
            { Name = Input.Name.Trim() };
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();

            TempData["StatusMessage"] = $"Artist '{artist.Name}' created successfully.";
            return RedirectToPage("Index");
        }
    }
