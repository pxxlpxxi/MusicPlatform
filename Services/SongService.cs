using Microsoft.EntityFrameworkCore;
using MusicPlatform.Application.Mappers;
using MusicPlatform.Application.Models;
using MusicPlatform.Data;
using MusicPlatform.Logging;
using MusicPlatform.Models;

namespace MusicPlatform.Services
{
    public class SongService
    {
        private readonly IMusicPlatformContext _context;
        public SongService(IMusicPlatformContext context)
        {
            _context = context;
        }

        //CREATE: A song must have a title
        public Song CreateSong(string title)
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
        public List<Song> SearchSongs(string searchTerm)
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
        public void UpdateSongTitle(int songId, string newTitle)
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
        public void DeleteSong(int songId)
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

        public List<Song> GetSongs()
        {
            DatabaseLogger.Log("READ", "Song", $"Fetching all songs.");
            return _context.Songs.ToList();
        }

        public SongInfo GetSongInfoBySongId(int songId)
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
            //GetSongArtistsBySongId();

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
            //Fordi _context.Artists er et EF Core DbSet bliver LINQ ikke bare kørt som almindelig C#-kode.
            //EF Core oversætter udtrykket til SQL.
            //feks: SELECT * FROM "Artist" WHERE "Id" IN (1, 10, 401), og
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

            //DatabaseLogger.Log("READ", "Song",
            //    $"Fetching data connected to Song with Id: {songId} | " +
            //    $"Results: {songArtists.Count + artists.Count + albumSongs.Count + albums.Count + media.Count + mediaTypes.Count}");

            return SongMapper.ToSongInfo(
                song,
                songArtists,
                artists,
                albumSongs,
                albums,
                media,
                mediaTypes);
        }

        public SongInfo UpdateSong(
    int songId,
    SongInfo songInfo)
        {
            if (songInfo == null)
            {
                throw new ArgumentException(
                    "Song data is required.");
            }

            if (string.IsNullOrWhiteSpace(songInfo.Title))
            {
                throw new ArgumentException(
                    "Song must have a title.");
            }

            if (string.IsNullOrWhiteSpace(songInfo.MainArtist))
            {
                throw new ArgumentException(
                    "Main artist is required.");
            }

            Song? song =
                _context.Songs
                .FirstOrDefault(s => s.Id == songId);

            if (song == null)
            {
                throw new InvalidOperationException(
                    "The song does not exist.");
            }

            using var transaction =
                _context.Database.BeginTransaction();

            try
            {
                // SONG

                song.Title =
                    songInfo.Title.Trim();

                _context.SaveChanges();


                // ARTISTS

                List<SongArtist> existingSongArtists =
                    _context.SongArtists
                    .Where(sa => sa.SongId == songId)
                    .ToList();

                _context.SongArtists.RemoveRange(
                    existingSongArtists);

                _context.SaveChanges();


                // Main artist
                Artist? mainArtist =
                    _context.Artists
                    .FirstOrDefault(a =>
                        a.Name == songInfo.MainArtist.Trim());

                if (mainArtist == null)
                {
                    mainArtist = new Artist
                    {
                        Name = songInfo.MainArtist.Trim()
                    };

                    _context.Artists.Add(mainArtist);
                    _context.SaveChanges();
                }

                _context.SongArtists.Add(
                    new SongArtist
                    {
                        SongId = songId,
                        ArtistId = mainArtist.Id,
                        IsMainArtist = true
                    });


                // Featured artists
                foreach (
                    string artistName
                    in songInfo.FeaturedArtists)
                {
                    if (string.IsNullOrWhiteSpace(artistName))
                    {
                        continue;
                    }

                    string normalizedArtistName =
                        artistName.Trim();

                    Artist? artist =
                        _context.Artists
                        .FirstOrDefault(a =>
                            a.Name == normalizedArtistName);

                    if (artist == null)
                    {
                        artist = new Artist
                        {
                            Name = normalizedArtistName
                        };

                        _context.Artists.Add(artist);
                        _context.SaveChanges();
                    }

                    _context.SongArtists.Add(
                        new SongArtist
                        {
                            SongId = songId,
                            ArtistId = artist.Id,
                            IsMainArtist = false
                        });
                }

                _context.SaveChanges();


                // ALBUMS

                List<AlbumSong> existingAlbumSongs =
                    _context.AlbumSongs
                    .Where(als => als.SongId == songId)
                    .ToList();

                _context.AlbumSongs.RemoveRange(
                    existingAlbumSongs);

                _context.SaveChanges();


                foreach (
                    AlbumInfo albumInfo
                    in songInfo.Albums)
                {
                    if (string.IsNullOrWhiteSpace(
                        albumInfo.Title))
                    {
                        continue;
                    }

                    string normalizedAlbumTitle =
                        albumInfo.Title.Trim();

                    Album? album =
                        _context.Albums
                        .FirstOrDefault(a =>
                            a.Title == normalizedAlbumTitle &&
                            a.ReleaseDate ==
                                albumInfo.ReleaseDate);

                    if (album == null)
                    {
                        album = new Album
                        {
                            Title =
                                normalizedAlbumTitle,

                            ReleaseDate =
                                albumInfo.ReleaseDate
                        };

                        _context.Albums.Add(album);
                        _context.SaveChanges();
                    }

                    _context.AlbumSongs.Add(
                        new AlbumSong
                        {
                            SongId = songId,
                            AlbumId = album.Id
                        });
                }

                _context.SaveChanges();


                // MEDIA

                List<Media> existingMedia =
                    _context.Media
                    .Where(m => m.SongId == songId)
                    .ToList();

                _context.Media.RemoveRange(
                    existingMedia);

                _context.SaveChanges();


                foreach (
                    MediaInfo mediaInfo
                    in songInfo.Media)
                {
                    if (string.IsNullOrWhiteSpace(
                        mediaInfo.ExternalId))
                    {
                        continue;
                    }

                    string mediaTypeName =
                        mediaInfo.Type.Trim();

                    MediaType? mediaType =
                        _context.MediaTypes
                        .FirstOrDefault(mt =>
                            mt.Name == mediaTypeName);

                    if (mediaType == null)
                    {
                        mediaType = new MediaType
                        {
                            Name = mediaTypeName
                        };

                        _context.MediaTypes.Add(
                            mediaType);

                        _context.SaveChanges();
                    }

                    _context.Media.Add(
                        new Media
                        {
                            SongId = songId,

                            MediaTypeId =
                                mediaType.Id,

                            ExternalId =
                                mediaInfo.ExternalId.Trim()
                        });
                }

                _context.SaveChanges();

                transaction.Commit();

                DatabaseLogger.Log(
                    "UPDATE",
                    "Song",
                    $"SongId: {songId} | " +
                    $"Title: {song.Title} | " +
                    $"MainArtist: {songInfo.MainArtist} | " +
                    $"FeaturedArtists: {string.Join(", ", songInfo.FeaturedArtists)} | " +
                    $"Albums: {string.Join(", ", songInfo.Albums.Select(a => a.Title))} | " +
                    $"Media: {string.Join(", ", songInfo.Media.Select(m => m.Type + ":" + m.ExternalId))}");

                // RETURN UPDATED SONG

                return GetSongInfoBySongId(songId);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


    }
}