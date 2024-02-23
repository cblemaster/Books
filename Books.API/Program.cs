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

app.MapPut("/author/{authorId:int}", async Task<Results<BadRequest<string>, NotFound<string>, NoContent>> (BooksContext context, int authorId, UpdateAuthor updateAuthor) =>
{
    (bool IsValid, string ErrorMessage) = GuardClauses.IdIsGreaterThanZero(authorId, "Invalid author id.");
    if (!IsValid || authorId != updateAuthor.AuthorId) { return TypedResults.BadRequest("Invalid author id."); }
    
    (IsValid, ErrorMessage) = updateAuthor.Validate();
    if (!IsValid) { return TypedResults.BadRequest(ErrorMessage); }

    Author authorToUpdate = await context.Authors.SingleOrDefaultAsync(a => a.AuthorId == authorId) ?? Author.NotFound;
    (IsValid, ErrorMessage) = GuardClauses.IdIsGreaterThanZero(authorToUpdate.AuthorId, "Author to update not found.");
    if (!IsValid) { return TypedResults.NotFound(ErrorMessage); }

    authorToUpdate = DTOsToEntities.MapUpdateAuthorDTOToAuthorEntity(updateAuthor, authorToUpdate);
    context.Authors.Entry(authorToUpdate).State = EntityState.Modified;
    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

app.MapPut("/book/{bookId:int}", async Task<Results<BadRequest<string>, NotFound<string>, NoContent>> (BooksContext context, int bookId, UpdateBook updateBook) =>
{
    (bool IsValid, string ErrorMessage) = GuardClauses.IdIsGreaterThanZero(bookId, "Invalid book id.");
    if (!IsValid || bookId != updateBook.BookId) { return TypedResults.BadRequest("Invalid book id."); }

    (IsValid, ErrorMessage) = updateBook.Validate();
    if (!IsValid) { return TypedResults.BadRequest(ErrorMessage); }

    Book bookToUpdate = await context.Books.Include(b => b.Genres).SingleOrDefaultAsync(b => b.BookId == bookId) ?? Book.NotFound;
    
    (IsValid, ErrorMessage) = GuardClauses.IdIsGreaterThanZero(bookToUpdate.BookId, "Book to update not found.");
    if (!IsValid) { return TypedResults.NotFound(ErrorMessage); }
    
    bookToUpdate = DTOsToEntities.MapUpdateBookDTOToBookEntity(updateBook, bookToUpdate);
    bookToUpdate.Genres.Clear();
    await context.SaveChangesAsync();

    /*  TODO: This is not ideal, but I also have not yet mastered many-to-many relationships in EF Core;
              We just updated the entity w/ an empty navigation (since we cleared the navigation on line 94);
              Now we will add the navigation prop elements back to the entity and save again - not very performant;
              Get the navigation items on the update dto from the db before adding so that they are part of change tracking;
    */

    foreach (ReadGenre genre in updateBook.Genres)
    {
        Genre genreToAddToNavigation = await context.Genres.SingleOrDefaultAsync(g => g.GenreId == genre.GenreId) ?? Genre.NotFound;
        
        (IsValid, ErrorMessage) = GuardClauses.IdIsGreaterThanZero(genreToAddToNavigation.GenreId, "Genre to add to book not found.");
        if (!IsValid) { return TypedResults.NotFound(ErrorMessage); }

        bookToUpdate.Genres.Add(genreToAddToNavigation);
    }
    
    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

app.MapPut("/genre/{genreId:int}", async Task<Results<BadRequest<string>, NotFound<string>, NoContent>> (BooksContext context, int genreId, UpdateGenre updateGenre) =>
{
    (bool IsValid, string ErrorMessage) = GuardClauses.IdIsGreaterThanZero(genreId, "Invalid genre id.");
    if (!IsValid || genreId != updateGenre.GenreId) { return TypedResults.BadRequest("Invalid genre id."); }

    (IsValid, ErrorMessage) = updateGenre.Validate();
    if (!IsValid) { return TypedResults.BadRequest(ErrorMessage); }

    Genre genreToUpdate = await context.Genres.SingleOrDefaultAsync(g => g.GenreId == genreId) ?? Genre.NotFound;
    (IsValid, ErrorMessage) = GuardClauses.IdIsGreaterThanZero(genreToUpdate.GenreId, "Genre to update not found.");
    if (!IsValid) { return TypedResults.NotFound(ErrorMessage); }

    if (await context.Genres.SingleOrDefaultAsync(g => g.GenreName == updateGenre.GenreName && g.GenreId != updateGenre.GenreId) is not null)
    {
        return TypedResults.BadRequest($"Genre name {updateGenre.GenreName} already used.");
    }

    genreToUpdate = DTOsToEntities.MapUpdateGenreDTOToGenreEntity(updateGenre, genreToUpdate);
    context.Genres.Entry(genreToUpdate).State = EntityState.Modified;
    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

app.MapPost("/author", async Task<Results<BadRequest<string>, Created<ReadAuthor>>> (BooksContext context, CreateAuthor createAuthor) =>
{
    (bool IsValid, string ErrorMessage) = createAuthor.Validate();
    
    if (!IsValid) { return TypedResults.BadRequest(ErrorMessage); }

    Author authorToAdd = DTOsToEntities.MapCreateAuthorDTOToAuthorEntity(createAuthor);
    context.Authors.Add(authorToAdd);
    await context.SaveChangesAsync();

    return TypedResults.Created($"/author/{authorToAdd.AuthorId}", EntitiesToDTOs.MapAuthorEntityToReadAuthorDTO(await context.Authors.SingleOrDefaultAsync(a => a.AuthorId == authorToAdd.AuthorId) ?? Author.NotFound));
});

app.MapPost("/book", async Task<Results<BadRequest<string>, Created<ReadBook>>> (BooksContext context, CreateBook createBook) =>
{
    (bool IsValid, string ErrorMessage) = createBook.Validate();

    if (!IsValid) { return TypedResults.BadRequest(ErrorMessage); }

    Book bookToAdd = DTOsToEntities.MapCreateBookDTOToBookEntity(createBook);

    foreach (Genre genre in bookToAdd.Genres)
    {
        context.Genres.Entry(genre).State = EntityState.Unchanged;
    }

    context.Books.Add(bookToAdd);
    await context.SaveChangesAsync();

    return TypedResults.Created($"/book/{bookToAdd.BookId}", EntitiesToDTOs.MapBookEntityToReadBookDTO(await context.Books.Include(b => b.Author).SingleOrDefaultAsync(b => b.BookId == bookToAdd.BookId) ?? Book.NotFound));
});

app.MapPost("/genre", async Task<Results<BadRequest<string>, Created<ReadGenre>>> (BooksContext context, CreateGenre createGenre) =>
{
    (bool IsValid, string ErrorMessage) = createGenre.Validate();

    if (!IsValid) { return TypedResults.BadRequest(ErrorMessage); }

    if (await context.Genres.SingleOrDefaultAsync(g => g.GenreName == createGenre.GenreName) is not null)
    {
        return TypedResults.BadRequest($"Genre name {createGenre.GenreName} already used.");
    }

    Genre genreToAdd = DTOsToEntities.MapCreateGenreDTOToGenreEntity(createGenre);
    context.Genres.Add(genreToAdd);
    await context.SaveChangesAsync();

    return TypedResults.Created($"/genre/{genreToAdd.GenreId}", EntitiesToDTOs.MapGenreEntityToReadGenreDTO(await context.Genres.SingleOrDefaultAsync(g => g.GenreId == genreToAdd.GenreId) ?? Genre.NotFound));
});

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
