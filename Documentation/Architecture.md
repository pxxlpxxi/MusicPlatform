## Arkitektur

Projektet bruger en **Simple Layered Architecture**, hvor applikationen er opdelt i komponenter med forskellige ansvarsområder. 
De centrale dele er præsentation/UI, application logic, services, dataadgang og database/infrastruktur.

Arkitekturen indeholder samtidig et princip fra **Clean Architecture**, nemlig at den konkrete databaseimplementation 
ikke er den direkte afhængighed for services. 
Dette er opnået ved at introducere interfacet ``IMusicPlatformContext``.

Services afhænger derfor af ``IMusicPlatformContext``, mens den konkrete ``MusicPlatformContext`` implementerer dette interface. 
På den måde ligger abstraktionen mellem applikationslogikken og den konkrete databaseimplementation.

Projektet er ikke en fuld Clean Architecture-implementering. 
Der er f.eks. ikke oprettet et separat Domain-lag med repository interfaces for alle former for dataadgang. 
Arkitekturen er derfor bedst beskrevet som en **Simple Layered Architecture med enkelte Clean Architecture-inspirerede principper**.

Formålet med opdelingen er at give de forskellige dele af programmet tydelige ansvarsområder og samtidig begrænse afhængigheden 
til konkrete teknologier, hvor det er relevant.

### Afhængigheder
Den overordnede struktur kan illustreres sådan her:
```
Presentation / UI
       │
       ▼
Application / Services
       │
       ▼
`IMusicPlatformContext`
       ▲
       │
`MusicPlatformContext`
       │
       ▼
EF Core / PostgreSQL
```

Pilene viser afhængigheden mellem delene.

Services afhænger af abstraktionen ``IMusicPlatformContext`` og ikke direkte af ``MusicPlatformContext``.

``MusicPlatformContext`` implementerer ``IMusicPlatformContext`` og står for den konkrete EF Core-baserede databaseadgang.

Det betyder, at den konkrete implementation af dataadgangen kan udskiftes uden nødvendigvis at ændre den service, der bruger interfacet. 
Det kan f.eks. være relevant ved tests eller hvis dataadgangen senere skal ændres.

Dette er et eksempel på **Dependency Inversion**, hvor den højere liggende logik afhænger af en abstraktion frem for direkte af en konkret implementation.


### Clean Architecture-elementer
Projektet følger ikke **Clean Architecture** fuldt ud, men indeholder enkelte principper, der minder om arkitekturen.

Det vigtigste eksempel er ``IMusicPlatformContext``.

Uden interfacet ville en service eksempelvis være direkte afhængig af:

```
SongService
    ↓
`MusicPlatformContext`

Efter ændringen er afhængigheden i stedet:

SongService
    ↓
`IMusicPlatformContext`
    ↑
`MusicPlatformContext`
```

`SongService` kender dermed kun interfacet, mens den konkrete ``MusicPlatformContext`` leverer implementationen.

Det reducerer koblingen mellem service-laget og den konkrete EF Core-context.

Projektet er dog ikke en fuld Clean Architecture, fordi der ikke er en komplet opdeling i eksempelvis Domain, Application og Infrastructure med interfaces for alle eksterne afhængigheder. Services arbejder fortsat med EF Core-relaterede abstraktioner såsom `DbSet`.


### Separation of concerns
Projektet forsøger at holde forskellige typer ansvar adskilt:

* UI håndterer brugerinput og output.

* Application indeholder application models, mapping og application services.

* Services indeholder den primære forretningslogik og databaseoperationer.

* Models indeholder projektets entiteter.

* Data indeholder databasekonteksten og abstraktionen over den.

* Database indeholder databaseopsætning og SQL-relaterede databaseobjekter.

* Seeding indeholder initialiseringsdata til databasen.

* Logging håndterer logning af databaseoperationer.

* Helpers indeholder hjælpefunktioner til blandt andet test og demonstration.

### Filstruktur

