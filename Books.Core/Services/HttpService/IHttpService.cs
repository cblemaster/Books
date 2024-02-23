using Books.Core.Services.DTOs;

namespace Books.Core.Services.HttpService
{
    public interface IHttpService
    {
        Task<ReadAuthor> GetAuthorByIdAsync(int authorId);
        Task<IEnumerable<ReadAuthor>> GetAuthorsAsync();
        Task<ReadBook> GetBookByIdAsync(int bookId);
        Task<IEnumerable<ReadBook>> GetBooksAsync();
        Task<ReadGenre> GetGenreByIdAsync(int genreId);
        Task<IEnumerable<ReadGenre>> GetGenresAsync();
        Task<ReadAuthor> CreateAuthorAsync(CreateAuthor createAuthor);
        Task<ReadBook> CreateBookAsync(CreateBook createBook);
        Task<ReadGenre> CreateGenreAsync(CreateGenre createGenre);
        void UpdateAuthorAsync(int authorId, UpdateAuthor updateAuthor);
        void UpdateBookAsync(int bookId, UpdateBook updateBook);
        void UpdateGenreAsync(int genreId, UpdateGenre updateGenre);
        void DeleteAuthorAsync(int authorId);
        void DeleteBookAsync(int bookId);
        void DeleteGenreAsync(int genreId);
    }
}
