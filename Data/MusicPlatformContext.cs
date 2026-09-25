using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusicPlatform.Models;

namespace MusicPlatform.Data
{
    internal class MusicPlatformContext : DbContext, IMusicPlatformContext
    {
        //properties som repræsenterer en tabel/entity type, som EF Core skal kunne arbejde med
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<AlbumSong> AlbumSongs { get; set; }
        public DbSet<SongArtist> SongArtists { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<MediaType> MediaTypes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<MusicPlatformContext>()
                .Build();

            var connectionString =
                configuration.GetConnectionString("MusicPlatform")
                ?? throw new InvalidOperationException(
                    "Connection string 'MusicPlatform' was not found.");

            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");            

            //mapping af DbSet til tabeller: Så hvor C# siger context.Artists (flertal) skal EF Core bruge Artist-tabellen (ental) i databasen
            modelBuilder.Entity<Artist>().ToTable("Artist");
            modelBuilder.Entity<Song>().ToTable("Song");
            modelBuilder.Entity<Album>().ToTable("Album");
            modelBuilder.Entity<SongArtist>().ToTable("SongArtist");
            modelBuilder.Entity<AlbumSong>().ToTable("AlbumSong");
            modelBuilder.Entity<Media>().ToTable("Media");
            modelBuilder.Entity<MediaType>().ToTable("MediaType");

            //fortæller EF Core, at SongArtists SongId+ArtistId skal betragtes som én sammensat primary key
            //svarer til SQL: PRIMARY KEY (SongId, ArtistId)
            modelBuilder.Entity<SongArtist>()
                .HasKey(sa => new { sa.SongId, sa.ArtistId});

            //AlbumSong har sammensat primary key: PRIMARY KEY (AlbumId, SongId)
            modelBuilder.Entity<AlbumSong>()
                .HasKey(als => new { als.AlbumId, als.SongId });


            //Artist 1----M SongArtist M----1 Song

            //SongArtist kobles til Song
            modelBuilder.Entity<SongArtist>()
                .HasOne<Song>()
                .WithMany()
                .HasForeignKey(sa => sa.SongId);

            //SongArtist kobles til Artist
            modelBuilder.Entity<SongArtist>()
                .HasOne<Artist>()
                .WithMany()
                .HasForeignKey(sa => sa.ArtistId);


            //Album 1 ----M AlbumSong M----1 Song

            //AlbumSong kobles til Album
            modelBuilder.Entity<AlbumSong>()
                .HasOne<Album>() //en AlbumSong peger på ét Album
                .WithMany() //et Album kan have mange AlbumSong
                .HasForeignKey(als => als.AlbumId);

            //AlbumSong kobles til Song: Song 1 --- M AlbumSong 
            modelBuilder.Entity<AlbumSong>()
                .HasOne<Song>()
                .WithMany() //Song kan have mange AlbumSong aka. kan optræde på flere albums
                .HasForeignKey(als => als.SongId);

            //Song 1----M Media

            //Media kobles til song
            modelBuilder.Entity<Media>()
                .HasOne<Song>() //Media tilhøører én Song
                .WithMany() //Song kan have mange Media
                .HasForeignKey(m => m.SongId);

            //MediaType 1----M Media

            //Media kobles til MediaType
            modelBuilder.Entity<Media>()
                .HasOne<MediaType>()
                .WithMany()
                .HasForeignKey(m => m.MediaTypeId);
        }
    }
}
