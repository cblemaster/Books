using Books.API.DTOs;
using Books.Core.Entities;

namespace Books.API.Mappers
{
    public static class DTOsToEntities
    {
        public static Author MapCreateAuthorDTOToAuthorEntity(CreateAuthor createAuthor)
        {
            if (createAuthor is null)
            {
                return Author.NotFound;
            }
            return new Author()
            {
                AuthorName = createAuthor.AuthorName,
            };
        }

        public static Book MapCreateBookDTOToBookEntity(CreateBook createBook)
        {
            if (createBook is null)
            {
                return Book.NotFound;
            }
            return new Book()
            {
                Title = createBook.Title,
                AuthorId = createBook.AuthorId,
                PageCount = createBook.PageCount,
                PublicationYear = createBook.PublicationYear,
                Genres = MapCollectionOfReadGenreDTOToCollectionOfGenreEntities(createBook.Genres).ToList(),
            };
        }

        public static Genre MapCreateGenreDTOToGenreEntity(CreateGenre createGenre)
        {
            if (createGenre is null)
            {
                return Genre.NotFound;
            }
            return new Genre()
            {
                GenreName = createGenre.GenreName,
            };
        }

        public static Author MapUpdateAuthorDTOToAuthorEntity(UpdateAuthor updateAuthor)
        {
            if (updateAuthor is null)
            {
                return Author.NotFound;
            }
            return new Author()
            {
                AuthorId = updateAuthor.AuthorId,
                AuthorName = updateAuthor.AuthorName,
            };
        }

        public static Book MapUpdateBookDTOToBookEntity(UpdateBook updateBook)
        {
            if (updateBook is null)
            {
                return Book.NotFound;
            }
            return new Book()
            {
                BookId = updateBook.BookId,
                Title = updateBook.Title,
                AuthorId = updateBook.AuthorId,
                PageCount = updateBook.PageCount,
                PublicationYear = updateBook.PublicationYear,
                Genres = MapCollectionOfReadGenreDTOToCollectionOfGenreEntities(updateBook.Genres).ToList(),
            };
        }

        public static Genre MapUpdateGenreDTOToGenreEntity(UpdateGenre updateGenre)
        {
            if (updateGenre is null)
            {
                return Genre.NotFound;
            }
            return new Genre()
            {
                GenreId = updateGenre.GenreId,
                GenreName = updateGenre.GenreName,
            };
        }

        public static Genre MapReadGenreDTOToGenreEntity(ReadGenre genre)
        {
            if (genre is null)
            {
                return Genre.NotFound;
            }
            return new Genre()
            {
                GenreId = genre.GenreId,
                GenreName = genre.GenreName,
            };
        }

        public static IEnumerable<Genre> MapCollectionOfReadGenreDTOToCollectionOfGenreEntities(IEnumerable<ReadGenre> genres)
        {
            if (genres is null || !genres.Any())
            {
                return Genre.CollectionNotFound;
            }
            List<Genre> entityCollection = [];

            foreach (ReadGenre genre in genres)
            {
                Genre entityGenre = MapReadGenreDTOToGenreEntity(genre);
                entityCollection.Add(entityGenre);
            }

            return entityCollection.AsEnumerable<Genre>();
        }
    }
}
