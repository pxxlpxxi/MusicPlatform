## Databaseprogrammering Del 4 noter



### Performance:



#### Query:

```
EXPLAIN ANALYZE

SELECT \*

FROM "Song"

WHERE "Title" ILIKE '%Test%';
```


#### Resultat:
```
"QUERY PLAN"

"Seq Scan on ""Song""  (cost=0.00..12.25 rows=1 width=422) (actual time=0.049..0.049 rows=0.00 loops=1)"

"  Filter: ((""Title"")::text \~\~\* '%Test%'::text)"

"  Rows Removed by Filter: 27"

"  Buffers: shared hit=1 dirtied=1"

"Planning:"

"  Buffers: shared hit=22"

"Planning Time: 1.590 ms"

"Execution Time: 0.063 ms"
```


#### Query:
```
CREATE EXTENSION IF NOT EXISTS pg\_trgm;
```


#### Result:
```
CREATE EXTENSION

Query returned successfully in 331 msec.
```


#### Query:
```
CREATE INDEX IX\_Song\_Title\_Trgm

ON "Song"

USING GIN ("Title" gin\_trgm\_ops);
```


#### Result:
```
CREATE INDEX

Query returned successfully in 109 msec.
```


#### Query:
```
EXPLAIN (ANALYZE, BUFFERS)

SELECT \*

FROM "Song"

WHERE "Title" ILIKE '%Test%';
```


#### Result:
```
"QUERY PLAN"

"Seq Scan on ""Song""  (cost=0.00..1.34 rows=1 width=422) (actual time=0.025..0.025 rows=0.00 loops=1)"

"  Filter: ((""Title"")::text \~\~\* '%Test%'::text)"

"  Rows Removed by Filter: 27"

"  Buffers: shared hit=1"

"Planning:"

"  Buffers: shared hit=25 dirtied=3"

"Planning Time: 1.525 ms"

"Execution Time: 0.036 ms"
```




### Reflektion:

Jeg undersøgte performance på min Song-søgning med EXPLAIN (ANALYZE, BUFFERS). Før indexering anvendte PostgreSQL et Sequential Scan og gennemgik alle 27 rækker.



Jeg oprettede derefter et GIN-index med pg\_trgm på Song.Title, fordi min søgning bruger ILIKE '%Test%', hvor et almindeligt B-tree-index ikke er velegnet.



Efter indexeringen valgte PostgreSQL stadig et Sequential Scan. Execution Time faldt fra 0,063 ms til 0,036 ms, men datasættet er meget lille. PostgreSQL vurderer derfor, at det er billigere at gennemgå tabellen direkte end at bruge indexet.



På et større datasæt vil et passende index kunne give en større performancefordel. Resultatet viser samtidig, at et index ikke nødvendigvis bliver brugt, blot fordi det eksisterer — PostgreSQL vælger execution plan ud fra den forventede omkostning.





### Indexering:



#### Query:
```
CREATE INDEX IX\_SongArtist\_ArtistId

ON "SongArtist" ("ArtistId");
```


#### Result:
```
CREATE INDEX

Query returned successfully in 60 msec.
```


Jeg bruger PostgreSQL, hvor indexmodellen er anderledes end den, der beskrives for SQL Server. Jeg har derfor valgt indexes ud fra de konkrete forespørgsler i min applikation frem for at forsøge at efterligne SQL Server-specifik funktionalitet.



Jeg har implementeret to indexes. Det første er et `*GIN-index`* på `Song.Title` sammen med `pg\_trgm`. Det er valgt, fordi min applikation søger med `ILIKE '%søgeterm%'`. `pg\_trgm` opdeler teksten i små grupper på tre tegn, kaldet trigrams, og `GIN-indexet` gemmer oplysninger om, hvilke rækker der indeholder disse tekststykker. Når databasen søger efter `%Test%`, kan indexet derfor hjælpe med hurtigt at finde relevante rækker i stedet for nødvendigvis at gennemgå hele tabellen. På mit lille datasæt vælger PostgreSQL dog et Sequential Scan, fordi det er billigere at gennemgå de 27 rækker direkte. Det betyder ikke, at indexet er ubrugeligt - fordelen bliver mest relevant ved større datamængder.



Det andet er et almindeligt `B-tree-index` på `SongArtist.ArtistId`. Det gør opslag og joins på ArtistId mere effektive ved at give databasen en separat datastruktur, hvor den hurtigt kan finde relevante rækker.



SQL Servers clustered index bestemmer samtidig den fysiske rækkefølge af dataene i tabellen, mens et non-clustered index er en separat datastruktur, der peger på dataene. PostgreSQL bruger ikke denne samme opdeling i clustered og non-clustered indexes. Mit B-tree-index kan derfor funktionelt sammenlignes med et non-clustered index, men det er ikke teknisk et SQL Server-non-clustered index. PostgreSQL har kommandoen `CLUSTER`, som kan reorganisere en tabel fysisk efter et index, men det er ikke det samme som et SQL Server-clustered index og vedligeholdes ikke automatisk på samme måde.



Indexes bruger ekstra plads og skal vedligeholdes, når data indsættes, opdateres eller slettes. Derfor skal et index vælges ud fra de forespørgsler, databasen faktisk udfører.



