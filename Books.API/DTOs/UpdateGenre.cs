﻿using Books.Core.GuardClauses;

namespace Books.API.DTOs
{
    public class UpdateGenre
    {
        public required int GenreId { get; init; }
        public required string GenreName { get; init; }

        public (bool IsValid, string ErrorMessage) Validate()
        {
            (bool IsValid, string ErrorMessage) = GuardClauses.IdIsGreaterThanZero(GenreId, "Invalid genre id.");
            if (!IsValid) { return (IsValid, ErrorMessage); }

            (IsValid, ErrorMessage) = GuardClauses.StringLengthIsValid(1, 50, "Genre name is required and must be 50 characters or fewer.", GenreName);
            return (IsValid, ErrorMessage);
        }
    }
}
