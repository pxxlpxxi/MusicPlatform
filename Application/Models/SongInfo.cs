using System.Collections.Generic;

namespace MusicPlatform.Application.Models
{
    public class SongInfo
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string MainArtist { get; set; } = "";

        public List<string> FeaturedArtists { get; set; } = [];

        public List<AlbumInfo> Albums { get; set; } = [];

        public List<MediaInfo> Media { get; set; } = [];
    }
}
