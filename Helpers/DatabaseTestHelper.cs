using MusicPlatform.Application.Services;
using MusicPlatform.Application.Models;
using MusicPlatform.Data;
using MusicPlatform.Models;
using MusicPlatform.Services;
using MusicPlatform.UI;
using Npgsql;

namespace MusicPlatform.Helpers
{
    internal class DatabaseTestHelper
    {
        private readonly IInput _input;
        private readonly IOutput _output;
        private readonly SongCreationApplicationService _songCreationApplicationService;
        private readonly SongCreationService _songCreationService;
        private readonly SongService _songService;
        private readonly SongInfo _testSong;


        internal DatabaseTestHelper(SongCreationApplicationService songCreationApplicationService, IInput input, IOutput output, SongCreationService songCreationService, SongService songService)
        {
            _input = input;
            _output = output;
            _songCreationApplicationService = songCreationApplicationService;
            _songCreationService = songCreationService;
            _songService = songService;
        }

        internal SongInfo? TestCreate()
        {
            _output.WriteInfo("* Create *");

            try
            {
                SongInfo song = CreateSongInfo(
                    title: "Test Song",
                    mainArtist: "Test Artist"
                    );

                _songCreationService.CreateSong(song);

                _output.WriteSuccess(
                    "Song created successfully.");

                PrintSongs(_songService);

                return song;
            }
            catch (ArgumentException ex)
            {
                _output.WriteError(
                    $"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _output.WriteError(
                    $"Unexpected error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    _output.WriteError(
                        $"Database error: {ex.InnerException.Message}");
                }
            }

            return null;
        }
        private SongInfo CreateSongInfo(string title, string mainArtist, List<MediaInfo>? media = null, List<string>? featuredArtists = null, List<AlbumInfo>? albums = null)
        {
            if (media == null)
            {
                media = new List<MediaInfo>() { new MediaInfo { Type = "YouTube", ExternalId = "external-test-id" } };
            }
            if (featuredArtists == null)
            {
                featuredArtists = new List<string> { "test-artist-2" };
            }
            if (albums == null)
            {
                albums = new List<AlbumInfo>() { new AlbumInfo { Title = "test-album" } };
            }

            return new SongInfo()
            {
                Title = title,
                MainArtist = mainArtist,
                FeaturedArtists = featuredArtists,
                Albums = albums,
                Media = media
            };
        }
        internal void TestRead()
        {
            _output.WriteInfo("* Read *");

            try
            {
                List<Song> songs =
                    _songService.SearchSongs("Test");

                _output.WriteSuccess("Search results:");

                songs.ForEach(song => { PrintSongDetails(song); });
            }
            catch (Exception ex)
            {
                _output.WriteError(
                    $"Unexpected error: {ex.Message}");
            }
        }