```
MusicPlatform/
│
├── Application/
│   └── Mappers/
│       └── SongMapper.cs
│   ├── Models/
│   │   ├── AlbumInfo.cs
│   │   ├── MediaInfo.cs
│   │   ├── MediaTypeName.cs
│   │   └── SongInfo.cs
│   │
│   └── Services/
│       └── SongCreationApplicationService.cs
│
├── Data/
│   ├── `IMusicPlatformContext`.cs
│   └── `MusicPlatformContext`.cs
│
├── Database/
│   ├── create_database.sql
│   ├── schema.sql
│   └── DatabaseInitializer.cs
│
├── Documentation/
│       ├── Evidence 
│       ├── Indexes.png
│       └── StoredProcedure_Indexes_Triggers.png
│   ├── Architecture.md
│   ├── Del_2_Noter.md
│   ├── Del_3_Noter.md
│   └── Del_4_Noter.md
│
├── Helpers/
│   └── DatabaseTestHelper.cs
│
├── Logging/
│   └── DatabaseLogger.cs
│
├── Models/
│   ├── Album.cs
│   ├── AlbumSong.cs
│   ├── Artist.cs
│   ├── Media.cs
│   ├── MediaType.cs
│   ├── Song.cs
│   ├── SongArtist.cs
│   └── User.cs
│
├── Seeding/
│   └── DatabaseSeeder.cs
│
├── Services/
│   ├── AlbumService.cs
│   ├── ArtistService.cs
│   ├── MediaService.cs
│   ├── SongCreationService.cs
│   ├── SongService.cs
│   └── UserService.cs
│
├── UI/
│   ├── DatabaseTestHelper.cs
│   ├── IInput.cs
│   ├── Input.cs
│   ├── IOutput.cs
│   ├── Output.cs
│   └── PasswordReader.cs
│
├── .gitattributes
├── .gitignore
│
└── Program.cs

```

### Program.cs
`Program.cs` fungerer som application entry point.

Den starter applikationen og opretter de nødvendige komponenter. Her kobles blandt andet den konkrete 
`MusicPlatformContext` sammen med de services, der modtager `IMusicPlatformContext`.

Program.cs bruges også til at starte test- og demonstrationsmetoder, som dokumenterer funktionaliteten i projektet.

UI-laget
UI-laget håndterer kommunikation med brugeren via konsollen.

IInput og Input håndterer brugerinput, mens `IOutput` og `Output` håndterer output.

`PasswordReader` håndterer input af passwords.

`UIHelper` indeholder mindre hjælpefunktioner til formatering og præsentation af data i konsollen.

UI-laget er dermed adskilt fra den underliggende forretningslogik og databaseadgang.

### Services
`Services` indeholder projektets primære forretningslogik og databaseoperationer.

F.eks.:

* `SongService` håndterer CRUD-operationer og søgning på sange.

* `ArtistService` håndterer operationer relateret til kunstnere.

* `AlbumService` håndterer operationer relateret til albums.

* `MediaService` håndterer operationer relateret til media.

* `UserService` håndterer brugeroprettelse, opslag, validering og sletning.

* `SongCreationService` håndterer oprettelse af en komplet sang med tilhørende data.

* `Services` modtager `IMusicPlatformContext` gennem constructor injection.

Det betyder, at services ikke selv opretter en konkret `MusicPlatformContext`, men i stedet arbejder gennem interfacet.

Eksempel:
```
internal SongService(IMusicPlatformContext context)
{
    _context = context;
}
```
Denne afhængighed er central for arkitekturen, da service-laget dermed ikke er direkte afhængigt af den konkrete databaseimplementation.

### Application
`Application` indeholder applikationsspecifik funktionalitet.

`Application/Models` indeholder modeller, der er tilpasset applikationens use cases. De bruges til at transportere og præsentere data uden nødvendigvis at eksponere de underliggende databaseentiteter direkte.

`SongMapper` bruges til at omdanne entiteter og relaterede data til eksempelvis SongInfo.

`SongCreationApplicationService` indeholder use-case-orienteret applikationslogik omkring oprettelse af sange.

Application-laget er derfor adskilt fra de databaseorienterede modeller og fra UI-laget.

