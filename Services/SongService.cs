using MusicPlatform.Data;
using MusicPlatform.Helpers;
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

            song.Title = newTitle;
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
    }
}
