using Books.API.DTOs;
using Books.Core.Entities;

namespace Books.API.Mappers
{
    public static class DTOsToEntities
    {
        public static Author MapCreateAuthorDTOToAuthorEntity(CreateAuthor createAuthor) => createAuthor is null
                ? Author.NotFound
                : new Author()
                {
                    AuthorName = createAuthor.AuthorName,
                };

        public static Book MapCreateBookDTOToBookEntity(CreateBook createBook) => createBook is null
                ? Book.NotFound
                : new Book()
                {
                    Title = createBook.Title,
                    AuthorId = createBook.AuthorId,
                    PageCount = createBook.PageCount,
                    PublicationYear = createBook.PublicationYear,
                    Genres = MapCollectionOfReadGenreDTOToCollectionOfGenreEntities(createBook.Genres).ToList(),
                };

        public static Genre MapCreateGenreDTOToGenreEntity(CreateGenre createGenre) => createGenre is null
                ? Genre.NotFound
                : new Genre()
                {
                    GenreName = createGenre.GenreName,
                };

        public static Author MapUpdateAuthorDTOToAuthorEntity(UpdateAuthor updateAuthor, Author authorToUpdate)
        {
            if (updateAuthor is null || updateAuthor.AuthorId != authorToUpdate.AuthorId)
            {
                return authorToUpdate;
            }
            else
            {
                if (!authorToUpdate.AuthorName.Equals(updateAuthor.AuthorName, StringComparison.Ordinal))
                {
                    authorToUpdate.AuthorName = updateAuthor.AuthorName;
                }

                return authorToUpdate;
            }
        }

        public static Book MapUpdateBookDTOToBookEntity(UpdateBook updateBook, Book bookToUpdate)
        {
            if (updateBook is null || updateBook.BookId != bookToUpdate.BookId)
            {
                return bookToUpdate;
            }
            else
            {
                if (!bookToUpdate.Title.Equals(updateBook.Title, StringComparison.Ordinal))
                {
                    bookToUpdate.Title = updateBook.Title;
                }
                if (!bookToUpdate.AuthorId.Equals(updateBook.AuthorId))
                {
                    bookToUpdate.AuthorId = updateBook.AuthorId;
                }
                if (!bookToUpdate.PageCount.Equals(updateBook.PageCount))
                {
                    bookToUpdate.PageCount = updateBook.PageCount;
                }
                if (!bookToUpdate.PublicationYear.Equals(updateBook.PublicationYear))
                {
                    bookToUpdate.PublicationYear = updateBook.PublicationYear;
                }

                return bookToUpdate;
            }
        }

        public static Genre MapUpdateGenreDTOToGenreEntity(UpdateGenre updateGenre, Genre genreToUpdate)
        {
            if (updateGenre is null || updateGenre.GenreId != genreToUpdate.GenreId)
            {
                return genreToUpdate;
            }
            else
            {
                if (!genreToUpdate.GenreName.Equals(updateGenre.GenreName, StringComparison.Ordinal))
                {
                    genreToUpdate.GenreName = updateGenre.GenreName;
                }

                return genreToUpdate;
            }
        }

        public static Genre MapReadGenreDTOToGenreEntity(ReadGenre genre) => genre is null
                ? Genre.NotFound
                : new Genre()
                {
                    GenreId = genre.GenreId,
                    GenreName = genre.GenreName,
                };

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