### Models
Models indeholder de primære entiteter, som projektet arbejder med, eksempelvis:

* `Song`

* `Artist`

* `Album`

* `Media`

* `MediaType`

* `User`

* `SongArtist`

* `AlbumSong`

Disse models repræsenterer de entiteter, som EF Core arbejder med i forbindelse med databasen.

De er adskilt fra UI-laget og fra de applikationsspecifikke modeller i `Application/Models`.

### Data
`Data` indeholder dataadgangen.

`MusicPlatformContext` er projektets konkrete EF Core DbContext. Den indeholder DbSet-egenskaber og EF Core-konfiguration af entiteter, relationer og database mapping.

`IMusicPlatformContext` fungerer som en abstraktion over `MusicPlatformContext`.

Services afhænger derfor af:

`IMusicPlatformContext`

og ikke direkte af:

`MusicPlatformContext`

`MusicPlatformContext` implementerer interfacet og står for den konkrete kommunikation med PostgreSQL gennem **Entity Framework Core**.

Data-laget fungerer dermed som forbindelsen mellem applikationens service-logik og den konkrete database.

### Database
`Database` indeholder database-relateret opsætning.

`create_database.sql` og `schema.sql` indeholder SQL til oprettelse og konfiguration af databasen.

`DatabaseInitializer` står for at initialisere databasen og oprette databaseobjekter såsom tabeller, 
constraints, indexes, triggers og stored procedures.

Database-delen repræsenterer dermed den konkrete database/infrastruktur, som applikationen anvender gennem dataadgangen.

### Seeding
`DatabaseSeeder` indeholder initialiseringsdata til databasen.

Den opretter blandt andet eksempeldata for:

* sange
* kunstnere
* albums
* media
* brugere

Seeding er adskilt fra de normale services, da dens formål er at oprette et kendt datasæt til applikationen.

DatabaseSeeder modtager også `IMusicPlatformContext`, så seeding-koden kan arbejde med samme dataabstraktion som de øvrige dele af applikationen.

### Logging
`DatabaseLogger` håndterer logning af databaseoperationer.

Den bruges eksempelvis af services til at registrere `CREATE`, `READ`, `UPDATE` og `DELETE`-operationer.

Logging er dermed adskilt fra selve service-logikken, selvom services kalder loggeren i forbindelse med databaseoperationer.

### Helpers
`DatabaseTestHelper` indeholder hjælpefunktioner til projektets test- og demonstrationsscenarier.

Den bruges blandt andet til at demonstrere:

* CRUD-funktionalitet
* database-triggers
* validering
* brugerrettigheder
* databasefunktionalitet

Test-helperen modtager de nødvendige afhængigheder gennem constructoren og arbejder med `IMusicPlatformContext`, hvor der er behov for direkte adgang til data.

### Samlet set
Projektet kan bedst beskrives som en **Simple Layered Architecture med enkelte Clean Architecture-inspirerede principper**.

Den grundlæggende struktur er stadig lagdelt, hvor UI, application logic, services, dataadgang og database/infrastruktur er opdelt efter ansvar.

Samtidig er afhængigheden mellem services og den konkrete databaseimplementation blevet reduceret gennem `IMusicPlatformContext`.

I stedet for:

```
Service
   ↓
`MusicPlatformContext`
```
er afhængigheden:
```
Service
   ↓
`IMusicPlatformContext`
   ↑
`MusicPlatformContext`
```
Det betyder, at service-laget afhænger af en abstraktion, mens den konkrete databaseimplementation implementerer denne abstraktion. 
Dette er et eksempel på **Dependency Inversion** og er et Clean Architecture-inspireret element i projektet.

Arkitekturen er derfor ikke en fuld Clean Architecture, men den indeholder et konkret arkitektonisk princip, der gør det muligt 
at adskille den primære applikationslogik fra den konkrete databaseimplementation.

Formålet med arkitekturen er at holde ansvar adskilt, reducere unødvendig kobling og gøre projektet lettere at forstå, teste og vedligeholde.