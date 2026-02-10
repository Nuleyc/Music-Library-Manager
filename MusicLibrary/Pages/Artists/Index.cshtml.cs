using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicLibrary.Data;
using MusicLibrary.Models;

namespace MusicLibrary.Pages.Artists
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Artist> Artists { get; set; } = new();

        public async Task OnGetAsync()
        {
            Artists = await _context.Artists
            .OrderBy(a => a.Name)
            .ToListAsync();
        }
    }
}