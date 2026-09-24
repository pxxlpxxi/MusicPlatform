using MusicPlatform.Application.Models;
using MusicPlatform.Models;

namespace MusicPlatform.Application.Mappers
{
    internal static class SongMapper
    {
        internal static SongInfo ToSongInfo(
            Song song,
            IEnumerable<SongArtist> songArtists,
            IEnumerable<Artist> artists,
            IEnumerable<AlbumSong> albumSongs,
            IEnumerable<Album> albums,
            IEnumerable<Media> media,
            IEnumerable<MediaType> mediaTypes)
        {
            var artistLookup = artists
                .ToDictionary(a => a.Id);

            var albumLookup = albums
                .ToDictionary(a => a.Id);

            var mediaTypeLookup = mediaTypes
                .ToDictionary(mt => mt.Id);

            var songArtistList = songArtists.ToList();

            SongArtist? mainArtist = songArtistList
                .FirstOrDefault(sa => sa.IsMainArtist);

            string mainArtistName = mainArtist != null
                ? artistLookup[mainArtist.ArtistId].Name
                : "";

            List<string> featuredArtists = songArtistList
                .Where(sa => !sa.IsMainArtist)
                .Select(sa => artistLookup[sa.ArtistId].Name)
                .ToList();

            List<AlbumInfo> songAlbums = albumSongs
                .Select(albumSong => albumLookup[albumSong.AlbumId])
                .Select(album => new AlbumInfo
                {
                    Title = album.Title,
                    ReleaseDate = album.ReleaseDate
                })
                .ToList();

            List<MediaInfo> songMedia = media
                .Select(m => new MediaInfo
                {
                    Type = mediaTypeLookup[m.MediaTypeId].Name,
                    ExternalId = m.ExternalId
                })
                .ToList();

            return new SongInfo
            {
                Id = song.Id,
                Title = song.Title,
                MainArtist = mainArtistName,
                FeaturedArtists = featuredArtists,
                Albums = songAlbums,
                Media = songMedia
            };
        }                 
    }
}