        internal void TestUpdate(
            SongInfo? testSong)
        {
            _output.WriteInfo("* Update *");

            try
            {
                if (testSong == null)
                {
                    _output.WriteError(
                        "Update skipped because the test song was not created.");
                }
                else
                {
                    _songService.UpdateSongTitle(
                        testSong.Id,
                        "Updated Test Song");

                    _output.WriteSuccess(
                        "Song updated successfully.");
                }

                PrintSongs(_songService);
            }
            catch (ArgumentException ex)
            {
                _output.WriteError(
                    $"Validation error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                _output.WriteError(
                    $"Operation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _output.WriteError(
                    $"Unexpected error: {ex.Message}");

                _output.WriteError(
                    $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        internal void TestDelete(
            SongInfo? testSong)
        {
            _output.WriteInfo("* Delete *");

            _output.Write($"Song to delete: {UIHelpers.FormatSong(testSong)}");
            try
            {

                if (testSong == null)
                {
                    _output.WriteError(
                        "Delete skipped because the test song was not created.");
                }
                else
                {
                    _songService.DeleteSong(testSong.Id);

                    _output.WriteSuccess(
                        "Song deleted successfully.");
                }
                PrintSongs(_songService);
            }
            catch (InvalidOperationException ex)
            {
                _output.WriteError(
                    $"Operation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _output.WriteError(
                    $"Unexpected error: {ex.Message}");

                _output.WriteError(
                    $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        internal void TestMainArtistChange(
            MusicPlatformContext context)
        {
            _output.WriteInfo("* Trigger *");

            try
            {
                using var transaction =
                    context.Database.BeginTransaction();

                Song? testSong = context.Songs
                    .FirstOrDefault(s => s.Title == "BRÆNDER");

                if (testSong == null)
                {
                    throw new InvalidOperationException(
                        "The test song was not found.");
                }

                SongArtist? mainArtist = context.SongArtists
                    .FirstOrDefault(sa =>
                        sa.SongId == testSong.Id &&
                        sa.IsMainArtist);

                SongArtist? newMainArtist = context.SongArtists
                    .FirstOrDefault(sa =>
                        sa.SongId == testSong.Id &&
                        !sa.IsMainArtist);

                if (mainArtist == null || newMainArtist == null)
                {
                    throw new InvalidOperationException(
                        "The test song does not have the expected artists.");
                }

                mainArtist.IsMainArtist = false;
                newMainArtist.IsMainArtist = true;

                context.SaveChanges();

                Artist oldArtist = context.Artists
                    .First(a => a.Id == mainArtist.ArtistId);

                Artist newArtist = context.Artists
                    .First(a => a.Id == newMainArtist.ArtistId);

                _output.WriteSuccess(
                    $"Main artist changed from '{oldArtist.Name}' to '{newArtist.Name}'.");

                transaction.Rollback();

                _output.WriteSuccess(
                    "Main artist restored.");
            }
            catch (Exception ex)
            {
                _output.WriteError(
                    $"Trigger error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    _output.WriteError(
                        $"Database error: {ex.InnerException.Message}");
                }
            }
        }

        internal void TestFirstArtistBecomesMain(
            MusicPlatformContext context)
        {
            _output.WriteInfo("* Trigger Test *");

            try
            {
                using var transaction =
                    context.Database.BeginTransaction();

                Song triggerTestSong = new()
                {
                    Title = "Trigger Test Song"
                };

                context.Songs.Add(triggerTestSong);
                context.SaveChanges();

                Artist? testArtist =
                    context.Artists.FirstOrDefault();

                if (testArtist == null)
                {
                    throw new InvalidOperationException(
                        "No artist was found.");
                }

                SongArtist testSongArtist = new()
                {
                    SongId = triggerTestSong.Id,
                    ArtistId = testArtist.Id,
                    IsMainArtist = false
                };

                context.SongArtists.Add(testSongArtist);
                context.SaveChanges();

                context.Entry(testSongArtist).Reload();

                if (!testSongArtist.IsMainArtist)
                {
                    throw new InvalidOperationException(
                        "Trigger did not set the first artist as main artist.");
                }

                _output.WriteSuccess(
                    $"Trigger correctly set '{testArtist.Name}' as main artist for '{triggerTestSong.Title}'.");

                //UIHelpers.WriteGreen(
                //    $"Trigger correctly set ArtistId {testArtist.Id} as main artist for SongId {triggerTestSong.Id}.");

                transaction.Rollback();
            }
            catch (Exception ex)
            {
                _output.WriteError(
                    $"Trigger test failed: {ex.Message}");

                if (ex.InnerException != null)
                {
                    _output.WriteError(
                        $"Database error: {ex.InnerException.Message}");
                }
            }
        }

        internal void TestMainArtistValue(
            MusicPlatformContext context)
        {
            _output.WriteInfo("* Main Artist Value Test *");

            try
            {
                using var transaction =
                    context.Database.BeginTransaction();

                Artist? firstArtist =
                    context.Artists.FirstOrDefault();

                Artist? secondArtist =
                    context.Artists
                        .Skip(1)
                        .FirstOrDefault();

                if (firstArtist == null || secondArtist == null)
                {
                    throw new InvalidOperationException(
                        "Two artists are required for this test.");
                }

                Song testSong = new()
                {
                    Title = "Main Artist Value Test Song"
                };

                context.Songs.Add(testSong);
                context.SaveChanges();

                SongArtist firstSongArtist = new()
                {
                    SongId = testSong.Id,
                    ArtistId = firstArtist.Id,
                    IsMainArtist = true
                };

                context.SongArtists.Add(firstSongArtist);
                context.SaveChanges();

                SongArtist secondSongArtist = new()
                {
                    SongId = testSong.Id,
                    ArtistId = secondArtist.Id,
                    IsMainArtist = false
                };

                context.SongArtists.Add(secondSongArtist);
                context.SaveChanges();

                context.Entry(secondSongArtist).Reload();

                if (secondSongArtist.IsMainArtist)
                {
                    throw new InvalidOperationException(
                        "Trigger incorrectly changed the second artist to main artist.");
                }

                _output.WriteSuccess(
                    $"Trigger correctly respected IsMainArtist = false, and kept '{secondArtist.Name}' as non-main artist.");

                //UIHelpers.WriteGreen(
                //    $"Trigger correctly respected IsMainArtist = false, and kept ArtistId {secondArtist.Id} as non-main artist.");

                transaction.Rollback();
            }
            catch (Exception ex)
            {
                _output.WriteError(
                    $"Main artist value test failed: {ex.Message}");

                if (ex.InnerException != null)
                {
                    _output.WriteError(
                        $"Database error: {ex.InnerException.Message}");
                }
            }
        }
        private void PrintSongDetails(Song song)
        {

            SongInfo songInfo = _songService.GetSongInfo(song.Id);

            _output.WriteLine(UIHelpers.FormatSong(songInfo));

        }
        private void PrintSongs(
            SongService songService)
        {
            var context = new MusicPlatformContext();

            List<Song> songs =
                songService.GetSongs();

            _output.WriteInfo("Songs in DB:");
            songs.ForEach(s =>
            {
                _output.WriteLine(UIHelpers.OLDFormatSong(context, s));
                _output.WriteLine("");
            });
        }
        
        internal int? TestCreateUser(UserService userService)
        {
            _output.WriteInfo("* Create User *");

            try
            {
                string username = "testuser";
                string password = "test123";
                string role = "User";

                userService.CreateUser(
                    username,
                    password,
                    role);

                User? user = userService.GetUser(username);

                _output.WriteSuccess(
                    $"User '{username}' created successfully.");

                return user?.Id;
            }
            catch (PostgresException ex)
            {
                _output.WriteError(
                    $"Database error: {ex.MessageText}");
            }
            catch (Exception ex)
            {
                _output.WriteError(
                    $"Unexpected error: {ex.Message}");
            }

            return null;
        }


        internal void TestUserPermissions(UserService userService)
        {
            _output.WriteInfo("* User Permissions *");

            try
            {
                User? testUser = userService.GetUser("testuser");

                if (testUser == null)
                {
                    _output.WriteError("Test user was not found.");
                    return;
                }

                try
                {
                    userService.DeleteUser(testUser.Id, "User");

                    _output.WriteError(
                        "User was incorrectly allowed to delete.");
                }
                catch (UnauthorizedAccessException)
                {
                    _output.WriteSuccess(
                        "User correctly denied delete access.");
                }

                userService.DeleteUser(testUser.Id, "Admin");

                _output.WriteSuccess(
                    "Admin correctly allowed to delete.");
            }
            catch (Exception ex)
            {
                _output.WriteError(
                    $"User permission test failed: {ex.Message}");
            }
        }
        internal void TestUserValidation(UserService userService)
        {
            _output.WriteInfo("* User Validation *");

            try
            {
                userService.CreateUser(
                    "",
                    "test123",
                    "User");

                _output.WriteError(
                    "Invalid user was incorrectly created.");
            }
            catch (PostgresException ex)
            {
                _output.WriteSuccess(
                    $"Validation correctly rejected user: {ex.MessageText}");
            }
            catch (Exception ex)
            {
                _output.WriteError(
                    $"Unexpected error: {ex.Message}");
            }
        }

    }
}

