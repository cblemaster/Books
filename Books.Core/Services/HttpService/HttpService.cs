using Books.Core.Services.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace Books.Core.Services.HttpService
{
    public class HttpService: IHttpService
    {
        private readonly HttpClient _client;
        private const string BASE_URI = "https://localhost:7048";

        public HttpService() => _client = new HttpClient
        {
            BaseAddress = new Uri(BASE_URI)
        };

        public async Task<ReadAuthor> GetAuthorByIdAsync(int authorId)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"/author/{authorId}");
                return response.IsSuccessStatusCode && response.Content is not null
                    ? await response.Content.ReadFromJsonAsync<ReadAuthor>() ?? ReadAuthor.NotFound
                    : ReadAuthor.NotFound;
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<ReadAuthor>> GetAuthorsAsync()
        {
            try
            {
                IEnumerable<ReadAuthor> noneFound = ReadAuthor.CollectionNotFound;

                HttpResponseMessage response = await _client.GetAsync("/author");
                if (response.IsSuccessStatusCode && response.Content is not null)
                {
                    IEnumerable<ReadAuthor> authors = response.Content.ReadFromJsonAsAsyncEnumerable<ReadAuthor>().ToBlockingEnumerable().ToList().AsReadOnly();
                    return authors is null || !authors.Any(a => a?.GetType() == typeof(ReadAuthor)) ? noneFound : authors!;
                }
                return noneFound;
            }
            catch (Exception) { throw; }
        }

        public async Task<ReadBook> GetBookByIdAsync(int bookId)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"/book/{bookId}");
                return response.IsSuccessStatusCode && response.Content is not null
                    ? await response.Content.ReadFromJsonAsync<ReadBook>() ?? ReadBook.NotFound
                    : ReadBook.NotFound;
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<ReadBook>> GetBooksAsync()
        {
            try
            {
                IEnumerable<ReadBook> noneFound = ReadBook.CollectionNotFound;

                HttpResponseMessage response = await _client.GetAsync("/book");
                if (response.IsSuccessStatusCode && response.Content is not null)
                {
                    IEnumerable<ReadBook> books = response.Content.ReadFromJsonAsAsyncEnumerable<ReadBook>().ToBlockingEnumerable().ToList().AsReadOnly();
                    return books is null || !books.Any(a => a?.GetType() == typeof(ReadBook)) ? noneFound : books!;
                }
                return noneFound;
            }
            catch (Exception) { throw; }
        }

        public async Task<ReadGenre> GetGenreByIdAsync(int genreId)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"/genre/{genreId}");
                return response.IsSuccessStatusCode && response.Content is not null
                    ? await response.Content.ReadFromJsonAsync<ReadGenre>() ?? ReadGenre.NotFound
                    : ReadGenre.NotFound;
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<ReadGenre>> GetGenresAsync()
        {
            try
            {
                IEnumerable<ReadGenre> noneFound = ReadGenre.CollectionNotFound;

                HttpResponseMessage response = await _client.GetAsync("/genre");
                if (response.IsSuccessStatusCode && response.Content is not null)
                {
                    IEnumerable<ReadGenre> genres = response.Content.ReadFromJsonAsAsyncEnumerable<ReadGenre>().ToBlockingEnumerable().ToList().AsReadOnly();
                    return genres is null || !genres.Any(a => a?.GetType() == typeof(ReadGenre)) ? noneFound : genres!;
                }
                return noneFound;
            }
            catch (Exception) { throw; }
        }

        public async Task<ReadAuthor> CreateAuthorAsync(CreateAuthor createAuthor)
        {
            StringContent content = new(JsonSerializer.Serialize(createAuthor));
            content.Headers.ContentType = new("application/json");

            try
            {
                HttpResponseMessage response = await _client.PostAsync("/author", content);
                response.EnsureSuccessStatusCode();
                return await GetAuthorByIdAsync((await response.Content.ReadFromJsonAsync<ReadAuthor>() is ReadAuthor newAuthor ? newAuthor.AuthorId : 0));
            }
            catch (Exception) { throw; }
        }

        public async Task<ReadBook> CreateBookAsync(CreateBook createBook)
        {
            StringContent content = new(JsonSerializer.Serialize(createBook));
            content.Headers.ContentType = new("application/json");

            try
            {
                HttpResponseMessage response = await _client.PostAsync("/book", content);
                response.EnsureSuccessStatusCode();
                return await GetBookByIdAsync((await response.Content.ReadFromJsonAsync<ReadBook>() is ReadBook newBook ? newBook.BookId : 0));
            }
            catch (Exception) { throw; }
        }

        public async Task<ReadGenre> CreateGenreAsync(CreateGenre createGenre)
        {
            StringContent content = new(JsonSerializer.Serialize(createGenre));
            content.Headers.ContentType = new("application/json");

            try
            {
                HttpResponseMessage response = await _client.PostAsync("/genre", content);
                response.EnsureSuccessStatusCode();
                return await GetGenreByIdAsync((await response.Content.ReadFromJsonAsync<ReadGenre>() is ReadGenre newGenre ? newGenre.GenreId : 0));
            }
            catch (Exception) { throw; }
        }

        public async void UpdateAuthorAsync(int authorId, UpdateAuthor updateAuthor)
        {
            StringContent content = new(JsonSerializer.Serialize(updateAuthor));
            content.Headers.ContentType = new("application/json");

            try
            {
                HttpResponseMessage response = await _client.PutAsync($"/author/{authorId}", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) { throw; }
        }

        public async void UpdateBookAsync(int bookId, UpdateBook updateBook)
        {
            StringContent content = new(JsonSerializer.Serialize(updateBook));
            content.Headers.ContentType = new("application/json");

            try
            {
                HttpResponseMessage response = await _client.PutAsync($"/book/{bookId}", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) { throw; }
        }

        public async void UpdateGenreAsync(int genreId, UpdateGenre updateGenre)
        {
            StringContent content = new(JsonSerializer.Serialize(updateGenre));
            content.Headers.ContentType = new("application/json");

            try
            {
                HttpResponseMessage response = await _client.PutAsync($"/genre/{genreId}", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) { throw; }
        }

        public async void DeleteAuthorAsync(int authorId)
        {
            (bool IsValid, _) = GuardClauses.GuardClauses.IdIsGreaterThanZero(authorId, "Invalid author id.");
            if (!IsValid) { return; }

            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"/author/{authorId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) { throw; }
        }

        public async void DeleteBookAsync(int bookId)
        {
            (bool IsValid, _) = GuardClauses.GuardClauses.IdIsGreaterThanZero(bookId, "Invalid book id.");
            if (!IsValid) { return; }

            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"/book/{bookId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) { throw; }
        }

        public async void DeleteGenreAsync(int genreId)
        {
            (bool IsValid, _) = GuardClauses.GuardClauses.IdIsGreaterThanZero(genreId, "Invalid genre id.");
            if (!IsValid) { return; }

            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"/genre/{genreId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) { throw; }
        }
    }
}
