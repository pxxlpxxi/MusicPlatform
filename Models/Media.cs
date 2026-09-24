namespace MusicPlatform.Models
{
    public class Media
    {
        public int Id { get; set; }
        public string ExternalId { get; set; } = "";
        public int SongId { get; set; }
        public int MediaTypeId { get; set; }
    }
}