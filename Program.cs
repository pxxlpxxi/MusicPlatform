using MusicPlatform.Application.Models;
using MusicPlatform.Application.Services;
using MusicPlatform.Data;
using MusicPlatform.Database;
using MusicPlatform.Helpers;
using MusicPlatform.Seeding;
using MusicPlatform.Services;
using MusicPlatform.UI;

IInput input = new Input();
IOutput output = new Output();

DatabaseInitializer databaseInitializer = new(output);

databaseInitializer.Initialize();

using var context = new MusicPlatformContext();

if (context.Database.CanConnect())
{
    output.WriteSuccess("Establishing EF Core connection succeeded.");

}
else
{
    output.WriteError("Establishing EF Core connection failed.");
}




DatabaseSeeder databaseSeeder = new(output, context);
SongService songService = new(context);
ArtistService artistService = new(context);
AlbumService albumService = new(context);
MediaService mediaService = new(context);
SongCreationService songCreationService = new(context, songService, artistService, albumService, mediaService);
SongCreationApplicationService songCreationApplicationService = new(songCreationService, input, output);

databaseSeeder.Seed();



SongInfo songInfo = songService.GetSongInfo(1);
output.WriteLine(UIHelpers.FormatSong(songInfo));

DatabaseTestHelper tester = new(songCreationApplicationService, input, output, songCreationService, songService);

// Create
SongInfo? testSong =
    tester.TestCreate();

//Read
tester.TestRead();

//Update
tester.TestUpdate(
    testSong);

//Delete
tester.TestDelete(
    testSong);

//trigger test
tester.TestMainArtistChange(context);

//test that the first artist automatically becomes main artist
tester.TestFirstArtistBecomesMain(context);

//test that the user's IsMainArtist value is respected when the song already has an artist.
tester.TestMainArtistValue(context);

UserService userService = new(context);

tester.TestCreateUser(userService);

tester.TestUserPermissions(userService);

tester.TestUserValidation(userService);

context.Dispose();
input.ReadKey();
