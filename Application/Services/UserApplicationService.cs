using MusicPlatform.Application.Models;
using MusicPlatform.Services;

namespace MusicPlatform.Application.Services
{
    internal class UserApplicationService
    {
        private readonly UserService _userService;
        internal UserApplicationService(UserService userService)
        {
            _userService = userService;
        }

        internal void CreateUser(string username, string password, string role)
        {
            _userService.CreateUser(username, password, role);
        }
        internal UserInfo? Login(string username, string password)
        {
            return _userService.Login(username, password);
        }

            }
}
