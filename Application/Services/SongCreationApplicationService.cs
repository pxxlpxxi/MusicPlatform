using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicPlatform.Application.Models;
using MusicPlatform.Models;
using MusicPlatform.Services;
using MusicPlatform.UI;

namespace MusicPlatform.Application.Services
{
    internal class SongCreationApplicationService
    {
        private readonly SongCreationService _songCreationService;
        private readonly IInput _input;
        private readonly IOutput _output;
        internal SongCreationApplicationService(
            SongCreationService songCreationService,
            IInput input,
            IOutput output)
        {
            _songCreationService = songCreationService;
            _input = input;
            _output = output;
        }

        internal SongInfo AddNewSong()
        {
            string title = PromptForTitle();

            string mainArtist = PromptForArtist();

            List<string> featuredArtists = PromptForFeaturedArtists();

            List<AlbumInfo> albums = PromptForAlbums();

            List<MediaInfo> media = PromptForMedia();

            SongInfo song = CreateSongInfo(title, mainArtist, featuredArtists, albums, media);

            _songCreationService.CreateSong(song);
            return song;
        }

        private string PromptForTitle()
        {
            while (true)
            {
                _output.Write("Title: ");
                string title = _input.ReadString();
                if (!string.IsNullOrWhiteSpace(title))
                {
                    return ValidateAndNormalizeTitle(title);
                }
                _output.WriteError("Title cannot be empty.");
            }
        }
        private string PromptForArtist()
        {
            while (true)
            {
                _output.Write("Artist: ");

                string artist = _input.ReadString();

                if (!string.IsNullOrWhiteSpace(artist))
                {
                    return ValidateAndNormalizeArtist(artist);
                }

                _output.WriteError("Artist cannot be empty.");
            }
        }
        private List<string> PromptForFeaturedArtists()
        {
            List<string> featuredArtists = [];

            while (true)
            {
                _output.Write("Featured artist (press Enter when finished): ");

                string artist = _input.ReadString();

                if (string.IsNullOrWhiteSpace(artist))
                {
                    break;
                }

                featuredArtists.Add(
                    ValidateAndNormalizeArtist(artist));
            }

            return featuredArtists;
        }



        private List<AlbumInfo> PromptForAlbums()
        {
            List<AlbumInfo> albums = [];

            while (true)
            {
                string albumTitle = PromptForAlbumTitle();

                string normalizedAlbumTitle = ValidateAndNormalizeAlbumTitle(albumTitle);

                if (normalizedAlbumTitle == null)
                {
                    break;
                }
                
                DateOnly? releaseDate = PromptForReleaseDate();
                
                albums.Add(new AlbumInfo
                {
                    Title = normalizedAlbumTitle,
                    ReleaseDate = releaseDate
                });
                
                _output.Write("Add another album for the song? [ Y / N ]: ");
                ConsoleKeyInfo key = _input.ReadKey();

                _output.WriteLine("");

                if (key.Key != ConsoleKey.Y)
                {
                    break;
                }
            }
            return albums;
        }
        private string PromptForAlbumTitle()
        {
            _output.Write("Album title (leave blank to skip): ");
            string albumTitle = _input.ReadString();

            return ValidateAndNormalizeAlbumTitle(albumTitle);
        }
        private DateOnly? PromptForReleaseDate() {
            while (true)
            {
                _output.Write(
                    "Release date (YYYY-MM-DD, or leave blank to skip): ");

                string input = _input.ReadString();

                if (string.IsNullOrWhiteSpace(input))
                {
                    return null;
                }

                if (DateOnly.TryParse(input, out DateOnly date))
                {
                    return date;
                }

                _output.WriteError("Invalid date. Please use YYYY-MM-DD. Example: 2008-09-30.");
            }
        }
        private List<MediaInfo> PromptForMedia()
        {
            List<MediaInfo> media = [];
            while (true)
            {
                MediaTypeName? mediaType = PromptForMediaType();

                if (mediaType == null)
                {
                    break;
                }
                    string externalId = PromptForExternalId();

                    if (mediaType == MediaTypeName.YouTube)
                    {
                        externalId = ExtractYouTubeId(externalId);
                    }
                    media.Add(new MediaInfo
                    {
                        Type = mediaType.ToString()!,
                        ExternalId = externalId
                    });
                _output.Write("Add another media link? [ Y / N ]: ");

                ConsoleKeyInfo key = _input.ReadKey();

                _output.WriteLine("");

                if (key.Key != ConsoleKey.Y)
                {
                    break;
                }
            }
            return media;
        }

        private string PromptForExternalId()
        {
            while (true)
            {
                _output.Write("Link or media ID: ");

                string externalId = _input.ReadString();

                if (string.IsNullOrWhiteSpace(externalId))
                {
                    _output.WriteError("Media link cannot be empty.");
                    continue;
                }
                return externalId.Trim();
            }
        }

        private MediaTypeName? PromptForMediaType()
        {
            bool exit = false;
            string[] options = {
                "",
                "Select media source type:",
                "",
                "[1] YouTube",
                "[2] Spotify",
                "[3] SoundCloud",
                "",
                "[R] Return",
                ""
            };

            do
            {
                foreach (var o in options)
                {
                    _output.WriteLine(o);
                }

                ConsoleKey? key = _input.WaitForKeyOrQuit();

                _output.WriteLine("");

                switch (key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        exit = true;
                        return MediaTypeName.YouTube;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        exit = true;
                        return MediaTypeName.Spotify;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        exit = true;
                        return MediaTypeName.SoundCloud;
                    case ConsoleKey.R:
                        exit = true;
                        break;
                }
            } while (!exit);
            return null;
        }
        private string ExtractYouTubeId(string input)
        {
            if (input.Contains("watch?v="))
            {
                return input
                    .Split(
                        new[] { "watch?v=" },
                        StringSplitOptions.None)[1]
                    .Split('?', '&')[0];
            }

            if (input.Contains("youtu.be/"))
            {
                return input
                    .Split(
                        new[] { "youtu.be/" },
                        StringSplitOptions.None)[1]
                    .Split('?', '&')[0];
            }

            return input.Trim();
        }

        private SongInfo CreateSongInfo(string title, string mainArtist, List<string> featuredArtists, List<AlbumInfo> albums, List<MediaInfo> media)
        {
            return new SongInfo()
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
