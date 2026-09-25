## Databaseprogrammering Del 3 noter



### 1. Datastrategi



Jeg fortsætter med `Entity Framework Core` som ORM til kommunikationen mellem min .NET-applikation og `PostgreSQL`-databasen.



Jeg bruger også parametriserede SQL-kommandoer, hvor jeg har behov for at skrive SQL direkte.



#### NuGet:



`Microsoft.EntityFrameworkCore`



`Npgsql.EntityFrameworkCore.PostgreSQL`



### 2. Indsæt data i databasen



#### SQL Injection



Når brugerinput indsættes eller opdateres i en database, kan applikationen være udsat for SQL Injection, hvis brugerens input sættes direkte ind i SQL-strengen.



Jeg har derfor implementeret `CreateUser` med parametriseret SQL:


````
string sql =

"CALL CreateUser(@username, @password, @role)"; 
````



```
_context.Database.ExecuteSqlRaw(

   sql,

   new NpgsqlParameter("@username", username),

   new NpgsqlParameter("@password", password),

   new NpgsqlParameter("@role", role));
```




Her er `@username`, `@password` og `@role` parametre, mens de faktiske værdier sendes separat.



Det betyder, at brugerinput ikke bliver fortolket som en del af selve SQL-kommandoen. Inputtet behandles derfor som data og ikke som SQL-kode.



Jeg har også en `CreateUserUnsafe`-metode, som demonstrerer forskellen:


```
string sql =
   `$"INSERT INTO \"User\" (\"Username\", \"Password\", \"Role\") " +`

  $"VALUES ('{username}', '{password}', '{role}')";
  ```





Denne metode indsætter værdier direkte i SQL-strengen og er derfor et eksempel på den usikre tilgang, som parametrisering beskytter imod.



### Refleksion



SQL Injection er et sikkerhedsproblem, fordi en angriber kan forsøge at få brugerinput fortolket som SQL-kode.



Ved parametrisering bliver SQL-strukturen adskilt fra værdierne. Derfor kan brugerinput ikke ændre SQL-kommandoens struktur på samme måde som ved direkte string concatenation.





### 3. Rettighedsstyring



#### `Brugere og rettigheder`



Jeg har implementeret en `User`-tabel med:



* `Id`
* `Username`
* `Password`
* `Role`



Role kan være enten `User` eller `Admin`.



Dette håndhæves også af en database constraint:



`CONSTRAINT CK\_User\_Role`

`CHECK ("Role" IN ('User', 'Admin'))`





Jeg har implementeret reglen om, at kun en Admin må slette en bruger.



I `UserService` kontrolleres brugerens rolle før DELETE:



`if (role != "Admin")`

`{`

&#x20;   `throw new UnauthorizedAccessException(`

&#x20;       `"Only an admin can delete a user.");`

`}`





Selve `DELETE`-kommandoen bruger også en parameter:


```
string sql =

   "DELETE FROM \\"User\\" " +


   "WHERE \\"Id\\" = @userId";
   ```




#### Test

Jeg har testet rettighedsstyringen med både en almindelig User og en Admin.



Resultatet er:

```
User correctly denied delete access.

Admin correctly allowed to delete.
```


#### Refleksion



Formålet med rettighedsstyringen er Least Privilege.



En almindelig bruger skal ikke have mulighed for at udføre handlinger, som kun en administrator har behov for. Derfor kontrollerer applikationen brugerens rolle, før en bruger kan slettes.



### 4. Validering af data

Validering af brugerinput



Jeg har placeret valideringen af brugeroprettelse i min stored procedure `CreateUser`.



Stored proceduren kontrollerer:



1. Username må ikke være tom.
2. Password må ikke være tom.
3. Role skal være enten User eller Admin.



Eksempel:


```
IF p_username IS NULL OR TRIM(p_username) = '' THEN

   RAISE EXCEPTION 'Username cannot be empty.';

END IF;
```




Role bliver også valideret:


```
IF p\_role NOT IN ('User', 'Admin') THEN

   RAISE EXCEPTION 'Invalid role.';

END IF;
```




