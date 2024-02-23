using Books.Core.GuardClauses;

namespace Books.Core.Services.DTOs
{
    public class CreateAuthor
    {
        public required string AuthorName { get; init; }

        public (bool IsValid, string ErrorMessage) Validate() =>
            GuardClauses.GuardClauses.StringLengthIsValid(1, 50, "Author name is required and must be 50 characters or fewer.", AuthorName);
    }
}
