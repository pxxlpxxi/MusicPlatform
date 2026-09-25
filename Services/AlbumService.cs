using MusicPlatform.Data;
using MusicPlatform.Logging;
using MusicPlatform.Models;

namespace MusicPlatform.Services
{
    internal class AlbumService
    {
        private readonly IMusicPlatformContext _context;

        internal AlbumService(IMusicPlatformContext context)
        {
            _context = context;
        }

        internal Album CreateAlbum(string title, DateOnly? releaseDate)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(
                    "Album title is required.");
            }

            string normalizedTitle = title.Trim();

            Album? existingAlbum = _context.Albums
                .FirstOrDefault(a => 
                a.Title == normalizedTitle &&
                a.ReleaseDate == releaseDate);

            if (existingAlbum != null)
            {
                return existingAlbum;
            }

            Album album = new()
            {
                Title = normalizedTitle,
                ReleaseDate = releaseDate
            };

            _context.Albums.Add(album);
            _context.SaveChanges();

            DatabaseLogger.Log(
                "CREATE",
                "Album",
                $"Title: {normalizedTitle}");

            return album;
        }

        internal AlbumSong AddSongToAlbum(
            int albumId,
            int songId)
        {
            AlbumSong albumSong = new()
            {
                AlbumId = albumId,
                SongId = songId
            };

            _context.AlbumSongs.Add(albumSong);
            _context.SaveChanges();

            DatabaseLogger.Log(
                "CREATE",
                "AlbumSong",
                $"AlbumId: {albumId} | SongId: {songId}");

            return albumSong;
        }
    }
}