using MusicPlatform.Application.Models;
using MusicPlatform.Data;
using MusicPlatform.Models;


namespace MusicPlatform.UI
{
    internal static class UIHelper
    {
        private const ConsoleColor Green = ConsoleColor.DarkGreen;

        private const ConsoleColor Blue = ConsoleColor.DarkBlue;

        private const ConsoleColor Red = ConsoleColor.DarkRed;

        internal static string FormatSong(SongInfo song)
        {
            string title = song.Title;

            if (song.FeaturedArtists.Count > 0)
            {
                title += $"{FormatFeaturedArtists(song.FeaturedArtists)}";
            }

            string albums = FormatAlbums(song.Albums);

            string media = string.Join(
                ", ",
                song.Media.Select(m => $"{m.Type}: {m.ExternalId}"));

            return $"Title: {title} | " +
                $"Artist: {song.MainArtist} | " +
                $"Albums: {albums} | " +
                $"Media: {media}";


        }

        private static string FormatFeaturedArtists(List<string> featuredArtists)
        {
            if (featuredArtists.Count == 0)
            {
                return "";
            }

            if (featuredArtists.Count == 1)
            {
                return $" feat. {featuredArtists[0]}";
            }

            if (featuredArtists.Count == 2)
            {
                return $" feat. {featuredArtists[0]} & {featuredArtists[1]}";
            }

            string allButLast = string.Join(
                ", ",
                featuredArtists.Take(featuredArtists.Count - 1));

            string last = featuredArtists[^1];

            return $" feat. {allButLast} & {last}";
        }
        private static string FormatAlbums(List<AlbumInfo> albums)
        {
            if (albums.Count == 0)
            {
                return "N/A";
            }

            if (albums.Count == 1)
            {
                return $"{albums[0].Title}";
            }

            if (albums.Count == 2)
            {
                return $"{albums[0].Title} & {albums[1].Title}";
            }

            string allButLast = string.Join(
                ", ",
                albums.Take(albums.Count - 1).Select(a => a.Title));

            string last = albums[^1].Title;

            return $"{allButLast} & {last}";
        }
        internal static string OLDFormatSong(
            IMusicPlatformContext context,
            Song song)
        {
            List<SongArtist> songArtists = context.SongArtists
                .Where(sa => sa.SongId == song.Id)
                .ToList();

            List<string> artists = songArtists
                .Select(sa =>
                {
                    Artist artist = context.Artists
                        .First(a => a.Id == sa.ArtistId);

                    return sa.IsMainArtist
                        ? $"{artist.Name} (Main Artist)"
                        : artist.Name;
                })
                .ToList();

            List<AlbumSong> albumSongs = context.AlbumSongs
                .Where(als => als.SongId == song.Id)
                .ToList();

            List<string> albums = albumSongs
                .Select(als =>
                    context.Albums
                        .First(a => a.Id == als.AlbumId)
                        .Title)
                .ToList();

            List<Media> media = context.Media
                .Where(m => m.SongId == song.Id)
                .ToList();

            List<string> mediaInfo = media
                .Select(m =>
                {
                    MediaType mediaType = context.MediaTypes
                        .First(mt => mt.Id == m.MediaTypeId);

                    return $"{mediaType.Name}: {m.ExternalId}";
                })
                .ToList();

            return
                $"Song: {song.Title} | " +
                $"Artists: {string.Join(", ", artists)} | " +
                $"Album: {string.Join(", ", albums)} | " +
                $"Media: {string.Join(", ", mediaInfo)}";
        }

    }
}

    