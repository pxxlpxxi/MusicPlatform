using MusicPlatform.Application.Models;
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

DatabaseSeeder databaseSeeder = new();

databaseSeeder.Seed();

SongService songService = new(context);
ArtistService artistService = new(context);
AlbumService albumService = new(context);
MediaService mediaService = new(context);
SongInfo songInfo = songService.GetSongInfo(1);

output.Write(UIHelpers.FormatSong(songInfo));
//Console.WriteLine(songInfo.Title);
//Console.WriteLine(songInfo.MainArtist);

//foreach (string artist in songInfo.FeaturedArtists)
//{
//    Console.WriteLine($"Featured: {artist}");
//}

//foreach (AlbumInfo album in songInfo.Albums)
//{
//    Console.WriteLine($"Album: {album.Title}");
//}

//foreach (MediaInfo media in songInfo.Media)
//{
//    Console.WriteLine($"Media: {media.Type} - {media.ExternalId}");
//}

//SongCreationService songCreationService =
//    new(
//        context,
//        songService,
//        artistService,
//        albumService,
//        mediaService);

//// Create
//Song? testSong =
//    DatabaseTestHelper.TestCreate(
//        songCreationService,
//        songService);

////Read
//DatabaseTestHelper.TestRead(songService);

////Update
//DatabaseTestHelper.TestUpdate(
//    songService,
//    testSong);

////Delete
//DatabaseTestHelper.TestDelete(
//    songService,
//    testSong);

////trigger test
//DatabaseTestHelper.TestMainArtistChange(context);

////test that the first artist automatically becomes main artist
//DatabaseTestHelper.TestFirstArtistBecomesMain(context);

////test that the user's IsMainArtist value is respected when the song already has an artist.
//DatabaseTestHelper.TestMainArtistValue(context);

input.ReadKey();
