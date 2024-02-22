using Books.API.DTOs;
using Books.Core.Entities;

namespace Books.API.Mappers
{
    public static class EntitiesToDTOs
    {
        public static ReadAuthor MapAuthorEntityToReadAuthorDTO(Author author)
        {
            if (author is null)
            {
                return ReadAuthor.NotFound;
            }
            return new ReadAuthor()
            {
                AuthorId = author.AuthorId,
                AuthorName = author.AuthorName,
            };
        }
        
        public static ReadBook MapBookEntityToReadBookDTO(Book book)
        {
            if (book is null)
            {
                return ReadBook.NotFound;
            }
            return new ReadBook()
            {
                BookId = book.BookId,
                Title = book.Title,
                AuthorId = book.AuthorId,
                Author = MapAuthorEntityToReadAuthorDTO(book.Author),
                PageCount = book.PageCount,
                PublicationYear = book.PublicationYear,
                Genres = MapCollectionOfGenreEntitiesToCollectionOfReadGenreDTO(book.Genres),
            };
        }

        public static ReadGenre MapGenreEntityToReadGenreDTO(Genre genre)
        {
            if (genre is null)
            {
                return ReadGenre.NotFound;
            }
            return new ReadGenre()
            {
                GenreId = genre.GenreId,
                GenreName = genre.GenreName,
            };
        }

        public static IEnumerable<ReadAuthor> MapCollectionOfAuthorEntitiesToCollectionOfReadAuthorDTO(IEnumerable<Author> authors)
        {
            if (authors is null || !authors.Any())
            {
                return ReadAuthor.CollectionNotFound;
            }

            List<ReadAuthor> dtoCollection = [];

            foreach (Author author in authors)
            {
                ReadAuthor dtoAuthor = MapAuthorEntityToReadAuthorDTO(author);
                dtoCollection.Add(dtoAuthor);
            }

            return dtoCollection.AsEnumerable<ReadAuthor>();
        }

        public static IEnumerable<ReadBook> MapCollectionOfBookEntitiesToCollectionOfReadBookDTO(IEnumerable<Book> books)
        {
            if (books is null || !books.Any())
            {
                return ReadBook.CollectionNotFound;
            }

            List<ReadBook> dtoCollection = [];

            foreach (Book book in books)
            {
                ReadBook dtoBook = MapBookEntityToReadBookDTO(book);
                dtoCollection.Add(dtoBook);
            }

            return dtoCollection.AsEnumerable<ReadBook>();
        }

        public static IEnumerable<ReadGenre> MapCollectionOfGenreEntitiesToCollectionOfReadGenreDTO(IEnumerable<Genre> genres)
        {
            if (genres is null || !genres.Any())
            {
                return ReadGenre.CollectionNotFound;
            }

            List<ReadGenre> dtoCollection = [];

            foreach (Genre genre in genres)
            {
                ReadGenre dtoGenre = MapGenreEntityToReadGenreDTO(genre);
                dtoCollection.Add(dtoGenre);
            }

            return dtoCollection.AsEnumerable<ReadGenre>();
        }
    }
}
