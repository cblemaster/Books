using Books.API.Context;
using Books.API.DTOs;
using Books.API.Mappers;
using Books.Core.Entities;
using Books.Core.GuardClauses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .Build();

string connectionString = config.GetConnectionString("Project") ?? "Error retrieving connection string!";

builder.Services.AddDbContext<BooksContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/author/{authorId:int}", async Task<Results<BadRequest<string>, Ok<ReadAuthor>, NotFound<string>>> (BooksContext context, int authorId) =>
    !GuardClauses.IdIsGreaterThanZero(authorId, "Invalid author id.").IsValid
        ? TypedResults.BadRequest("Invalid author id.")
        : await context.Authors.SingleOrDefaultAsync(a => a.AuthorId == authorId) is Author author
            ? TypedResults.Ok(EntitiesToDTOs.MapAuthorEntityToReadAuthorDTO(author))
            : TypedResults.NotFound("Author not found."));

app.MapGet("/author", Results<Ok<IEnumerable<ReadAuthor>>, NotFound<string>> (BooksContext context) =>
    context.Authors is IEnumerable<Author> authors && authors.Any()
        ? TypedResults.Ok(EntitiesToDTOs.MapCollectionOfAuthorEntitiesToCollectionOfReadAuthorDTO(authors))
        : TypedResults.NotFound("No authors found."));

app.MapGet("/book/{bookId:int}", async Task<Results<BadRequest<string>, Ok<ReadBook>, NotFound<string>>> (BooksContext context, int bookId) =>
    !GuardClauses.IdIsGreaterThanZero(bookId, "Invalid book id.").IsValid
        ? TypedResults.BadRequest("Invalid book id.")
        : await context.Books.Include(b => b.Author).Include(b => b.Genres).SingleOrDefaultAsync(b => b.BookId == bookId) is Book book
            ? TypedResults.Ok(EntitiesToDTOs.MapBookEntityToReadBookDTO(book))
            : TypedResults.NotFound("Book not found."));

app.MapGet("/book", Results<Ok<IEnumerable<ReadBook>>, NotFound<string>> (BooksContext context) =>
    context.Books.Include(b => b.Author).Include(b => b.Genres) is IEnumerable<Book> books && books.Any()
        ? TypedResults.Ok(EntitiesToDTOs.MapCollectionOfBookEntitiesToCollectionOfReadBookDTO(books))
        : TypedResults.NotFound("No books found."));

app.MapGet("/genre/{genreId:int}", async Task<Results<BadRequest<string>, Ok<ReadGenre>, NotFound<string>>> (BooksContext context, int genreId) =>
    !GuardClauses.IdIsGreaterThanZero(genreId, "Invalid genre id.").IsValid
        ? TypedResults.BadRequest("Invalid genre id.")
        : await context.Genres.SingleOrDefaultAsync(g => g.GenreId == genreId) is Genre genre
            ? TypedResults.Ok(EntitiesToDTOs.MapGenreEntityToReadGenreDTO(genre))
            : TypedResults.NotFound("Genre not found."));

app.MapGet("/genre", Results<Ok<IEnumerable<ReadGenre>>, NotFound<string>> (BooksContext context) =>
    context.Genres is IEnumerable<Genre> genres && genres.Any()
        ? TypedResults.Ok(EntitiesToDTOs.MapCollectionOfGenreEntitiesToCollectionOfReadGenreDTO(genres))
        : TypedResults.NotFound("No genres found."));

app.MapPut("/author/{authorId:int}", (BooksContext context, int authorId, UpdateAuthor updateAuthor) => { });
app.MapPut("/book/{bookId:int}", (BooksContext context, int bookId, UpdateBook updateBook) => { });
app.MapPut("/genre/{genreId:int}", (BooksContext context, int genreId, UpdateGenre updateGenre) => { });

app.MapPost("/author", (BooksContext context, CreateAuthor createAuthor) => { });
app.MapPost("/book", (BooksContext context, CreateBook createBook) => { });
app.MapPost("/genre", (BooksContext context, CreateGenre createGenre) => { });

app.MapDelete("/author/{authorId:int}", async Task<Results<BadRequest<string>, NoContent, NotFound<string>>> (BooksContext context, int authorId) =>
{
    if (!GuardClauses.IdIsGreaterThanZero(authorId, "Invalid author id.").IsValid)
    {
        return TypedResults.BadRequest("Invalid author id.");
    }
    if (await context.Authors.Include(a => a.Books).SingleOrDefaultAsync(a => a.AuthorId == authorId) is Author author)
    {
        if (author.Books.Count != 0)
        {
            return TypedResults.BadRequest("Unable to delete author because the author is associated with one or more books. Remove the author from any books it is associated with before deleting the author.");
        }

        context.Authors.Remove(author);
        await context.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound("Unable to find author to delete.");
});

app.MapDelete("/book/{bookId:int}", async Task<Results<BadRequest<string>, NoContent, NotFound<string>>> (BooksContext context, int bookId) =>
{
    if (!GuardClauses.IdIsGreaterThanZero(bookId, "Invalid book id.").IsValid)
    {
        return TypedResults.BadRequest("Invalid book id.");
    }
    if (await context.Books.Include(b => b.Genres).SingleOrDefaultAsync(b => b.BookId == bookId) is Book book)
    {
        if (book.Genres.Count != 0)
        {
            return TypedResults.BadRequest("Unable to delete book because it is associated with one or more genres. Remove the genres from the book before deleting.");
        }

        context.Books.Remove(book);
        await context.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound("Unable to find book to delete.");
});

app.MapDelete("/genre/{genreId:int}", async Task<Results<BadRequest<string>, NoContent, NotFound<string>>> (BooksContext context, int genreId) =>
{
    if (!GuardClauses.IdIsGreaterThanZero(genreId, "Invalid genre id.").IsValid)
    {
        return TypedResults.BadRequest("Invalid genre id.");
    }
    if (await context.Genres.Include(g => g.Books).SingleOrDefaultAsync(g => g.GenreId == genreId) is Genre genre)
    {
        if (genre.Books.Count != 0)
        {
            return TypedResults.BadRequest("Unable to delete genre because it is associated with one or more books. Remove the genre from any books it is associated with before deleting the genre.");
        }

        context.Genres.Remove(genre);
        await context.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound("Unable to find genre to delete.");
});

app.Run();
