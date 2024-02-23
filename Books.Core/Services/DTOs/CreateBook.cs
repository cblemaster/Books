namespace Books.Core.Services.DTOs
{
    public class CreateBook
    {
        public required string Title { get; init; }
        public required int AuthorId { get; init; }
        public int PageCount { get; init; }
        public int? PublicationYear { get; init; }
        public required ICollection<ReadGenre> Genres { get; init; }

        public (bool IsValid, string ErrorMessage) Validate()
        {
            (bool IsValid, string ErrorMessage) = GuardClauses.GuardClauses.StringLengthIsValid(1, 50, "Book title is required and must be 50 characters or fewer.", Title);
            if (!IsValid) { return (IsValid, ErrorMessage); }

            (IsValid, ErrorMessage) = GuardClauses.GuardClauses.IdIsGreaterThanZero(AuthorId, "Invalid author.");
            return (IsValid, ErrorMessage);
        }
    }
}