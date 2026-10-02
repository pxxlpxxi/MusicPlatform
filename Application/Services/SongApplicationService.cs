using MusicPlatform.Application.Models;
using MusicPlatform.Data;
using MusicPlatform.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicPlatform.Application.Services
{
    public class SongApplicationService
    {
        private readonly SongService _songService;
        private readonly SongCreationService _songCreationService;

        //public SongApplicationService(SongService songService)
        //{
        //    _songService = songService;
        //}

        //api constructor
        public SongApplicationService(
            IMusicPlatformContext context,
            SongService songService)
        {
            _songService = songService;

            ArtistService artistService = new(context);
            AlbumService albumService = new(context);
            MediaService mediaService = new(context);

            _songCreationService = new SongCreationService(
                context,
                songService,
                artistService,
                albumService,
                mediaService);
        }


        public SongInfo CreateSong(SongInfo song)
        {
            foreach (MediaInfo media in song.Media)
            {
                if (media.Type.Equals(
                    MediaTypeName.YouTube.ToString(),
                    StringComparison.OrdinalIgnoreCase))
                {
                    media.ExternalId =
                        ExtractYouTubeId(media.ExternalId);
                }
            }

            return _songCreationService.CreateSong(song);
        }

        public List<SongInfo> GetSongs()
        {
            return _songService
                .GetSongs()
                .Select(song => _songService.GetSongInfoBySongId(song.Id))
                .ToList();
        }

        public SongInfo GetSong(int songId)
        {
            return _songService.GetSongInfoBySongId(songId);
        }

        public List<SongInfo> SearchSongs(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException(
                    "Search term cannot be empty.");
            }

            string normalizedSearchTerm =
                searchTerm.Trim();

            return GetSongs()
                .Where(song =>
                    song.Title.Contains(
                        normalizedSearchTerm,
                        StringComparison.OrdinalIgnoreCase)

                    ||

                    song.MainArtist.Contains(
                        normalizedSearchTerm,
                        StringComparison.OrdinalIgnoreCase)

                    ||

                    song.FeaturedArtists.Any(artist =>
                        artist.Contains(
                            normalizedSearchTerm,
                            StringComparison.OrdinalIgnoreCase))
                )
                .ToList();
        }


        //public List<SongInfo> SearchSongs(string searchTerm)
        //{
        //    return _songService
        //        .SearchSongs(searchTerm)
        //        .Select(song => _songService.GetSongInfoBySongId(song.Id))
        //        .ToList();
        //}

        public void DeleteSong(int songId)
        {
            _songService.DeleteSong(songId);
        }
        public void UpdateSongTitle(int songId, string newTitle)
        {
            _songService.UpdateSongTitle(songId, newTitle);
        }

        public SongInfo UpdateSong(
     int songId,
     SongInfo songInfo)
        {
            foreach (MediaInfo media in songInfo.Media)
            {
                if (media.Type.Equals(
                    MediaTypeName.YouTube.ToString(),
                    StringComparison.OrdinalIgnoreCase))
                {
                    media.ExternalId =
                        ExtractYouTubeId(
                            media.ExternalId);
                }
            }

            return _songService.UpdateSong(
                songId,
                songInfo);
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
    }

}
