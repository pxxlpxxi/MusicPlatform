using Microsoft.Extensions.Configuration;
using Npgsql;

namespace MusicPlatform.Database
{
    internal class DatabaseInitializer
    {
        private readonly string _postgresConnectionString;
        private readonly string _musicPlatformConnectionString;

        public DatabaseInitializer()
        {
            //henter user secrets
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<DatabaseInitializer>()
                .Build();

            //henter connection string til postgres-database fra user secrets
            _postgresConnectionString =
                configuration.GetConnectionString("Postgres")
                ?? throw new IndexOutOfRangeException("Connection string 'Postgres' was not found");

            //henter connection string til music platform-database fra user secrets
            _musicPlatformConnectionString =
                configuration.GetConnectionString("MusicPlatform")
                ?? throw new IndexOutOfRangeException("Connection string 'MusicPlatform' was not found");
        }

        internal void Initialize() 
        {
            CreateDatabase();
            CreateSchema();
            CreateTriggers();
        }

        private void CreateDatabase() 
        {
            //opretter forbindelse til postgres-serveren
            using var connection = new NpgsqlConnection(_postgresConnectionString);

            connection.Open();

            //opretter kommando til at tjekke, om databasen allerede eksisterer + opretter forbindelse
            using var command = new NpgsqlCommand(
                "SELECT 1 FROM pg_database WHERE datname = 'music_platform';",
                connection);

            //kører kommando + oversætter resultatet til en bool baseret på, om databasen findes eller ej
            var databaseExists = command.ExecuteScalar() != null;

            if (!databaseExists)
            {
                //læser script fra sql fil
                string sql = File.ReadAllText("Database/create_database.sql");

                //opretter en sql commando med sql scriptet og forbindelsen
                using var createCommand = new NpgsqlCommand(sql, connection);

                //kører create database fra sql filen
                createCommand.ExecuteNonQuery();

                Console.WriteLine($"Database created.");
            }
            else {
                Console.WriteLine("Database already exists.");
            }

           
        }

        private void CreateSchema()
        {
            //læser sql scriptet til sql-schema (tabellerne) 
            string sql = File.ReadAllText("Database/schema.sql");

            //opretter forbindelse til music platform
            using var connection = new NpgsqlConnection(_musicPlatformConnectionString);

            connection.Open();

            //opretter en sql-kommando med schema og forbindelse
            using var command = new NpgsqlCommand( sql, connection);

            //kører CREATE TABLE-kommandoerne fra schema.sql
            command.ExecuteNonQuery();

            Console.WriteLine("Database schema created.");
        }

        private void CreateTriggers() {
            string sql = """
                CREATE OR REPLACE FUNCTION set_main_artist()
                RETURNS TRIGGER
                AS $$
                BEGIN
                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM "SongArtist"
                        WHERE "SongId" = NEW."SongId"
                    )
                    THEN
                        NEW."IsMainArtist" := TRUE;
                    END IF;

                    RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;

                DROP TRIGGER IF EXISTS TRG_SongArtist_SetMainArtist
                ON "SongArtist";

                CREATE TRIGGER TRG_SongArtist_SetMainArtist
                BEFORE INSERT ON "SongArtist"
                FOR EACH ROW
                EXECUTE FUNCTION set_main_artist();
                """;

            using var connection = new NpgsqlConnection(_musicPlatformConnectionString);

            connection.Open();

            using var command = new NpgsqlCommand(sql, connection);

            command.ExecuteNonQuery();

            Console.WriteLine("Database triggers created");
        }
    }

}
