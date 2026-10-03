namespace MusicPlatform.Models
{
    public class AuthenticatedUser
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string Role { get; set; } = "User";
    }
}
