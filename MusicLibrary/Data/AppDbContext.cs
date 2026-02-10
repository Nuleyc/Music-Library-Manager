using Microsoft.EntityFrameworkCore;
using MusicLibrary.Models;

namespace MusicLibrary.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<Artist> Artists => Set<Artist>();
        public DbSet<Album> Albums => Set<Album>();
        public DbSet<Song> Songs => Set<Song>();

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);

        //     // Configure relationships and constraints if necessary
        //      modelBuilder.Entity<Artist>().HasData(
        //          new Artist { Id = 1, Name = "The Beatles" },
        //          new Artist { Id = 2, Name = "Led Zeppelin" },
        //          new Artist { Id = 3, Name = "Pink Floyd" }
        //      );
        //      modelBuilder.Entity<Album>().HasData(
        //          new Album { Id = 1, Title = "Abbey Road", ReleaseYear = 1969, ArtistId = 1 },
        //          new Album { Id = 2, Title = "Led Zeppelin IV", ReleaseYear = 1971, ArtistId = 2 },
        //          new Album { Id = 3, Title = "The Dark Side of the Moon", ReleaseYear = 1973, ArtistId = 3 }
        //      );
        //                  modelBuilder.Entity<Album>().HasOne(a => a.Artist)
        //          .WithMany(ar => ar.Albums)
        //          .HasForeignKey(a => a.ArtistId)
        //          .OnDelete(DeleteBehavior.Restrict);

        //          modelBuilder.Entity<Song>().HasOne(s => s.Album)
        //          .WithMany(a => a.Songs)
        //          .HasForeignKey(s => s.AlbumId)
        //          .OnDelete(DeleteBehavior.Restrict);

        // }
    }
}