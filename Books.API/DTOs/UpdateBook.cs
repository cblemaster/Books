using Books.Core.GuardClauses;

namespace Books.API.DTOs
{
    public class UpdateBook
    {
        public required int BookId { get; init; }
        public required string Title { get; init; }
        public required int AuthorId { get; init; }
        public int PageCount { get; init; }
        public int? PublicationYear { get; init; }
        public required ICollection<ReadGenre> Genres { get; init; }

        public (bool IsValid, string ErrorMessage) Validate()
        {
            (bool IsValid, string ErrorMessage) = GuardClauses.IdIsGreaterThanZero(BookId, "Invalid book id.");
            if (!IsValid) { return (IsValid, ErrorMessage); }

            (IsValid, ErrorMessage) = GuardClauses.IdIsGreaterThanZero(AuthorId, "Invalid author.");
            if (!IsValid) { return (IsValid, ErrorMessage); }

            (IsValid, ErrorMessage) = GuardClauses.StringLengthIsValid(1, 50, "Book title is required and must be 50 characters or fewer.", Title);
            return (IsValid, ErrorMessage);
        }
    }
}