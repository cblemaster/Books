namespace Books.API.DTOs
{
    public class CreateGenre
    {
        public required string GenreName { get; init; }

        public (bool IsValid, string ErrorEmssage) Validate() =>
            GenreName.Length is > 0 and <= 50
                ? (true, string.Empty)
                : (false, "Genre name is required and must be 50 characters or fewer.");
    }
}
