# Books

## About:
- A demo application for my common application architecture

### Built with:
- .NET 8 / C# 12
- SQL Server database
- API: ASP.NET Core, minimal API, Entity Framework Core 8
- UI: Empty projects for MAUI, Razor Pages, Signal R (Blazor Server app), Blazor Web App, Angular, and WPF
- Programming techniques:
	- Null object pattern
	- Expression bodied members
	- Pattern matching
	- Asynchronous programming
	- Dependency injection

## Features:
- Authors: Create, Read one, Read all, Update, Delete (if not associated with any books)
- Books: Create, Read one, Read all, Update, Delete (if not associated with any genres)
- Genres: Create, Read one, Read all, Update, Delete (if not associated with any books)

## Business rules:
## UI conventions:
- TBD

## Instructions for running the application:
- Note that SQL server and Visual Studio are required for running the application
- Clone or download the repo
- Browse to \Books\Database
- Run the database script 'Books-DB-Create-DB-And-Initialize-Data-Script.sql'
	- This script will drop (if exists) and re-create the database and tables
	- Note that there is optional sample data that can be inserted into the database as well; it can be found throughout the database script and it is commented out
- Browse to \Books\Books.API\appsettings.json
	- There is a database connection string in this config file that needs to point to your database
- Run the solution in Visual Studio

## Improvement opportunities:
- Updating book entities can be handled better in the API; I have not yet mastered many-to-many relationships in EF
- There is duplication of DTO classes between the API and the Core project; UI projects will need their own DTOs too; for now I think this duplication is better than sharing DTOs, say, from the Core project, as it allows for DTO customization per project