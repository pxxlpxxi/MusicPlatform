using MusicPlatform.Application.Services;
using MusicPlatform.Application.Models;
using MusicPlatform.Data;
using MusicPlatform.Models;
using MusicPlatform.Services;
using MusicPlatform.UI;

namespace MusicPlatform.Helpers
{
    internal class DatabaseTestHelper
    {
        IInput _input =new Input();
        IOutput _output = new Output();

        internal SongInfo? TestCreate(
            SongCreationApplicationService songCreationApplicationService,
            SongService songService)
        {
            _output.WriteInfo("* Create *");

            try
            {
                SongInfo song = songCreationApplicationService.AddNewSong(
                    "Test Song",
                    "Test Artist",
                    "Youtube",
                    "test-external-id");

                _output.WriteSuccess(
                    "Song created successfully.");

                PrintSongs(songService);

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

        internal void TestRead(
            SongService songService)
        {
            _output.WriteInfo("* Read *");

            try
            {
                List<Song> songs =
                    songService.SearchSongs("Test");

                _output.WriteSuccess("Search results:");

                songs.ForEach(song => {PrintSongDetails(song);});
            }
            catch (Exception ex)
            {
                _output.WriteError(
                    $"Unexpected error: {ex.Message}");
            }
        }

        internal void TestUpdate(
            SongService songService,
            Song? testSong)
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
                    songService.UpdateSongTitle(
                        testSong.Id,
                        "Updated Test Song");

                    _output.WriteSuccess(
                        "Song updated successfully.");
                }

                PrintSongs(songService);
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
            SongService songService,
            Song? testSong)
        {
            _output.WriteInfo("* Delete *");

            try
            {
                if (testSong == null)
                {
                    _output.WriteError(
                        "Delete skipped because the test song was not created.");
                }
                else
                {
                    _output.WriteError(
                        testSong.Id.ToString());

                    _output.WriteError(
                        "Song deleted successfully.");
                }

                PrintSongs(songService);
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

                //UIHelpers.WriteGreen(
                //    $"Main artist changed from Artist {mainArtist.ArtistId} to ArtistId {newMainArtist.ArtistId}.");

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

            var context = new MusicPlatformContext();

            _output.WriteLine(UIHelpers.OLDFormatSong(context, song));

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
                _output.WriteLine( UIHelpers.OLDFormatSong(context, s));
            });

        }
    }
}

