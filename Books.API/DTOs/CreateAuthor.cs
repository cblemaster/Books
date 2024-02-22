namespace Books.API.DTOs
{
    public class CreateAuthor
    {
        public required string AuthorName { get; init; }

        public (bool IsValid, string ErrorEmssage) Validate() =>
            AuthorName.Length is > 0 and <= 50
                ? (true, string.Empty)
                : (false, "Author name is required and must be 50 characters or fewer.");
    }
}
