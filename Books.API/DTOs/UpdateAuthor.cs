namespace Books.API.DTOs
{
    public class UpdateAuthor
    {
        public required int AuthorId { get; init; }
        public required string AuthorName { get; init; }

        public (bool IsValid, string ErrorEmssage) Validate()
        {
            if (AuthorId < 1)
            {
                return (false, "Invalid author id.");
            }
            if (AuthorName.Length is > 0 and <= 50)
            {
                return (false, "Author name is required and must be 50 characters or fewer.");
            }
            return (true, string.Empty);
        }
    }
}
