using Microsoft.EntityFrameworkCore;
using MusicPlatform.Application.Models;
using MusicPlatform.Data;
using MusicPlatform.Models;
using MusicPlatform.UI;

namespace MusicPlatform.Seeding
{
    internal class DatabaseSeeder
    {
        private readonly MusicPlatformContext _context;
        private readonly IOutput _output;
        private readonly List<Song> _songs = [];
        private readonly List<Artist> _artists = [];
        private readonly List<Album> _albums = [];
        private MediaType? _youtube;

        internal DatabaseSeeder(IOutput output, MusicPlatformContext context)
        {
            _context = context;
            _output = output;
        }
        internal void Seed()
        {
            using var context = new MusicPlatformContext();

            using var transaction = context.Database.BeginTransaction();

            //artists
            SeedArtists();
            context.SaveChanges();

            //songs
            SeedSongs();

            //albums
            SeedAlbums();
            context.SaveChanges();


            //album songs
            SeedAlbumSongs();

            //MediaType
            SeedMediaTypes();

            //Media
            SeedMedia();

            //SongArtist
            SeedSongArtists();

            //Admin-user
            SeedAdmin();

            context.SaveChanges();
            transaction.Commit();

        }

        private void SeedAlbums()
        {
            if (_context.Albums.Any())
            {
                _output.WriteInfo("Database already contains AlbumSong data.");
                return;
            }
            _albums.AddRange(
                new Album()
                {
                    Title = "I've Tried Everything But Therapy (Part 1)",
                    ReleaseDate = new DateOnly(2023, 9, 15)
                },
                new Album()
                {
                    Title = "Short n' Sweet",
                    ReleaseDate = new DateOnly(2024, 8, 23)
                },
                new Album()
                {
                    Title = "HIT ME HARD AND SOFT",
                    ReleaseDate = new DateOnly(2024, 5, 17)
                },
                new Album()
                {
                    Title = "Even In Arcadia",
                    ReleaseDate = new DateOnly(2025, 5, 9)
                },
                new Album()
                {
                    Title = "Church Of Scars",
                    ReleaseDate = new DateOnly(2018, 4, 20)
                },
                new Album()
                {
                    Title = "N1YA",
                    ReleaseDate = new DateOnly(2025, 7, 3)
                },
                new Album()
                {
                    Title = "Witch Club Satan",
                    ReleaseDate = new DateOnly(2024, 3, 8)
                },
                new Album()
                {
                    Title = "Amputechture",
                    ReleaseDate = new DateOnly(2006, 9, 12)
                },
                new Album()
                {
                    Title = "Led Zeppelin",
                    ReleaseDate = new DateOnly(1969, 1, 12)
                },
                new Album()
                {
                    Title = "Devil Is Fine",
                    ReleaseDate = new DateOnly(2017, 2, 24)
                },
                new Album()
                {
                    Title = "Fish",
                    ReleaseDate = new DateOnly(2017, 6, 2)
                },
                new Album()
                {
                    Title = "Led Zeppelin IV",
                    ReleaseDate = new DateOnly(1971, 11, 8)
                },
                new Album()
                {
                    Title = "MTV Unplugged in New York",
                    ReleaseDate = new DateOnly(1994, 11, 1)
                },
                new Album()
                {
                    Title = "Nevermind",
                    ReleaseDate = new DateOnly(1991, 9, 24)
                },
                new Album()
                {
                    Title = "Hozier",
                    ReleaseDate = new DateOnly(2014, 10, 7)
                },
                new Album()
                {
                    Title = "Evening Machines",
                    ReleaseDate = new DateOnly(2018, 10, 5)
                },
                new Album()
                {
                    Title = "Take Me Back To Eden",
                    ReleaseDate = new DateOnly(2023, 5, 19)
                },
                new Album()
                {
                    Title = "This Place Will Become Your Tomb",
                    ReleaseDate = new DateOnly(2021, 9, 24)
                },
                new Album()
                {
                    Title = "ten/eleven",
                    ReleaseDate = new DateOnly(2006, 6, 6)
                }
            );
            _context.Albums.AddRange(_albums);

            _context.SaveChanges();
            _output.WriteSuccess("Album data added.");
        }
        private void SeedAlbumSongs()
        {
            if (_context.AlbumSongs.Any())
            {
                _output.WriteInfo("Database already contains AlbumSong data.");
                return;
            }
            _context.AlbumSongs.AddRange(
                new AlbumSong
                {
                    AlbumId = _albums[0].Id,
                    SongId = _songs[0].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[1].Id,
                    SongId = _songs[1].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[2].Id,
                    SongId = _songs[2].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[3].Id,
                    SongId = _songs[3].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[4].Id,
                    SongId = _songs[4].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[5].Id,
                    SongId = _songs[7].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[6].Id,
                    SongId = _songs[8].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[7].Id,
                    SongId = _songs[9].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[8].Id,
                    SongId = _songs[10].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[9].Id,
                    SongId = _songs[11].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[10].Id,
                    SongId = _songs[13].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[11].Id,
                    SongId = _songs[14].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[12].Id,
                    SongId = _songs[15].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[12].Id,
                    SongId = _songs[16].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[14].Id,
                    SongId = _songs[17].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[15].Id,
                    SongId = _songs[19].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[3].Id,
                    SongId = _songs[21].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[16].Id,
                    SongId = _songs[22].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[17].Id,
                    SongId = _songs[23].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[3].Id,
                    SongId = _songs[24].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[16].Id,
                    SongId = _songs[25].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[3].Id,
                    SongId = _songs[26].Id
                },
                new AlbumSong
                {
                    AlbumId = _albums[18].Id,
                    SongId = _songs[12].Id
                }
            );
            _context.SaveChanges();
            _output.WriteSuccess("AlbumSongData added.");
                    }


