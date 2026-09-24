using MusicPlatform.Data;
using MusicPlatform.Logging;
using MusicPlatform.Models;

namespace MusicPlatform.Services
{
    internal class MediaService
    {
        private readonly MusicPlatformContext _context;

        internal MediaService(MusicPlatformContext context)
        {
            _context = context;
        }
        internal MediaType GetOrCreateMediaType(string mediaTypeName)
        {
            MediaType? mediaType = _context.MediaTypes
                .FirstOrDefault(mt => mt.Name == mediaTypeName);

            if (mediaType != null)
            {
                return mediaType;
            }

            mediaType = new MediaType
            {
                Name = mediaTypeName
            };

            _context.MediaTypes.Add(mediaType);
            _context.SaveChanges();

            DatabaseLogger.Log(
                "CREATE",
                "MediaType",
                $"MediaTypeId: {mediaType.Id} | Name: {mediaTypeName}");

            return mediaType;
        }


        internal Media AddMedia(
            int songId,
            int mediaTypeId,
            string externalId)
        {
            if (string.IsNullOrWhiteSpace(externalId))
            {
                throw new ArgumentException(
                    "ExternalId cannot be empty.");
            }

            Media media = new()
            {
                SongId = songId,
                MediaTypeId = mediaTypeId,
                ExternalId = externalId
            };

            _context.Media.Add(media);
            _context.SaveChanges();

            DatabaseLogger.Log(
                "CREATE",
                "Media",
                $"MediaId: {media.Id} | SongId: {songId} | ExternalId: {externalId}");

            return media;
        }
    }
}
