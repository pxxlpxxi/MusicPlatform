using MusicPlatform.Data;
using MusicPlatform.Helpers;
using MusicPlatform.Models;

namespace MusicPlatform.Services
{
    internal class AlbumService
    {
        private readonly MusicPlatformContext _context;

        internal AlbumService(MusicPlatformContext context)
        {
            _context = context;
        }

        internal Album CreateAlbum(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(
                    "Album title is required.");
            }

            string normalizedTitle = title.Trim();

            Album? existingAlbum = _context.Albums
                .FirstOrDefault(a => a.Title == normalizedTitle);

            if (existingAlbum != null)
            {
                return existingAlbum;
            }

            Album album = new()
            {
                Title = normalizedTitle
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