        private void SeedArtists()
        {
            if (_context.Artists.Any())
            {
                _output.WriteInfo("Database already contains Artist data.");
                return;
            }

            _artists.AddRange(
                new Artist() { Name = "Teddy Swims" },
                new Artist() { Name = "Sabrina Carpenter" },
                new Artist() { Name = "Billie Eilish" },
                new Artist() { Name = "Sleep Token" },
                new Artist() { Name = "Bishop Briggs" },
                new Artist() { Name = "Vuvuvultures" },
                new Artist() { Name = "August Høyen" },
                new Artist() { Name = "D1MA" },
                new Artist() { Name = "SYL" },
                new Artist() { Name = "Witch Club Satan" },
                new Artist() { Name = "The Mars Volta" },
                new Artist() { Name = "Led Zeppelin" },
                new Artist() { Name = "ZEAL & ARDOR" },
                new Artist() { Name = "I Would Set Myself On Fire For You" },
                new Artist() { Name = "ShitKid" },
                new Artist() { Name = "Nirvana" },
                new Artist() { Name = "Hozier" },
                new Artist() { Name = "Ben Howard" },
                new Artist() { Name = "Gregory Alan Isakov" },
                new Artist() { Name = "Hollow Coves" }
            );
            _context.Artists.AddRange(_artists);

            _context.SaveChanges();
            _output.WriteSuccess("Artist data added.");
        }
    

        private void SeedSongArtists()
        {
            if (_context.SongArtists.Any())
            {
                _output.WriteInfo("Database already contains song data.");
                return;
            }
            _context.SongArtists.AddRange(
                new SongArtist
                {
                    SongId = _songs[0].Id,
                    ArtistId = _artists[0].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[1].Id,
                    ArtistId = _artists[1].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[2].Id,
                    ArtistId = _artists[2].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[3].Id,
                    ArtistId = _artists[3].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[4].Id,
                    ArtistId = _artists[4].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[5].Id,
                    ArtistId = _artists[5].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[6].Id,
                    ArtistId = _artists[6].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[7].Id,
                    ArtistId = _artists[7].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[8].Id,
                    ArtistId = _artists[9].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[9].Id,
                    ArtistId = _artists[10].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[10].Id,
                    ArtistId = _artists[11].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[11].Id,
                    ArtistId = _artists[12].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[12].Id,
                    ArtistId = _artists[13].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[13].Id,
                    ArtistId = _artists[14].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[14].Id,
                    ArtistId = _artists[11].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[15].Id,
                    ArtistId = _artists[15].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[16].Id,
                    ArtistId = _artists[15].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[17].Id,
                    ArtistId = _artists[16].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[18].Id,
                    ArtistId = _artists[17].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[19].Id,
                    ArtistId = _artists[18].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[20].Id,
                    ArtistId = _artists[19].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[21].Id,
                    ArtistId = _artists[3].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[22].Id,
                    ArtistId = _artists[3].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[23].Id,
                    ArtistId = _artists[3].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[24].Id,
                    ArtistId = _artists[3].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[25].Id,
                    ArtistId = _artists[3].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[26].Id,
                    ArtistId = _artists[3].Id,
                    IsMainArtist = true
                },
                new SongArtist
                {
                    SongId = _songs[7].Id,
                    ArtistId = _artists[8].Id,
                    IsMainArtist = false
                }
            );
            _context.SaveChanges();
            _output.WriteSuccess("SongArtists added.");
        }

