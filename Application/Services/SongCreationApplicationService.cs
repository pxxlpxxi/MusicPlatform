using MusicPlatform.Application.Models;
using MusicPlatform.Models;
using MusicPlatform.Services;

namespace MusicPlatform.Application.Services
{
    internal class SongCreationApplicationService
    {
        private readonly SongCreationService _songCreationService;
        private readonly SongService _songService;

        internal SongCreationApplicationService(
            SongCreationService songCreationService,
            SongService songService)
        {
            _songCreationService = songCreationService;

            _songService = songService;
        }

        internal SongInfo AddNewSong()
        {

            string title = PromptForTitle();

            string mainArtist = PromptForArtist();

            List<string> featuredArtists = PromptForFeaturedArtists();

            List<AlbumInfo> album = PromptForAlbum();

            List<MediaInfo> media = PromptForMedia();

            SongInfo song = CreateSongInfo(title, mainArtist, featuredArtists, albums, media);

            _songCreationService.CreateSong(song);
            return song;

        }

        private string PromptForTitle()
        {
            throw new NotImplementedException();

            //string normalizedTitle = ValidateAndNormalizeTitle(title);
        }

        private string PromptForArtist()
        {
            throw new NotImplementedException();
            //string normalizedMainArtist = ValidateAndNormalizeArtist(mainArtistName);
        }

        private List<string> PromptForFeaturedArtists()
        {
            throw new NotImplementedException();
            //List<string> normalizedFeaturedArtists = ValidateAndNormalizeArtists(additionalArtistNames);
        }

        private List<AlbumInfo> PromptForAlbum()
        {
            throw new NotImplementedException();
            //string? normalizedAlbumTitle = ValidateAndNormalizeAlbum(albumTitle);

        }

        private List<MediaInfo> PromptForMedia()
        {
            throw new NotImplementedException();
            //MediaTypeName normalizedMediaType = ValidateMediaType(mediaType);
        }

        private SongInfo CreateSongInfo(string title, string mainArtist, List<string> featuredArtists, List<AlbumInfo> albums, List<MediaInfo> media)
        {
            throw new NotImplementedException();
            SongInfo song = new()
            {
                Title = title,
                MainArtist = mainArtist,
                FeaturedArtists = featuredArtists,
                Albums = albums,
                Media = media
            };
        }

        private string ValidateAndNormalizeAlbumTitle(
            string? albumTitle)
        {
            if (string.IsNullOrWhiteSpace(albumTitle))
            {
                return null;
            }

            return albumTitle.Trim();
        }

        private List<string> ValidateAndNormalizeArtists(
            List<string>? artistNames)
        {
            if (artistNames == null)
            {
                return new List<string>();
            }

            return artistNames
                .Select(ValidateAndNormalizeArtist)
                .ToList();
        }

        private string ValidateAndNormalizeArtist(
            string artistName)
        {
            if (string.IsNullOrWhiteSpace(artistName))
            {
                throw new ArgumentException(
                    "Artist name is required.");
            }

            return artistName.Trim();
        }

        private string ValidateAndNormalizeTitle(
            string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(
                    "Song title is required.");
            }

            return title.Trim();
        }

        private MediaTypeName ValidateMediaType(
            string mediaTypeName)
        {
            if (string.IsNullOrWhiteSpace(mediaTypeName))
            {
                throw new ArgumentException(
                    "MediaType cannot be empty.");
            }

            if (!Enum.TryParse(
                mediaTypeName,
                true,
                out MediaTypeName parsedMediaType))
            {
                throw new ArgumentException(
                    $"MediaType '{mediaTypeName}' is not supported.");
            }

            return parsedMediaType;
        }
    }

}
