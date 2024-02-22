USE master
GO

DECLARE @SQL nvarchar(1000);
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = N'Books')
BEGIN
    SET @SQL = N'USE Books;

                 ALTER DATABASE Books SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                 USE master;

                 DROP DATABASE Books;';
    EXEC (@SQL);
END;

CREATE DATABASE Books
GO

USE Books
GO

CREATE TABLE Genres (
	GenreId				int IDENTITY(1,1)					NOT NULL,
	GenreName			varchar(50)                         NOT NULL,
	CONSTRAINT PK_Genres PRIMARY KEY(GenreId),
	CONSTRAINT UC_GenreName UNIQUE(GenreName),
)
GO

INSERT INTO Genres(GenreName) VALUES ('Literary fiction');
INSERT INTO Genres(GenreName) VALUES ('Historical fiction');
INSERT INTO Genres(GenreName) VALUES ('Science fiction');
INSERT INTO Genres(GenreName) VALUES ('Fantasy');
INSERT INTO Genres(GenreName) VALUES ('Horror/suspense');
INSERT INTO Genres(GenreName) VALUES ('Mystery');
INSERT INTO Genres(GenreName) VALUES ('General fiction');
INSERT INTO Genres(GenreName) VALUES ('Classics');
INSERT INTO Genres(GenreName) VALUES ('Sports');
INSERT INTO Genres(GenreName) VALUES ('Non-fiction');
INSERT INTO Genres(GenreName) VALUES ('Poetry');
INSERT INTO Genres(GenreName) VALUES ('Romance');

CREATE TABLE Authors (
	AuthorId			int IDENTITY(1,1)					NOT NULL,
	AuthorName			varchar(50)							NOT NULL,
	CONSTRAINT PK_Authors PRIMARY KEY(AuthorId),
)
GO

INSERT INTO Authors(AuthorName) VALUES ('Neal Stephenson');
INSERT INTO Authors(AuthorName) VALUES ('Emma Newman');
INSERT INTO Authors(AuthorName) VALUES ('John Steinbeck');
INSERT INTO Authors(AuthorName) VALUES ('George R.R. Martin');
INSERT INTO Authors(AuthorName) VALUES ('Linnea Hartsuyker');
INSERT INTO Authors(AuthorName) VALUES ('Arthur C. Clarke');
INSERT INTO Authors(AuthorName) VALUES ('Bill Bryson');
INSERT INTO Authors(AuthorName) VALUES ('John Krakauer');
INSERT INTO Authors(AuthorName) VALUES ('Ernest Hemmingway');
INSERT INTO Authors(AuthorName) VALUES ('Graham Greene');
INSERT INTO Authors(AuthorName) VALUES ('John Grisham');
INSERT INTO Authors(AuthorName) VALUES ('Sylvia Plath');

CREATE Table Books (
	BookId				int IDENTITY(1,1)					NOT NULL,
	Title				varchar(50)							NOT NULL,
	AuthorId			int									NOT NULL,
	[PageCount]			int									NOT NULL,
	PublicationYear		int									NULL,
	CONSTRAINT PK_Books PRIMARY KEY(BookId),
	CONSTRAINT FK_Books_Authors FOREIGN KEY(AuthorId) REFERENCES Authors(AuthorId),
)
GO

INSERT INTO Books(Title, AuthorId, [PageCount], PublicationYear) VALUES ('REAMDE', (SELECT a.AuthorId FROM Authors a WHERE a.AuthorName = 'Neal Stephenson'), 1042, 2011);
INSERT INTO Books(Title, AuthorId, [PageCount], PublicationYear) VALUES ('Planetfall', (SELECT a.AuthorId FROM Authors a WHERE a.AuthorName = 'Emma Newman'), 320, 2015);
INSERT INTO Books(Title, AuthorId, [PageCount], PublicationYear) VALUES ('Grapes of Wrath', (SELECT a.AuthorId FROM Authors a WHERE a.AuthorName = 'John Steinbeck'), 473, 1939);
INSERT INTO Books(Title, AuthorId, [PageCount], PublicationYear) VALUES ('A Dance With Dragons', (SELECT a.AuthorId FROM Authors a WHERE a.AuthorName = 'George R.R. Martin'), 1051, 2011);
INSERT INTO Books(Title, AuthorId, [PageCount], PublicationYear) VALUES ('The Half-Drowned King', (SELECT a.AuthorId FROM Authors a WHERE a.AuthorName = 'Linnea Hartsuyker'), 423, 2017);
INSERT INTO Books(Title, AuthorId, [PageCount], PublicationYear) VALUES ('The Fountains of Paradise', (SELECT a.AuthorId FROM Authors a WHERE a.AuthorName = 'Arthur C. Clarke'), 255, 1978);
INSERT INTO Books(Title, AuthorId, [PageCount], PublicationYear) VALUES ('A Walk in the Woods', (SELECT a.AuthorId FROM Authors a WHERE a.AuthorName = 'Bill Bryson'), 274, 1998);
INSERT INTO Books(Title, AuthorId, [PageCount], PublicationYear) VALUES ('Into Thin Air', (SELECT a.AuthorId FROM Authors a WHERE a.AuthorName = 'John Krakauer'), 301, 1997);
INSERT INTO Books(Title, AuthorId, [PageCount], PublicationYear) VALUES ('The Pelican Brief', (SELECT a.AuthorId FROM Authors a WHERE a.AuthorName = 'John Grisham'), 388, 1992);
INSERT INTO Books(Title, AuthorId, [PageCount], PublicationYear) VALUES ('A Storm of Swords', (SELECT a.AuthorId FROM Authors a WHERE a.AuthorName = 'George R.R. Martin'), 1128, NULL);
INSERT INTO Books(Title, AuthorId, [PageCount], PublicationYear) VALUES ('Fall', (SELECT a.AuthorId FROM Authors a WHERE a.AuthorName = 'Neal Stephenson'), 883, NULL);
INSERT INTO Books(Title, AuthorId, [PageCount], PublicationYear) VALUES ('The Colossus and Other Poems', (SELECT a.AuthorId FROM Authors a WHERE a.AuthorName = 'Sylvia Plath'), 72, 1960);

