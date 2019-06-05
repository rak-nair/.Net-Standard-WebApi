# WebAPIAssignment

Task Create a full functioning backend capable of serving endpoints.

Endpoints required:
- Add player
- Get players
- Add match
- Get matches
- Add player to match
- Get players in a match

Entities Player:
- Name
- YOB
Match:
- Date and Time of Match
- Title (who’s playing)

Technical Use

- WebAPI
- REST / Json
- Sql Server (any type)
- Entity Framework

Points for
Evaluation

- All endpoints functioning
- SOLID
- Testability
- Tests
- Scalability
- Performant
- Consistency
- Simplicity

Hints and links - For DI you can use SimpleInjector found in NuGet (along with
SimpleInjector.Integration.WebAPI).

#Solution

Endpoints Implemented – 
•	Add player - ... api/players POST
•	Get players - ... api/players?page=x&pageSize=y GET***
•	Get player - ... api/players/playerid GET
•	Add match - ... api/matches POST  
•	Get matches - ... api/matches?page=x&pageSize=y GET***
•	Get match -  ... api/matches/matchid GET
•	Add player to match - ... api/matches/matchid/players POST
•	Get players in a match - ... api/matches/matchid/players?page=x&pageSize=y GET***

***Note: page and pageSize parameters are optional, the defaults are 1 and 50 respectively.

Comments
•	All API methods return Task<IHttpActionResult> and the content format is left up to content-negotiation.
•	All API methods return Tasks in the hopes of better performance, but the best way to check would be by running diagnostics on the server.
•	InitialCreate in the Migrations folder can be used to create the SQLDB using the “Update-Database” command in the PackageManagerConsole.
•	The Connection string used, goes by the name “DefaultConnection” in the web.config.
•	For all EF queries AsNoTracking() is used to improve performance.
•	Adding the Paging option in all the GETs may have added an extra degree of complexity, but it helps with performance when dealing with large datasets. I also recommend setting an upper limit for PageSize.
