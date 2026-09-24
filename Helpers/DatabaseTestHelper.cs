using MusicPlatform.Data;
using MusicPlatform.Models;
using MusicPlatform.Services;

namespace MusicPlatform.Helpers
{
    internal static class DatabaseTestHelper
    {
        internal static Song? TestCreate(
            SongCreationService songCreationService,
            SongService songService)
        {
            UIHelpers.WriteBlue("* Create *");

            try
            {
                Song song = songCreationService.AddNewSong(
                    "Test Song",
                    "Test Artist",
                    "Youtube",
                    "test-external-id");

                UIHelpers.WriteGreen(
                    "Song created successfully.");

                PrintSongs(songService);

                return song;
            }
            catch (ArgumentException ex)
            {
                UIHelpers.WriteRed(
                    $"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                UIHelpers.WriteRed(
                    $"Unexpected error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    UIHelpers.WriteRed(
                        $"Database error: {ex.InnerException.Message}");
                }
            }

            return null;
        }

        internal static void TestRead(
            SongService songService)
        {
            UIHelpers.WriteBlue("* Read *");

            try
            {
                List<Song> songs =
                    songService.SearchSongs("Test");

                UIHelpers.WriteGreen("Search results:");

                songs.ForEach(song => {PrintSongDetails(song);});
                //songs.ForEach(
                //    s => Console.WriteLine(
                //        $"{s.Id}: {s.Title}"));
            }
            catch (Exception ex)
            {
                UIHelpers.WriteRed(
                    $"Unexpected error: {ex.Message}");
            }
        }

        internal static void TestUpdate(
            SongService songService,
            Song? testSong)
        {
            UIHelpers.WriteBlue("* Update *");

            try
            {
                if (testSong == null)
                {
                    UIHelpers.WriteRed(
                        "Update skipped because the test song was not created.");
                }
                else
                {
                    songService.UpdateSongTitle(
                        testSong.Id,
                        "Updated Test Song");

                    UIHelpers.WriteGreen(
                        "Song updated successfully.");
                }

                PrintSongs(songService);
            }
            catch (ArgumentException ex)
            {
                UIHelpers.WriteRed(
                    $"Validation error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                UIHelpers.WriteRed(
                    $"Operation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                UIHelpers.WriteRed(
                    $"Unexpected error: {ex.Message}");

                UIHelpers.WriteRed(
                    $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        internal static void TestDelete(
            SongService songService,
            Song? testSong)
        {
            UIHelpers.WriteBlue("* Delete *");

            try
            {
                if (testSong == null)
                {
                    UIHelpers.WriteRed(
                        "Delete skipped because the test song was not created.");
                }
                else
                {
                    songService.DeleteSong(
                        testSong.Id);

                    UIHelpers.WriteGreen(
                        "Song deleted successfully.");
                }

                PrintSongs(songService);
            }
            catch (InvalidOperationException ex)
            {
                UIHelpers.WriteRed(
                    $"Operation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                UIHelpers.WriteRed(
                    $"Unexpected error: {ex.Message}");

                UIHelpers.WriteRed(
                    $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        internal static void TestMainArtistChange(
            MusicPlatformContext context)
        {
            UIHelpers.WriteBlue("* Trigger *");

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

                UIHelpers.WriteGreen(
                    $"Main artist changed from '{oldArtist.Name}' to '{newArtist.Name}'.");

                //UIHelpers.WriteGreen(
                //    $"Main artist changed from Artist {mainArtist.ArtistId} to ArtistId {newMainArtist.ArtistId}.");

                transaction.Rollback();

                UIHelpers.WriteGreen(
                    "Main artist restored.");
            }
            catch (Exception ex)
            {
                UIHelpers.WriteRed(
                    $"Trigger error: {ex.Message}");

                if (ex.InnerException != null)
                {
                    UIHelpers.WriteRed(
                        $"Database error: {ex.InnerException.Message}");
                }
            }
        }

        internal static void TestFirstArtistBecomesMain(
            MusicPlatformContext context)
        {
            UIHelpers.WriteBlue("* Trigger Test *");

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

                UIHelpers.WriteGreen(
                    $"Trigger correctly set '{testArtist.Name}' as main artist for '{triggerTestSong.Title}'.");

                //UIHelpers.WriteGreen(
                //    $"Trigger correctly set ArtistId {testArtist.Id} as main artist for SongId {triggerTestSong.Id}.");

                transaction.Rollback();
            }
            catch (Exception ex)
            {
                UIHelpers.WriteRed(
                    $"Trigger test failed: {ex.Message}");

                if (ex.InnerException != null)
                {
                    UIHelpers.WriteRed(
                        $"Database error: {ex.InnerException.Message}");
                }
            }
        }

        internal static void TestMainArtistValue(
            MusicPlatformContext context)
        {
            UIHelpers.WriteBlue("* Main Artist Value Test *");

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

                UIHelpers.WriteGreen(
                    $"Trigger correctly respected IsMainArtist = false, and kept '{secondArtist.Name}' as non-main artist.");

                //UIHelpers.WriteGreen(
                //    $"Trigger correctly respected IsMainArtist = false, and kept ArtistId {secondArtist.Id} as non-main artist.");

                transaction.Rollback();
            }
            catch (Exception ex)
            {
                UIHelpers.WriteRed(
                    $"Main artist value test failed: {ex.Message}");

                if (ex.InnerException != null)
                {
                    UIHelpers.WriteRed(
                        $"Database error: {ex.InnerException.Message}");
                }
            }
        }
        private static void PrintSongDetails(Song song)
        {
            var context = new MusicPlatformContext();

            Console.WriteLine(UIHelpers.FormatSong(context, song));

        }
        private static void PrintSongs(
            SongService songService)
        {
            var context = new MusicPlatformContext();

            List<Song> songs =
                songService.GetSongs();

            Console.WriteLine("Songs in DB:");
            songs.ForEach(s =>
            {
                Console.WriteLine( UIHelpers.FormatSong(context, s));
            });

        }
    }
}

