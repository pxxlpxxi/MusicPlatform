using MusicPlatform.Data;
using MusicPlatform.Logging;
using MusicPlatform.Models;

namespace MusicPlatform.Services
{
    internal class ArtistService
    {
        private readonly MusicPlatformContext _context;

        internal ArtistService(MusicPlatformContext context)
        {
            _context = context;
        }
        internal Artist CreateArtist(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Artist name is required.");
            }

            string normalizedName = name.Trim();

            Artist? existingArtist = _context.Artists
                .FirstOrDefault(a => a.Name == normalizedName);

            if (existingArtist != null)
            {
                return existingArtist!;
            }

            Artist artist = new()
            {
                Name = normalizedName
            };

            _context.Artists.Add(artist);
            _context.SaveChanges();

            DatabaseLogger.Log(
                "CREATE",
                "Artist",
                $"Name: {normalizedName}");

            return artist!;
        }


        internal SongArtist AddSongArtist(
            int songId,
            int artistId,
            bool isMainArtist)
        {
            SongArtist songArtist = new()
            {
                SongId = songId,
                ArtistId = artistId,
                IsMainArtist = isMainArtist
            };

            _context.SongArtists.Add(songArtist);
            _context.SaveChanges();

            DatabaseLogger.Log(
                "CREATE",
                "SongArtist",
                $"SongId: {songId} | ArtistId: {artistId} | MainArtist: {isMainArtist}");

            return songArtist;
        }

        internal void ChangeMainArtist(
            int songId,
            int newArtistId)
        {
            SongArtist? currentMainArtist = _context.SongArtists
                .FirstOrDefault(sa =>
                    sa.SongId == songId &&
                    sa.IsMainArtist);

            if (currentMainArtist == null)
            {
                throw new InvalidOperationException(
                    "The song does not have a main artist.");
            }

            SongArtist? newMainArtist = _context.SongArtists
                .FirstOrDefault(sa =>
                    sa.SongId == songId &&
                    sa.ArtistId == newArtistId);

            if (newMainArtist == null)
            {
                throw new InvalidOperationException(
                    "The new artist is not associated with the song.");
            }

            currentMainArtist.IsMainArtist = false;
            newMainArtist.IsMainArtist = true;

            _context.SaveChanges();

            DatabaseLogger.Log(
                "UPDATE",
                "SongArtist",
                $"SongId: {songId} | New main ArtistId: {newArtistId}");
        }
    }
}
