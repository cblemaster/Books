using Books.Core.GuardClauses;

namespace Books.API.DTOs
{
    public class CreateGenre
    {
        public required string GenreName { get; init; }

        public (bool IsValid, string ErrorMessage) Validate() =>
            GuardClauses.StringLengthIsValid(1, 50, "Genre name is required and must be 50 characters or fewer.", GenreName);
    }
}
