using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MusicPlatform.Models;

namespace MusicPlatform.Data
{
    internal interface IMusicPlatformContext
    {
        DbSet<Album> Albums { get; }
        DbSet<Artist> Artists { get; }
        DbSet<Song> Songs { get; }
        DbSet<AlbumSong> AlbumSongs { get; }
        DbSet<SongArtist> SongArtists { get; }
        DbSet<Media> Media { get; }
        DbSet<MediaType> MediaTypes { get; }
        DbSet<User> Users { get; }

        int SaveChanges();
        DatabaseFacade Database { get; }
    }
}
