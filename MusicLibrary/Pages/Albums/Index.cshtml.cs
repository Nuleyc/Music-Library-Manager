using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using MusicLibrary.Data;
using MusicLibrary.Models;

namespace MusicLibrary.Pages.Albums;

    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Album> Albums { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? ArtistId { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<Album> query = _context.Albums.Include(a => a.Artist) .OrderBy(a => a.Title);

            if (ArtistId.HasValue)
            {
                query = query.Where(a => a.ArtistId == ArtistId.Value);
                ViewData["Title"] = "Albums by Artist";
            }
            else
            {
                ViewData["Title"] = "All Albums";
            }

            Albums = await query.ToListAsync();
        }
    }
