using MusicPlatform.Application.Models;
using MusicPlatform.Data;
using MusicPlatform.Models;

namespace MusicPlatform.Services
{
    internal class SongCreationService
    {
        private readonly MusicPlatformContext _context;
        private readonly SongService _songService;
        private readonly ArtistService _artistService;
        private readonly AlbumService _albumService;
        private readonly MediaService _mediaService;

        internal SongCreationService(
            MusicPlatformContext context,
            SongService songService,
            ArtistService artistService,
            AlbumService albumService,
            MediaService mediaService)
        {
            _context = context;
            _songService = songService;
            _artistService = artistService;
            _albumService = albumService;
            _mediaService = mediaService;
        }

        internal void CreateSong(SongInfo songInfo)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Song song = _songService.CreateSong(songInfo.Title);

                Artist mainArtist = _artistService.CreateArtist(songInfo.MainArtist);

                _artistService.AddSongArtist(song.Id, mainArtist.Id, true);

                foreach (string artistName in songInfo.FeaturedArtists)
                {
                    Artist artist = _artistService.CreateArtist(artistName);

                    _artistService.AddSongArtist(song.Id, artist.Id, false);
                }

                foreach (AlbumInfo albumInfo in songInfo.Albums)
                {
                    Album album = _albumService.CreateAlbum(albumInfo.Title, albumInfo.ReleaseDate);

                    _albumService.AddSongToAlbum(album.Id, song.Id);
                }

                foreach (MediaInfo mediaInfo in songInfo.Media)
                {
                    MediaType mediaType = _mediaService.GetOrCreateMediaType(mediaInfo.Type);

                    _mediaService.AddMedia(song.Id, mediaType.Id, mediaInfo.ExternalId);
                }
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }
    }
}
//}

//internal Song AddNewSong(
//    string title,
//    string mainArtistName,
//    string mediaType,
//    string externalId,
//    List<string>? additionalArtistNames = null,
//    string? albumTitle = null
//    )
//{
//    using var transaction =
//        _context.Database.BeginTransaction();

//    try
//    {

//        //1. validate and normalize title
//        string normalizedTitle =
//            ValidateAndNormalizeTitle(title);

//        //2. create Song
//        Song song = _songService.CreateSong(normalizedTitle);

//        //3. validate and normalize main artist
//        string normalizedMainArtist = ValidateAndNormalizeArtist(mainArtistName);

//        //4. add/find main artist and add to song
//        Artist mainArtist =
//            _artistService.CreateArtist(normalizedMainArtist);

//        _artistService.AddSongArtist(
//            song.Id,
//            mainArtist.Id,
//            true);

//        //5. validate and normalize featured artists
//        List<string> normalizedFeaturedArtists =
//            ValidateAndNormalizeArtists(
//                additionalArtistNames);

//        //6. add/find featured artists and add to song
//        foreach (string artistName in normalizedFeaturedArtists)
//        {
//            Artist artist = _artistService.CreateArtist(artistName);

//            _artistService.AddSongArtist(
//                song.Id,
//                artist.Id,
//                false);
//        }

//        //7. validate and normalize album
//        string? normalizedAlbumTitle =
//            ValidateAndNormalizeAlbum(albumTitle);

//        //8. add/find album and add it to song
//        if (normalizedAlbumTitle != null)
//        {
//            Album album =
//                _albumService.CreateAlbum(
//                    normalizedAlbumTitle);

//            _albumService.AddSongToAlbum(
//                album.Id,
//                song.Id);
//        }


//        //9. validate and normalize media type
//        MediaTypeName normalizedMediaType = ValidateMediaType(mediaType);
//        //10. find or create MediaType
//        MediaType mediaTypeEntity =
//            _mediaService.GetOrCreateMediaType(
//                normalizedMediaType.ToString());

//        //11. add media
//        _mediaService.AddMedia(
//            song.Id,
//            mediaTypeEntity.Id,
//            externalId);

//        //trigger: song has at least one main artist
//        transaction.Commit();

//        return song;
//    }
//    catch
//    {
//        transaction.Rollback();
//        throw;
//    }
//}

//private string ValidateAndNormalizeAlbum(string albumTitle)
//{
//    if (string.IsNullOrWhiteSpace(albumTitle))
//    {
//        return null;
//    }

//    return albumTitle.Trim();
//}

//private List<string> ValidateAndNormalizeArtists(List<string>? artistNames)
//{
//    if (artistNames == null)
//    {
//        return new List<string>();
//    }

//    return artistNames
//        .Select(ValidateAndNormalizeArtist)
//        .ToList();
//}

//private string ValidateAndNormalizeArtist(string artistName)
//{
//    if (string.IsNullOrWhiteSpace(artistName))
//    {
//        throw new ArgumentException(
//            "Artist name is required.");
//    }

//    return artistName.Trim();
//}

//private string ValidateAndNormalizeTitle(string title)
//{
//    if (string.IsNullOrWhiteSpace(title))
//    {
//        throw new ArgumentException(
//            "Song title is required.");
//    }

//    return title.Trim();
//}

//private MediaTypeName ValidateMediaType(string mediaTypeName)
//{
//    if (string.IsNullOrWhiteSpace(mediaTypeName))
//    {
//        throw new ArgumentException(
//            "MediaType cannot be empty.");
//    }

//    if (!Enum.TryParse<MediaTypeName>(
//        mediaTypeName,
//        true,
//        out MediaTypeName parsedMediaType))
//    {
//        throw new ArgumentException(
//            $"MediaType '{mediaTypeName}' is not supported.");
//    }

//    return parsedMediaType;
//}
//    }
//}