        private void SeedSongs()
        {
            if (_context.Songs.Any())
            {
                _output.WriteInfo("Database already contains song data.");
                return;
            }
            _songs.AddRange(
                new Song() { Title = "Lose Control" },
                new Song() { Title = "Please Please Please" },
                new Song() { Title = "BIRDS OF A FEATHER" },
                new Song() { Title = "Emergence" },
                new Song() { Title = "River" },
                new Song() { Title = "Your Thoughts Are A Plague (Mahogany Sessions)" },
                new Song() { Title = "Vennerne Skinner" },
                new Song() { Title = "BRÆNDER" },
                new Song() { Title = "Mother Sea" },
                new Song() { Title = "Vicarious Atonement" },
                new Song() { Title = "Babe I'm Gonna Leave You (Remaster)" },
                new Song() { Title = "Devil Is Fine" },
                new Song() { Title = "Eleven" },
                new Song() { Title = "Sugar Town" },
                new Song() { Title = "Black Dog (Remaster)" },
                new Song() { Title = "Where Did You Sleep Last Night" },
                new Song() { Title = "Something In The Way" },
                new Song() { Title = "Cherry Wine" },
                new Song() { Title = "Depth Over Distance Live on KCRW" },
                new Song() { Title = "San Luis" },
                new Song() { Title = "Coastline (Lakeside Acoustic Session)" },
                new Song() { Title = "Gethsemane" },
                new Song() { Title = "Take Me Back To Eden" },
                new Song() { Title = "Alkaline" },
                new Song() { Title = "Damocles" },
                new Song() { Title = "Chokehold" },
                new Song() { Title = "Past Self" }
            );
            _context.Songs.AddRange(_songs);

            _context.SaveChanges();
            _output.WriteSuccess("SongArtists added.");
        }
        private void SeedMediaTypes()
        {
            if (_context.MediaTypes.Any())
            {
                _output.WriteInfo("Database already contains MediaType data");
                return;
            }
            _youtube = new MediaType() { Name = MediaTypeName.YouTube.ToString() };
            _context.MediaTypes.Add(_youtube);

            _context.SaveChanges();
            _output.WriteSuccess("MediaType data added.");
        }

        private void SeedMedia()
        {
            if (_context.Media.Any())
            {
                _output.WriteInfo("Database already contains Media data.");
                return;
            }

            _context.Media.AddRange(
                new Media
                {
                    ExternalId = "XpDEdMu_6O8",
                    SongId = _songs[0].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "cF1Na4AIecM",
                    SongId = _songs[1].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "V9PVRfjEBTI",
                    SongId = _songs[2].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "JJpFTUP6fIo",
                    SongId = _songs[3].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "h5jz8xdpR0M",
                    SongId = _songs[4].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "tdpohV-WWJE",
                    SongId = _songs[5].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "ex9YNLci0ns",
                    SongId = _songs[6].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "i7iSxBR_ArE",
                    SongId = _songs[7].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "jLN_qGrvcZg",
                    SongId = _songs[8].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "hhzM3VhM2yA",
                    SongId = _songs[9].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "dZitPJMh60A",
                    SongId = _songs[10].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "jlGBer0VoF8",
                    SongId = _songs[11].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "r7O2lyMFIAA",
                    SongId = _songs[12].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "ceyb0K3YO1s",
                    SongId = _songs[13].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "yBuub4Xe1mw",
                    SongId = _songs[14].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "hEMm7gxBYSc",
                    SongId = _songs[15].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "1YhR5UfaAzM",
                    SongId = _songs[16].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "SdSCCwtNEjA",
                    SongId = _songs[17].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "phktiVZqUbQ",
                    SongId = _songs[18].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "7BJ7MDOmLPE",
                    SongId = _songs[19].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "Cp-1t9B62zc",
                    SongId = _songs[20].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "EuwLeCs9z2o",
                    SongId = _songs[21].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "vFHBOKa_ZG0",
                    SongId = _songs[22].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "uU5vVT_Cp7c",
                    SongId = _songs[23].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "0NDqYZVbpho",
                    SongId = _songs[24].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "-UUSUrr6zyo",
                    SongId = _songs[25].Id,
                    MediaTypeId = _youtube.Id
                },
                new Media
                {
                    ExternalId = "GSkY_xmkoYg",
                    SongId = _songs[26].Id,
                    MediaTypeId = _youtube.Id
                }
            );
            _context.SaveChanges();
            _output.WriteSuccess("Media added.");
        }

        private void SeedAdmin()
        {
            if (_context.Users.Any())
            {
                _output.WriteInfo("Database already contains admin.");
                return;
            }

            _context.Users.Add(new User
            {
                Username = "admin",
                Password = "admin123",
                Role = "Admin"
            });


            _context.SaveChanges();
            _output.WriteSuccess("Admin added.");
        }

    }
}
