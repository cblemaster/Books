using Books.Core.GuardClauses;

namespace Books.API.DTOs
{
    public class UpdateAuthor
    {
        public required int AuthorId { get; init; }
        public required string AuthorName { get; init; }

        public (bool IsValid, string ErrorMessage) Validate()
        {
            (bool IsValid, string ErrorMessage) = GuardClauses.IdIsGreaterThanZero(AuthorId, "Invalid author id.");
            if (!IsValid) { return (IsValid, ErrorMessage); }

            (IsValid, ErrorMessage) = GuardClauses.StringLengthIsValid(1, 50, "Author name is required and must be 50 characters or fewer.", AuthorName);
            return (IsValid, ErrorMessage);
        }
    }
}