CREATE TABLE BooksGenres (
	BookId				int									NOT NULL,
	GenreId				int									NOT NULL,
	CONSTRAINT PK_BooksGenres PRIMARY KEY(BookId, GenreId),
	CONSTRAINT FK_BooksGenres_Books FOREIGN KEY(BookId) REFERENCES Books(BookId),
	CONSTRAINT FK_BooksGenres_Genres FOREIGN KEY(GenreId) REFERENCES Genres(GenreId),
)
GO

INSERT INTO BooksGenres(BookId, GenreId) VALUES ((SELECT b.BookId FROM Books b WHERE b.Title = 'REAMDE'), (SELECT g.GenreId FROM Genres g WHERE g.GenreName = 'Science fiction'));
INSERT INTO BooksGenres(BookId, GenreId) VALUES ((SELECT b.BookId FROM Books b WHERE b.Title = 'Fall'), (SELECT g.GenreId FROM Genres g WHERE g.GenreName = 'Science fiction'));
INSERT INTO BooksGenres(BookId, GenreId) VALUES ((SELECT b.BookId FROM Books b WHERE b.Title = 'Planetfall'), (SELECT g.GenreId FROM Genres g WHERE g.GenreName = 'Science fiction'));
INSERT INTO BooksGenres(BookId, GenreId) VALUES ((SELECT b.BookId FROM Books b WHERE b.Title = 'Grapes of Wrath'), (SELECT g.GenreId FROM Genres g WHERE g.GenreName = 'Classics'));
INSERT INTO BooksGenres(BookId, GenreId) VALUES ((SELECT b.BookId FROM Books b WHERE b.Title = 'Grapes of Wrath'), (SELECT g.GenreId FROM Genres g WHERE g.GenreName = 'Literary fiction'));
INSERT INTO BooksGenres(BookId, GenreId) VALUES ((SELECT b.BookId FROM Books b WHERE b.Title = 'The Half-Drowned King'), (SELECT g.GenreId FROM Genres g WHERE g.GenreName = 'Literary fiction'));
INSERT INTO BooksGenres(BookId, GenreId) VALUES ((SELECT b.BookId FROM Books b WHERE b.Title = 'A Dance With Dragons'), (SELECT g.GenreId FROM Genres g WHERE g.GenreName = 'Fantasy'));
INSERT INTO BooksGenres(BookId, GenreId) VALUES ((SELECT b.BookId FROM Books b WHERE b.Title = 'A Storm of Swords'), (SELECT g.GenreId FROM Genres g WHERE g.GenreName = 'Fantasy'));
INSERT INTO BooksGenres(BookId, GenreId) VALUES ((SELECT b.BookId FROM Books b WHERE b.Title = 'The Colossus and Other Poems'), (SELECT g.GenreId FROM Genres g WHERE g.GenreName = 'Poetry'));
INSERT INTO BooksGenres(BookId, GenreId) VALUES ((SELECT b.BookId FROM Books b WHERE b.Title = 'A Walk in the Woods'), (SELECT g.GenreId FROM Genres g WHERE g.GenreName = 'Non-fiction'));
INSERT INTO BooksGenres(BookId, GenreId) VALUES ((SELECT b.BookId FROM Books b WHERE b.Title = 'Into Thin Air'), (SELECT g.GenreId FROM Genres g WHERE g.GenreName = 'Non-fiction'));
