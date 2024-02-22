namespace Books.API.DTOs
{
    public class UpdateAuthor
    {
        public required int AuthorId { get; init; }
        public required string AuthorName { get; init; }

        public (bool IsValid, string ErrorEmssage) Validate() => AuthorId < 1
                ? (false, "Invalid author id.")
                : AuthorName.Length is > 0 and <= 50
                ? (false, "Author name is required and must be 50 characters or fewer.")
                : (true, string.Empty);
    }
}
