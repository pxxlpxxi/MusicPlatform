using Microsoft.EntityFrameworkCore;
using MusicPlatform.Data;
using MusicPlatform.Logging;
using MusicPlatform.Models;
using Npgsql;
using System.Data;

namespace MusicPlatform.Services
{
    internal class UserService
    {
        private readonly IMusicPlatformContext _context;

        internal UserService(IMusicPlatformContext context)
        {
            _context = context;
        }

        internal void CreateUserUnsafe(
            string username,
            string password,
            string role)
        {
            string sql =
                $"INSERT INTO \"User\" (\"Username\", \"Password\", \"Role\") " +
                $"VALUES ('{username}', '{password}', '{role}')";

            _context.Database.ExecuteSqlRaw(sql);
            
            DatabaseLogger.Log("CREATE", "User", $"Username: {username} | Role: {role}");
        }

        internal void CreateUser(
            string username,
            string password,
            string role)
        {
            string sql =
                "CALL CreateUser(@username, @password, @role)";

            _context.Database.ExecuteSqlRaw(
                sql,
                new NpgsqlParameter("@username", username),
                new NpgsqlParameter("@password", password),
                new NpgsqlParameter("@role", role));
           
            DatabaseLogger.Log("CREATE", "User", $"Username: {username} | Role: {role}");
                    }


        internal void DeleteUser(
            int userId,
            string role)
        {
            if (role != "Admin")
            {
                DatabaseLogger.Log("DELETE", "User", $"Failed to delete: UserId {userId} | Cause of failure: Insufficielt role permissions: {role}");

                throw new UnauthorizedAccessException(
                    "Only an admin can delete a user.");
            }

            string sql =
                "DELETE FROM \"User\" " +
                "WHERE \"Id\" = @userId";

            _context.Database.ExecuteSqlRaw(
                sql,
                new NpgsqlParameter("@userId", userId));

            DatabaseLogger.Log("DELETE", "User", $"Deleted user with UserId {userId} | Cause of success: Role permissions: {role}");

        }
        internal User? GetUser(string username)
        {

            User? result = _context.Users
                .FirstOrDefault(u => u.Username == username);

            DatabaseLogger.Log("READ", "User", $"Search: Username '{username}' | Result: {result.Username}");
            return result;

        }

    }
}
