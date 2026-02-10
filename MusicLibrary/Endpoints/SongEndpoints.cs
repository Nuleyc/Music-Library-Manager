using Microsoft.EntityFrameworkCore;
using MusicLibrary.Data;
using MusicLibrary.Models;

public static class SongEndpoints
{
    public static void MapSongEndpoints(this WebApplication app)
    {
        app.MapGet("/api/songs", async (AppDbContext db) =>
        {
            var songs = await db.Songs
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .OrderBy(s => s.Title)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.ReleaseYear,
                    s.Genre,
                    Artist = new { s.Artist!.Id, s.Artist.Name },
                    Album = new { s.Album!.Id, s.Album.Title }
                })
                .ToListAsync();

            return Results.Ok(songs);
        });

        app.MapGet("/api/songs/{id:int}", async (int id, AppDbContext db) =>
        {
            var song = await db.Songs
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.ReleaseYear,
                    s.Genre,
                    Artist = new { s.Artist!.Id, s.Artist.Name },
                    Album = new { s.Album!.Id, s.Album.Title }
                })
                .FirstOrDefaultAsync();

            return song is not null ? Results.Ok(song) : Results.NotFound();
        });

        app.MapPost("/api/songs", async (Song song, AppDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(song.Title) || string.IsNullOrWhiteSpace(song.Genre) || song.ArtistId <= 0 || song.AlbumId <= 0)
            {
                return Results.BadRequest("Title, Genre, ArtistId, and AlbumId are required.");
            }

            if (song.ReleaseYear < 1900 || song.ReleaseYear > 2100)
            {
                return Results.BadRequest("Release Year must be between 1900 and 2100.");
            }

            var artistExists = await db.Artists.AnyAsync(a => a.Id == song.ArtistId);
            var albumExists = await db.Albums.AnyAsync(a => a.Id == song.AlbumId);

            if (!artistExists || !albumExists)
            {
                return Results.BadRequest("Invalid ArtistId or AlbumId.");
            }

            song.Title = song.Title.Trim();
            song.Genre = song.Genre.Trim();

            db.Songs.Add(song);
            await db.SaveChangesAsync();
            var createdSong = await db.Songs
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .Where(s => s.Id == song.Id)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.ReleaseYear,
                    s.Genre,
                    Artist = new { s.Artist!.Id, s.Artist.Name },
                    Album = new { s.Album!.Id, s.Album.Title }
                })
                .FirstAsync();

            return Results.Created($"/api/songs/{createdSong.Id}", createdSong);
        });

        app.MapPut("/api/songs/{id:int}", async (int id, Song updatedSong, AppDbContext db) =>
        {
            if (id != updatedSong.Id)
            return Results.BadRequest("ID mismatch.");

            if (string.IsNullOrWhiteSpace(updatedSong.Title) || string.IsNullOrWhiteSpace(updatedSong.Genre) || updatedSong.ArtistId <= 0 || updatedSong.AlbumId <= 0)
            {
                return Results.BadRequest("Title, Genre, ArtistId, and AlbumId are required.");
            }

            if (updatedSong.ReleaseYear < 1900 || updatedSong.ReleaseYear > 2100)
            {
                return Results.BadRequest("Release Year must be between 1900 and 2100.");
            }

            var song = await db.Songs.FindAsync(id);
            if (song is null) return Results.NotFound();

            var artistExists = await db.Artists.AnyAsync(a => a.Id == updatedSong.ArtistId);
            var albumExists = await db.Albums.AnyAsync(a => a.Id == updatedSong.AlbumId);

            if (!artistExists || !albumExists)
            {
                return Results.BadRequest("ArtistId or AlbumId does not exist.");
        }
        
            song.Title = updatedSong.Title.Trim();
            song.ReleaseYear = updatedSong.ReleaseYear;
            song.Genre = updatedSong.Genre.Trim();
            song.ArtistId = updatedSong.ArtistId;
            song.AlbumId = updatedSong.AlbumId;

            await db.SaveChangesAsync();
            var resultSong = await db.Songs
                .Include(s => s.Artist)
                .Include(s => s.Album)
                .Where(s => s.Id == song.Id)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.ReleaseYear,
                    s.Genre,
                    Artist = new { s.Artist!.Id, s.Artist.Name },
                    Album = new { s.Album!.Id, s.Album.Title }
                })
                .FirstAsync();

            return Results.Ok(resultSong);
        });

        app.MapDelete("/api/songs/{id:int}", async (int id, AppDbContext db) =>
        {
            var song = await db.Songs.FindAsync(id);
            if (song is null) return Results.NotFound();

            db.Songs.Remove(song);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}