Derudover har databasen en `NOT NULL` constraint på Username, Password og Role samt en `CHECK` constraint på Role.



`Username` har også en `UNIQUE` constraint, så to brugere ikke kan have samme username.



#### Refleksion



Validering er vigtigt, fordi brugerinput ikke automatisk kan antages at være korrekt.



Applikationen skal sikre, at data følger systemets regler, før data bliver accepteret.



Jeg har valgt at placere denne specifikke validering i stored proceduren, så reglerne bliver håndhævet, når brugeroprettelsen foretages gennem denne procedure.



Jeg har testet valideringen med et tomt username:


```
 * User Validation *

Validation correctly rejected user: Username cannot be empty.
```


### 5. Stored Procedures

#### CreateUser



Jeg har implementeret en stored procedure i PostgreSQL:


```
CREATE OR REPLACE PROCEDURE CreateUser(

   p_username VARCHAR(100),

   p_password VARCHAR(255),

   p_role VARCHAR(20)
)
```




Proceduren modtager tre parametre:



* `p_username`
* `p_password`
* `p_role`



Proceduren validerer inputtet og indsætter derefter brugeren i User-tabellen.



Den indeholder også fejlhåndtering for duplicate username:


```
EXCEPTION

   WHEN unique\_violation THEN

       RAISE EXCEPTION 'Username already exists.';
```




Applikationen kalder proceduren med:


```
string sql =

   "CALL CreateUser(@username, @password, @role)";
```


#### Refleksion



En stored procedure er kode, der ligger i databasen og kan udføre databaseoperationer.



Fordelen ved at placere denne funktionalitet i databasen er, at procedurens regler kan ændres uden nødvendigvis at ændre og genkompilere applikationen.



Proceduren modtager parametre i stedet for at bygge SQL ud fra brugerinput. Det gør, at den kan bruges sikkert sammen med parametriserede SQL-kald.



Min `CreateUser`-procedure både modtager parametre, validerer data, ændrer data og håndterer en databasefejl.



#### 6. Fejlhåndtering



**Database-specifik exception**



Jeg har implementeret fejlhåndtering med PostgresException fra Npgsql.



Eksempel:


```
catch (PostgresException ex)

{

   \_output.WriteError(

       $"Database error: {ex.MessageText}");

}
```




Jeg bruger PostgresException, fordi applikationen kommunikerer med en PostgreSQL-database.



Det gør det muligt at skelne mellem databasefejl og andre uventede exceptions.



Jeg har også en almindelig `Exception` som sidste fallback:


```
catch (Exception ex)

{

   \_output.WriteError(

       $"Unexpected error: {ex.Message}");
}
```


#### Test



Jeg har testet databasefejlhåndteringen ved at forsøge at oprette en bruger med et tomt username.



Stored proceduren genererer en databasefejl:


```
Username cannot be empty.
```




Denne fejl bliver sendt tilbage til applikationen som en `PostgresException`, som derefter bliver fanget og vist til brugeren.



#### Refleksion



Fejlhåndtering er vigtigt, fordi databaseoperationer kan fejle af mange forskellige årsager, eksempelvis ugyldige data, constraints eller andre databaseproblemer.



Ved at bruge en database-specifik exception kan jeg håndtere PostgreSQL-fejl særskilt fra almindelige programmeringsfejl.



Brugeren får samtidig en forståelig fejlbesked i stedet for at applikationen bare stopper med en ukontrolleret exception.



#### Samlet sikkerhed



I denne del har jeg fokuseret på at sikre kommunikationen mellem applikationen og databasen.



* Parametriserede SQL-kommandoer for at beskytte mod SQL Injection.
* Rollerne `User` og `Admin` til at begrænse, hvem der må udføre bestemte handlinger.
* Validering i stored procedures samt database constraints til at sikre, at ugyldige data ikke accepteres.
* En stored procedure til brugeroprettelse, som modtager parametre, validerer input, ændrer data og håndterer databasefejl.
* PostgresException til at håndtere databasefejl specifikt og give brugeren en meningsfuld fejlbesked.

