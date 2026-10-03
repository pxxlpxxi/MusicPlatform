using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MusicPlatform.Models;

namespace MusicPlatform.Data
{
    public interface IMusicPlatformContext
    {
        public DbSet<Album> Albums { get; }
        public DbSet<Artist> Artists { get; }
        public DbSet<Song> Songs { get; }
        public DbSet<AlbumSong> AlbumSongs { get; }
        public DbSet<SongArtist> SongArtists { get; }
        public DbSet<Media> Media { get; }
        public DbSet<MediaType> MediaTypes { get; }
        public DbSet<User> Users { get; }
        public DbSet<AuthenticatedUser> AuthenticatedUsers { get; }
        public int SaveChanges();
        public DatabaseFacade Database { get; }
    }
}
