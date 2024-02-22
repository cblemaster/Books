namespace Books.API.DTOs
{
    public class UpdateGenre
    {
        public required int GenreId { get; init; }
        public required string GenreName { get; init; }

        public (bool IsValid, string ErrorEmssage) Validate()
        {
            if (GenreId < 1)
            {
                return (false, "Invalid genre id.");
            }
            if (GenreName.Length is > 0 and <= 50)
            {
                return (false, "Genre name is required and must be 50 characters or fewer.");
            }
            return (true, string.Empty);
        }
    }
}
