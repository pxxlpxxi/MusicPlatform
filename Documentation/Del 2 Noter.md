## Databaseprogrammering Del 2 noter



### 1. Datastrategi



Udgangspunkt: ADO.net



Udvidelse: `Entity Framework Core` + `PostgreSQL`

Argumentation:

Jeg har valgt `Entity Framework Core` som ORM til kommunikationen

mellem min .NET-applikation og `PostgreSQL`-databasen.



EF Core gør det muligt at arbejde med databasen gennem C#-objekter

i stedet for at skrive SQL til alle databaseoperationer.



Jeg bruger stadig SQL, hvor det giver mening, eksempelvis

til databaseoprettelse, constraints og triggers.



NuGet:

- `Microsoft.EntityFrameworkCore`

- `Npgsql.EntityFrameworkCore.PostgreSQL`



### 2. Seeding function



Jeg har implementeret en `DatabaseSeeder`, som indsætter testdata i databasen.



Seeding bruges til at oprette et kendt datasæt, som kan bruges til at teste applikationens funktioner i de efterfølgende øvelser.



Seederens data indsættes kun, hvis databasen ikke allerede indeholder artister, så eksisterende seed-data ikke overskrives.



### 3. Dataintegritet - Forretningsregler i C# / application layer



####  Arkitektur:

Jeg bruger en simpel Layered Architecture i min	Console App.



`Program.cs` fungerer som applikationens entry point, mine modeller repræsenterer domænedata



`MusicPlatformContext` fungerer som data access-lag gennem Entity Framework Core, og PostgreSQL er den underliggende database.



`DatabaseInitializer` og `DatabaseSeeder` er hjælpekomponenter til henholdsvis databaseopsætning og testdata.



#### Forretningsregler:

#### Create: En `Song` må ikke oprettes uden en titel.

(Application/service layer)



#### `Read`: Søgeterm må ikke være tom, og sange kan søges frem ud fra deres titel.

(Application layer + FK)



#### `Update`: En `Song` må ikke opdateres til en tom titel, og sangen skal eksistere.

(Application layer)



#### `Delete`: En Song skal eksistere, før den kan slettes.

Forretningsreglen om, at en sang skal have en titel, håndhæves i applikationslaget i `SongService`. Databasen har samtidig en `NOT NULL` constraint på `Title`, som beskytter dataintegriteten på databaseniveau.		(Application + database ON DELETE CASCADE)





### `4. Constraints, 4.1`



Jeg bruger blandt andet `NOT NULL`, `UNIQUE` og `FOREIGN KEY` constraints til at beskytte dataintegriteten.



Jeg har også implementeret en regel om, at en sang højst må have én hovedkunstner.



Dette håndhæves med et partial unique index:


```
CREATE UNIQUE INDEX UX\_SongArtist\_OneMainArtist

ON "SongArtist" ("SongId")

WHERE "IsMainArtist" = TRUE;
```


Det betyder, at databasen ikke tillader mere end én række med `IsMainArtist` = `TRUE` for den samme sang





### 5. Automatisering, 5.1



Trigger: `Håndtering af main artist`



Triggeren håndterer tre regler:



1. Hvis en sang får sin første artist, bliver denne automatisk main artist.



2. Hvis sangen allerede har en main artist, respekteres den `IsMainArtist`-værdi, der angives ved tilføjelse af en ny artist.



3. En eksisterende main artist kan ændres, så en anden artist bliver main artist.



Triggeren oprettes gennem .NET-applikationen i `DatabaseInitializer.cs` og gemmes i PostgreSQL.



Sammen med unique indexet sikrer løsningen, at der højst er én main artist, samtidig med at triggeren automatisk gør den første artist til main artist.



Triggeren testes i `Program.cs` med tests for automatisk main artist, respekt for `IsMainArtist` og ændring af main artist.



### `6. Datahistorik / logging`



Jeg har implementeret filbaseret logging gennem `DatabaseLogger`.



CRUD-operationerne `CREATE`, `READ`, `UPDATE` og `DELETE` bliver skrevet til `database.log`. Loggen ligger her: \\MusicPlatform\\bin\\Debug\\net10.0\\database.log



Loggen indeholder tidspunkt, operation, entity, ID og detaljer.



Eksempel:


```
2026-09-22 18:59:09 | CREATE | Song | 28 | Test Song
```




Logging fungerer som datahistorik, fordi man kan se, hvilke databaseoperationer der er blevet udført.



Logging forhindrer ikke ugyldige data, men supplerer constraints og triggers ved at dokumentere, hvad der er sket.

