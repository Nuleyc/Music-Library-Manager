using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicLibrary.Data;
using MusicLibrary.Models;

namespace MusicLibrary.Pages.Songs;

    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Song> Songs { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? AlbumId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ArtistId { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<Song> query = _context.Songs
                .Include(s => s.Album)
                .ThenInclude(a => a.Artist)
                .OrderBy(s => s.Title);

            if (AlbumId.HasValue)
            {
                query = query.Where(s => s.AlbumId == AlbumId.Value);
                ViewData["Title"] = "Songs in Album";
            }
            else if (ArtistId.HasValue)
            {
                query = query.Where(s => s.Album.ArtistId == ArtistId.Value);
                ViewData["Title"] = "Songs by Artist";
            }
            else
            {
                ViewData["Title"] = "All Songs";
            }

            Songs = await query.ToListAsync();
        }
    }