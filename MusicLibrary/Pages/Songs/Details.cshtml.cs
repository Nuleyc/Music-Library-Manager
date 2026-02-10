using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicLibrary.Data;
using MusicLibrary.Models;

namespace MusicLibrary.Pages.Songs;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _context;

    public DetailsModel(AppDbContext context)
    {
        _context = context;
    }

    public Song? Song { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Song = await _context.Songs
            .Include(s => s.Album)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (Song == null) return NotFound();
        return Page();
    }
}