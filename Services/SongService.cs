using MusicPlatform.Application.Mappers;
using MusicPlatform.Application.Models;
using MusicPlatform.Data;
using MusicPlatform.Logging;
using MusicPlatform.Models;

namespace MusicPlatform.Services
{
    internal class SongService
    {
        private readonly MusicPlatformContext _context;
        internal SongService(MusicPlatformContext context)
        {
            _context = context;
        }

        //CREATE: A song must have a title
        internal Song CreateSong(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(
                    "Song must have a title.");
            }

            Song song = new()
            {
                Title = title
            };

            _context.Songs.Add(song);
            _context.SaveChanges();

            DatabaseLogger.Log(
                "CREATE",
                "Song",
                $"{song.Id} | {song.Title}");

            return song;
        }

        //READ: The search term cannot be empty
        internal List<Song> SearchSongs(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Search term cannot be empty.");
            }

            List<Song> songs = _context
               .Songs
               .Where(
               s =>
               s.Title.Contains(searchTerm))
               .ToList();

            DatabaseLogger.Log("READ", "Song", $"Search: {searchTerm} | Result: {songs.Count}");
            return songs;

        }

        //UPDATE: A song must have a title, and the song must exist
        internal void UpdateSongTitle(int songId, string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentException("Song must have a title.");
            }

            Song? song = _context.Songs.Find(songId);

            if (song == null)
            {
                throw new InvalidOperationException("The song does not exist.");
            }

            string normalizedTitle = newTitle.Trim();

            song.Title = normalizedTitle;
            _context.SaveChanges();
            DatabaseLogger.Log("UPDATE", "Song", $"{song.Id} | {song.Title}");
        }

        //DELETE: The song must exist before it can be deleted
        internal void DeleteSong(int songId)
        {
            Song? song = _context.Songs.Find(songId);

            if (song == null)
            {
                throw new InvalidOperationException("The song does not exist.");
            }
            _context.Songs.Remove(song);
            _context.SaveChanges();
            DatabaseLogger.Log("DELETE", "Song", $"{song.Id} | {song.Title}");

        }

        internal List<Song> GetSongs()
        {
            return _context.Songs.ToList();
        }

        internal SongInfo GetSongInfo(int songId)
        {
            Song? song =
                _context.Songs
                .FirstOrDefault(s => s.Id == songId);

            if (song == null)
            {
                throw new InvalidOperationException("The song does not exist.");
            }
            List<SongArtist> songArtists =
                _context.SongArtists
                .Where(sa => sa.SongId == songId)
                .ToList();

            List<int> artistIds =
                 songArtists
                 .Select(sa => sa.ArtistId)
                 .Distinct()
                 .ToList();

            List<Artist> artists =
                _context.Artists
                .Where(a => artistIds.Contains(a.Id))
                .ToList();

            //Contains() her betyder i praksis: "findes denne præcise int-værdi i listen?"
            //Fordi _context.Artists er en EF Core DbSet, bliver LINQ ikke bare kørt som almindelig C#-kode.
            //EF Core oversætter udtrykket til SQL.
            //basically, feks: SELECT * FROM "Artist" WHERE "Id" IN (1, 10, 401), og
            //altså IKKE det samme som:
            // string id = "401";
            //text.Contains("1");

            List<AlbumSong> albumSongs =
                _context.AlbumSongs
                .Where(als => als.SongId == songId)
                .ToList();

            List<int> albumIds =
                albumSongs
                .Select(als => als.AlbumId)
                .Distinct()
                .ToList();

            List<Album> albums =
                _context.Albums
                .Where(a => albumIds.Contains(a.Id))
                .ToList();

            List<Media> media =
                _context.Media
                .Where(m => m.SongId == songId)
                .ToList();

            List<int> mediaTypeIds =
                media
                .Select(m => m.MediaTypeId)
                .Distinct()
                .ToList();

            List<MediaType> mediaTypes =
                _context.MediaTypes
                .Where(mt => mediaTypeIds.Contains(mt.Id))
                .ToList();

            return SongMapper.ToSongInfo(
                song,
                songArtists,
                artists,
                albumSongs,
                albums,
                media,
                mediaTypes);
        }
    }
}