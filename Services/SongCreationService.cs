using MusicPlatform.Application.Models;
using MusicPlatform.Data;
using MusicPlatform.Models;

namespace MusicPlatform.Services
{
    internal class SongCreationService
    {
        private readonly IMusicPlatformContext _context;
        private readonly SongService _songService;
        private readonly ArtistService _artistService;
        private readonly AlbumService _albumService;
        private readonly MediaService _mediaService;

        internal SongCreationService( 
            IMusicPlatformContext context,
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

        internal SongInfo CreateSong(SongInfo songInfo)
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

                songInfo.Id= song.Id;
                return songInfo;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }
    }
}