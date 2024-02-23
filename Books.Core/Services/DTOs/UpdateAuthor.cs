namespace Books.Core.Services.DTOs
{
    public class UpdateAuthor
    {
        public required int AuthorId { get; init; }
        public required string AuthorName { get; init; }

        public (bool IsValid, string ErrorMessage) Validate()
        {
            (bool IsValid, string ErrorMessage) = GuardClauses.GuardClauses.IdIsGreaterThanZero(AuthorId, "Invalid author id.");
            if (!IsValid) { return (IsValid, ErrorMessage); }

            (IsValid, ErrorMessage) = GuardClauses.GuardClauses.StringLengthIsValid(1, 50, "Author name is required and must be 50 characters or fewer.", AuthorName);
            return (IsValid, ErrorMessage);
        }
    }
}
