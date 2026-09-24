using MusicPlatform.Data;
using MusicPlatform.Database;
using MusicPlatform.Helpers;
using MusicPlatform.Models;
using MusicPlatform.Seeding;
using MusicPlatform.Services;

DatabaseInitializer databaseInitializer = new DatabaseInitializer();

databaseInitializer.Initialize();

using var context = new MusicPlatformContext();

if (context.Database.CanConnect())
{
    UIHelpers.WriteGreen("EF Core forbindelse oprettet.");
}
else
{
    UIHelpers.WriteRed("EF Core kunne ikke forbinde til databasen");
}

DatabaseSeeder databaseSeeder = new();

databaseSeeder.Seed();

SongService songService = new(context);
ArtistService artistService = new(context);
AlbumService albumService = new(context);
MediaService mediaService = new(context);

SongCreationService songCreationService =
    new(
        context,
        songService,
        artistService,
        albumService,
        mediaService);

// Create
Song? testSong =
    DatabaseTestHelper.TestCreate(
        songCreationService,
        songService);

//Read
DatabaseTestHelper.TestRead(songService);

//Update
DatabaseTestHelper.TestUpdate(
    songService,
    testSong);

//Delete
DatabaseTestHelper.TestDelete(
    songService,
    testSong);

//trigger test
DatabaseTestHelper.TestMainArtistChange(context);

//test that the first artist automatically becomes main artist
DatabaseTestHelper.TestFirstArtistBecomesMain(context);

//test that the user's IsMainArtist value is respected when the song already has an artist.
DatabaseTestHelper.TestMainArtistValue(context);

Console.ReadKey();