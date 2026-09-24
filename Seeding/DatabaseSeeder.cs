using MusicPlatform.Application.Models;
using MusicPlatform.Data;
using MusicPlatform.Models;
using MusicPlatform.UI;

namespace MusicPlatform.Seeding
{
    internal class DatabaseSeeder
    {
        private readonly IOutput _output = new Output();
        internal void Seed()
        {
            using var context = new MusicPlatformContext();

            using var transaction = context.Database.BeginTransaction();

            if (context.Artists.Any())
            {
                _output.WriteInfo("Database already contains seed data.");
                return;
            }

            //artists
            Artist teddySwims = new Artist() { Name = "Teddy Swims" };
            Artist sabrinaCarpenter = new Artist() { Name = "Sabrina Carpenter" };
            Artist billieEilish = new Artist() { Name = "Billie Eilish" };
            Artist sleepToken = new Artist() { Name = "Sleep Token" };
            Artist bishopBriggs = new Artist() { Name = "Bishop Briggs" };
            Artist vuvuvultures = new Artist() { Name = "Vuvuvultures" };
            Artist augustHoyen = new Artist() { Name = "August Høyen" };
            Artist d1ma = new Artist() { Name = "D1MA" };
            Artist syl = new Artist() { Name = "SYL" };
            Artist witchClubSatan = new Artist() { Name = "Witch Club Satan" };
            Artist theMarsVolta = new Artist() { Name = "The Mars Volta" };
            Artist ledZeppelin = new Artist() { Name = "Led Zeppelin" };
            Artist zealAndArdor = new Artist() { Name = "ZEAL & ARDOR" };
            Artist iWouldSetMyselfOnFireForYou = new Artist() { Name = "I Would Set Myself On Fire For You" };
            Artist shitKid = new Artist() { Name = "ShitKid" };
            Artist nirvana = new Artist() { Name = "Nirvana" };
            Artist hozier = new Artist() { Name = "Hozier" };
            Artist benHoward = new Artist() { Name = "Ben Howard" };
            Artist gregoryAlanIsakov = new Artist() { Name = "Gregory Alan Isakov" };
            Artist hollowCoves = new Artist() { Name = "Hollow Coves" };

            context.Artists.AddRange(
                teddySwims,
                sabrinaCarpenter,
                billieEilish,
                sleepToken,
                bishopBriggs,
                vuvuvultures,
                augustHoyen,
                d1ma,
                syl,
                witchClubSatan,
                theMarsVolta,
                ledZeppelin,
                zealAndArdor,
                iWouldSetMyselfOnFireForYou,
                shitKid,
                nirvana,
                hozier,
                benHoward,
                gregoryAlanIsakov,
                hollowCoves
                );
            context.SaveChanges();

            //songs
            Song song1 = new Song() { Title = "Lose Control" };
            Song song2 = new Song() { Title = "Please Please Please" };
            Song song3 = new Song() { Title = "BIRDS OF A FEATHER" };
            Song song4 = new Song() { Title = "Emergence" };
            Song song5 = new Song() { Title = "River" };
            Song song6 = new Song() { Title = "Your Thoughts Are A Plague (Mahogany Sessions)" };
            Song song7 = new Song() { Title = "Vennerne Skinner" };
            Song song8 = new Song() { Title = "BRÆNDER" };
            Song song9 = new Song() { Title = "Mother Sea" };
            Song song10 = new Song() { Title = "Vicarious Atonement" };
            Song song11 = new Song() { Title = "Babe I'm Gonna Leave You (Remaster)" };
            Song song12 = new Song() { Title = "Devil Is Fine" };
            Song song13 = new Song() { Title = "Eleven" };
            Song song14 = new Song() { Title = "Sugar Town" };
            Song song15 = new Song() { Title = "Black Dog (Remaster)" };
            Song song16 = new Song() { Title = "Where Did You Sleep Last Night" };
            Song song17 = new Song() { Title = "Something In The Way" };
            Song song18 = new Song() { Title = "Cherry Wine" };
            Song song19 = new Song() { Title = "Depth Over Distance Live on KCRW" };
            Song song20 = new Song() { Title = "San Luis" };
            Song song21 = new Song() { Title = "Coastline (Lakeside Acoustic Session)" };
            Song song22 = new Song() { Title = "Gethsemane" };
            Song song23 = new Song() { Title = "Take Me Back To Eden" };
            Song song24 = new Song() { Title = "Alkaline" };
            Song song25 = new Song() { Title = "Damocles" };
            Song song26 = new Song() { Title = "Chokehold" };
            Song song27 = new Song() { Title = "Past Self" };

            context.Songs.AddRange(
                song1,
                song2,
                song3,
                song4,
                song5,
                song6,
                song7,
                song8,
                song9,
                song10,
                song11,
                song12,
                song13,
                song14,
                song15,
                song16,
                song17,
                song18,
                song19,
                song20,
                song21,
                song22,
                song23,
                song24,
                song25,
                song26,
                song27
                );
            context.SaveChanges();

            //albums
            Album album1 = new Album()
            {
                Title = "I've Tried Everything But Therapy (Part 1)",
                ReleaseDate = new DateOnly(2023, 9, 15)
            };

            Album album2 = new Album()
            {
                Title = "Short n' Sweet",
                ReleaseDate = new DateOnly(2024, 8, 23)
            };

            Album album3 = new Album()
            {
                Title = "HIT ME HARD AND SOFT",
                ReleaseDate = new DateOnly(2024, 5, 17)
            };

            Album album4 = new Album()
            {
                Title = "Even In Arcadia",
                ReleaseDate = new DateOnly(2025, 5, 9)
            };

            Album album5 = new Album()
            {
                Title = "Church Of Scars",
                ReleaseDate = new DateOnly(2018, 4, 20)
            };

            Album album6 = new Album()
            {
                Title = "N1YA",
                ReleaseDate = new DateOnly(2025, 7, 3)
            };

            Album album7 = new Album()
            {
                Title = "Witch Club Satan",
                ReleaseDate = new DateOnly(2024, 3, 8)
            };

            Album album8 = new Album()
            {
                Title = "Amputechture",
                ReleaseDate = new DateOnly(2006, 9, 12)
            };

            Album album9 = new Album()
            {
                Title = "Led Zeppelin",
                ReleaseDate = new DateOnly(1969, 1, 12)
            };

            Album album10 = new Album()
            {
                Title = "Devil Is Fine",
                ReleaseDate = new DateOnly(2017, 2, 24)
            };

            Album album11 = new Album()
            {
                Title = "Fish",
                ReleaseDate = new DateOnly(2017, 6, 2)
            };

            Album album12 = new Album()
            {
                Title = "Led Zeppelin IV",
                ReleaseDate = new DateOnly(1971, 11, 8)
            };

            Album album13 = new Album()
            {
                Title = "MTV Unplugged in New York",
                ReleaseDate = new DateOnly(1994, 11, 1)
            };

            Album album14 = new Album()
            {
                Title = "Nevermind",
                ReleaseDate = new DateOnly(1991, 9, 24)
            };

            Album album15 = new Album()
            {
                Title = "Hozier",
                ReleaseDate = new DateOnly(2014, 10, 7)
            };

            Album album16 = new Album()
            {
                Title = "Evening Machines",
                ReleaseDate = new DateOnly(2018, 10, 5)
            };

            Album album17 = new Album()
            {
                Title = "Take Me Back To Eden",
                ReleaseDate = new DateOnly(2023, 5, 19)
            };

            Album album18 = new Album()
            {
                Title = "This Place Will Become Your Tomb",
                ReleaseDate = new DateOnly(2021, 9, 24)
            };
            Album album19 = new Album()
            {
                Title = "ten/eleven",
                ReleaseDate = new DateOnly(2006, 6, 6)
            };

            context.Albums.AddRange(
                album1,
                album2,
                album3,
                album4,
                album5,
                album6,
                album7,
                album8,
                album9,
                album10,
                album11,
                album12,
                album13,
                album14,
                album15,
                album16,
                album17,
                album18,
                album19
            );

            context.SaveChanges();


            //album songs
            AlbumSong albumSong1 = new AlbumSong()
            {
                AlbumId = album1.Id,
                SongId = song1.Id
            };

            AlbumSong albumSong2 = new AlbumSong()
            {
                AlbumId = album2.Id,
                SongId = song2.Id
            };

            AlbumSong albumSong3 = new AlbumSong()
            {
                AlbumId = album3.Id,
                SongId = song3.Id
            };

            AlbumSong albumSong4 = new AlbumSong()
            {
                AlbumId = album4.Id,
                SongId = song4.Id
            };

            AlbumSong albumSong5 = new AlbumSong()
            {
                AlbumId = album5.Id,
                SongId = song5.Id
            };

           
            AlbumSong albumSong6 = new AlbumSong()
            {
                AlbumId = album6.Id,
                SongId = song8.Id
            };

            AlbumSong albumSong7 = new AlbumSong()
            {
                AlbumId = album7.Id,
                SongId = song9.Id
            };

            AlbumSong albumSong8 = new AlbumSong()
            {
                AlbumId = album8.Id,
                SongId = song10.Id
            };

            AlbumSong albumSong9 = new AlbumSong()
            {
                AlbumId = album9.Id,
                SongId = song11.Id
            };

            AlbumSong albumSong10 = new AlbumSong()
            {
                AlbumId = album10.Id,
                SongId = song12.Id
            };


            AlbumSong albumSong11 = new AlbumSong()
            {
                AlbumId = album11.Id,
                SongId = song14.Id
            };

            AlbumSong albumSong12 = new AlbumSong()
            {
                AlbumId = album12.Id,
                SongId = song15.Id
            };

            AlbumSong albumSong13 = new AlbumSong()
            {
                AlbumId = album13.Id,
                SongId = song16.Id
            };

                       AlbumSong albumSong14 = new AlbumSong()
            {
                AlbumId = album13.Id,
                SongId = song17.Id
            };

            AlbumSong albumSong15 = new AlbumSong()
            {
                AlbumId = album15.Id,
                SongId = song18.Id
            };

            AlbumSong albumSong16 = new AlbumSong()
            {
                AlbumId = album16.Id,
                SongId = song20.Id
            };

            AlbumSong albumSong17 = new AlbumSong()
            {
                AlbumId = album4.Id,
                SongId = song22.Id
            };

            AlbumSong albumSong18 = new AlbumSong()
            {
                AlbumId = album17.Id,
                SongId = song23.Id
            };

            AlbumSong albumSong19 = new AlbumSong()
            {
                AlbumId = album18.Id,
                SongId = song24.Id
            };

            AlbumSong albumSong20 = new AlbumSong()
            {
                AlbumId = album4.Id,
                SongId = song25.Id
            };

            AlbumSong albumSong21 = new AlbumSong()
            {
                AlbumId = album17.Id,
                SongId = song26.Id
            };

            AlbumSong albumSong22 = new AlbumSong()
            {
                AlbumId = album4.Id,
                SongId = song27.Id
            };
            AlbumSong albumSong23 = new AlbumSong()
            {
                AlbumId = album19.Id,
                SongId = song13.Id
            };


            context.AlbumSongs.AddRange(
                albumSong1,
                albumSong2,
                albumSong3,
                albumSong4,
                albumSong5,
                albumSong6,
                albumSong7,
                albumSong8,
                albumSong9,
                albumSong10,
                albumSong11,
                albumSong12,
                albumSong13,
                albumSong14,
                albumSong15,
                albumSong16,
                albumSong17,
                albumSong18,
                albumSong19,
                albumSong20,
                albumSong21,
                albumSong22,
                albumSong23
            );

            context.SaveChanges();


            //MediaType
            MediaType youtube = new MediaType() { Name = MediaTypeName.YouTube.ToString() };
            context.MediaTypes.Add(youtube);
            context.SaveChanges();

            //Media
            Media media1 = new Media() { ExternalId = "XpDEdMu_6O8", SongId = song1.Id, MediaTypeId = youtube.Id };
            Media media2 = new Media() { ExternalId = "cF1Na4AIecM", SongId = song2.Id, MediaTypeId = youtube.Id };
            Media media3 = new Media() { ExternalId = "V9PVRfjEBTI", SongId = song3.Id, MediaTypeId = youtube.Id };
            Media media4 = new Media() { ExternalId = "JJpFTUP6fIo", SongId = song4.Id, MediaTypeId = youtube.Id };
            Media media5 = new Media() { ExternalId = "h5jz8xdpR0M", SongId = song5.Id, MediaTypeId = youtube.Id };
            Media media6 = new Media() { ExternalId = "tdpohV-WWJE", SongId = song6.Id, MediaTypeId = youtube.Id };
            Media media7 = new Media() { ExternalId = "ex9YNLci0ns", SongId = song7.Id, MediaTypeId = youtube.Id };
            Media media8 = new Media() { ExternalId = "i7iSxBR_ArE", SongId = song8.Id, MediaTypeId = youtube.Id };
            Media media9 = new Media() { ExternalId = "jLN_qGrvcZg", SongId = song9.Id, MediaTypeId = youtube.Id };
            Media media10 = new Media() { ExternalId = "hhzM3VhM2yA", SongId = song10.Id, MediaTypeId = youtube.Id };
            Media media11 = new Media() { ExternalId = "dZitPJMh60A", SongId = song11.Id, MediaTypeId = youtube.Id };
            Media media12 = new Media() { ExternalId = "jlGBer0VoF8", SongId = song12.Id, MediaTypeId = youtube.Id };
            Media media13 = new Media() { ExternalId = "r7O2lyMFIAA", SongId = song13.Id, MediaTypeId = youtube.Id };
            Media media14 = new Media() { ExternalId = "ceyb0K3YO1s", SongId = song14.Id, MediaTypeId = youtube.Id };
            Media media15 = new Media() { ExternalId = "yBuub4Xe1mw", SongId = song15.Id, MediaTypeId = youtube.Id };
            Media media16 = new Media() { ExternalId = "hEMm7gxBYSc", SongId = song16.Id, MediaTypeId = youtube.Id };
            Media media17 = new Media() { ExternalId = "1YhR5UfaAzM", SongId = song17.Id, MediaTypeId = youtube.Id };
            Media media18 = new Media() { ExternalId = "SdSCCwtNEjA", SongId = song18.Id, MediaTypeId = youtube.Id };
            Media media19 = new Media() { ExternalId = "phktiVZqUbQ", SongId = song19.Id, MediaTypeId = youtube.Id };
            Media media20 = new Media() { ExternalId = "7BJ7MDOmLPE", SongId = song20.Id, MediaTypeId = youtube.Id };
            Media media21 = new Media() { ExternalId = "Cp-1t9B62zc", SongId = song21.Id, MediaTypeId = youtube.Id };
            Media media22 = new Media() { ExternalId = "EuwLeCs9z2o", SongId = song22.Id, MediaTypeId = youtube.Id };
            Media media23 = new Media() { ExternalId = "vFHBOKa_ZG0", SongId = song23.Id, MediaTypeId = youtube.Id };
            Media media24 = new Media() { ExternalId = "uU5vVT_Cp7c", SongId = song24.Id, MediaTypeId = youtube.Id };
            Media media25 = new Media() { ExternalId = "0NDqYZVbpho", SongId = song25.Id, MediaTypeId = youtube.Id };
            Media media26 = new Media() { ExternalId = "-UUSUrr6zyo", SongId = song26.Id, MediaTypeId = youtube.Id };
            Media media27 = new Media() { ExternalId = "GSkY_xmkoYg", SongId = song27.Id, MediaTypeId = youtube.Id };


            context.Media.AddRange(
                media1,
                media2,
                media3,
                media4,
                media5,
                media6,
                media7,
                media8,
                media9,
                media10,
                media11,
                media12,
                media13,
                media14,
                media15,
                media16,
                media17,
                media18,
                media19,
                media20,
                media21,
                media22,
                media23,
                media24,
                media25,
                media26,
                media27
            );
            context.SaveChanges();

            //SongArtist
            SongArtist songArtist1 = new SongArtist() { SongId = song1.Id, ArtistId = teddySwims.Id, IsMainArtist = true };
            SongArtist songArtist2 = new SongArtist() { SongId = song2.Id, ArtistId = sabrinaCarpenter.Id, IsMainArtist = true };
            SongArtist songArtist3 = new SongArtist() { SongId = song3.Id, ArtistId = billieEilish.Id, IsMainArtist = true };
            SongArtist songArtist4 = new SongArtist() { SongId = song4.Id, ArtistId = sleepToken.Id, IsMainArtist = true };
            SongArtist songArtist5 = new SongArtist() { SongId = song5.Id, ArtistId = bishopBriggs.Id, IsMainArtist = true };
            SongArtist songArtist6 = new SongArtist() { SongId = song6.Id, ArtistId = vuvuvultures.Id, IsMainArtist = true };
            SongArtist songArtist7 = new SongArtist() { SongId = song7.Id, ArtistId = augustHoyen.Id, IsMainArtist = true };
            SongArtist songArtist8 = new SongArtist() { SongId = song8.Id, ArtistId = d1ma.Id, IsMainArtist = true };
            SongArtist songArtist9 = new SongArtist() { SongId = song9.Id, ArtistId = witchClubSatan.Id, IsMainArtist = true };
            SongArtist songArtist10 = new SongArtist() { SongId = song10.Id, ArtistId = theMarsVolta.Id, IsMainArtist = true };
            SongArtist songArtist11 = new SongArtist() { SongId = song11.Id, ArtistId = ledZeppelin.Id, IsMainArtist = true };
            SongArtist songArtist12 = new SongArtist() { SongId = song12.Id, ArtistId = zealAndArdor.Id, IsMainArtist = true };
            SongArtist songArtist13 = new SongArtist() { SongId = song13.Id, ArtistId = iWouldSetMyselfOnFireForYou.Id, IsMainArtist = true };
            SongArtist songArtist14 = new SongArtist() { SongId = song14.Id, ArtistId = shitKid.Id, IsMainArtist = true };
            SongArtist songArtist15 = new SongArtist() { SongId = song15.Id, ArtistId = ledZeppelin.Id, IsMainArtist = true };
            SongArtist songArtist16 = new SongArtist() { SongId = song16.Id, ArtistId = nirvana.Id, IsMainArtist = true };
            SongArtist songArtist17 = new SongArtist() { SongId = song17.Id, ArtistId = nirvana.Id, IsMainArtist = true };
            SongArtist songArtist18 = new SongArtist() { SongId = song18.Id, ArtistId = hozier.Id, IsMainArtist = true };
            SongArtist songArtist19 = new SongArtist() { SongId = song19.Id, ArtistId = benHoward.Id, IsMainArtist = true };
            SongArtist songArtist20 = new SongArtist() { SongId = song20.Id, ArtistId = gregoryAlanIsakov.Id, IsMainArtist = true };
            SongArtist songArtist21 = new SongArtist() { SongId = song21.Id, ArtistId = hollowCoves.Id, IsMainArtist = true };
            SongArtist songArtist22 = new SongArtist() { SongId = song22.Id, ArtistId = sleepToken.Id, IsMainArtist = true };
            SongArtist songArtist23 = new SongArtist() { SongId = song23.Id, ArtistId = sleepToken.Id, IsMainArtist = true };
            SongArtist songArtist24 = new SongArtist() { SongId = song24.Id, ArtistId = sleepToken.Id, IsMainArtist = true };
            SongArtist songArtist25 = new SongArtist() { SongId = song25.Id, ArtistId = sleepToken.Id, IsMainArtist = true };
            SongArtist songArtist26 = new SongArtist() { SongId = song26.Id, ArtistId = sleepToken.Id, IsMainArtist = true };
            SongArtist songArtist27 = new SongArtist() { SongId = song27.Id, ArtistId = sleepToken.Id, IsMainArtist = true };
            SongArtist songArtist28 = new SongArtist() { SongId = song8.Id, ArtistId = syl.Id, IsMainArtist = false };

            context.SongArtists.AddRange(
                songArtist1,
                songArtist2,
                songArtist3,
                songArtist4,
                songArtist5,
                songArtist6,
                songArtist7,
                songArtist8,
                songArtist9,
                songArtist10,
                songArtist11,
                songArtist12,
                songArtist13,
                songArtist14,
                songArtist15,
                songArtist16,
                songArtist17,
                songArtist18,
                songArtist19,
                songArtist20,
                songArtist21,
                songArtist22,
                songArtist23,
                songArtist24,
                songArtist25,
                songArtist26,
                songArtist27,
                songArtist28
            );
            context.SaveChanges();
            transaction.Commit();

        }
    }
}
