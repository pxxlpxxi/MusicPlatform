using MusicPlatform.Data;
using MusicPlatform.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicPlatform.Helpers
{
    internal static class UIHelpers
    {
        private const ConsoleColor Green = ConsoleColor.DarkGreen;

        private const ConsoleColor Blue = ConsoleColor.DarkBlue;

        private const ConsoleColor Red = ConsoleColor.DarkRed;

        internal static void WriteBlue(string message)
        {
            Console.ForegroundColor = Blue;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        internal static void WriteGreen(string message)
        {
            Console.ForegroundColor = Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        internal static void WriteRed(string message)
        {
            Console.ForegroundColor = Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        internal static string FormatSong(
            MusicPlatformContext context,
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
