using MusicPlatform.Application.Models;

namespace MusicPlatform.Application.Mappers
{
    public static class UserMapper
    {
        public static UserInfo ToUserInfo(int id, string username, string role)
        {
            return new UserInfo
            {
                Id = id,
                Username = username,
                Role = role
            };
        }
    }
}
