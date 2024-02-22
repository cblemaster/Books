namespace Books.API.DTOs
{
    public class UpdateGenre
    {
        public required int GenreId { get; init; }
        public required string GenreName { get; init; }

        public (bool IsValid, string ErrorEmssage) Validate() => GenreId < 1
                ? (false, "Invalid genre id.")
                : GenreName.Length is > 0 and <= 50
                ? (false, "Genre name is required and must be 50 characters or fewer.")
                : (true, string.Empty);
    }
}
