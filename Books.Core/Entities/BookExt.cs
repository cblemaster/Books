namespace Books.Core.Entities
{
    public partial class Book
    {
        public static Book NotFound => new()
        {
            BookId = -1,
            Title = "not found",
            AuthorId = -1,
            Author = null!,
            PageCount = 0,
            PublicationYear = null,
            Genres = Enumerable.Empty<Genre>().ToList(),
        };

        public static IEnumerable<Book> CollectionNotFound => Enumerable.Empty<Book>();

        public (bool IsValid, string ErrorMessage) Validate()
        {
            (bool IsValid, string ErrorMessage) = GuardClauses.GuardClauses.IdIsGreaterThanZero(AuthorId, "Invalid author.");
            if (!IsValid) { return (IsValid, ErrorMessage); }

            (IsValid, ErrorMessage) = GuardClauses.GuardClauses.StringLengthIsValid(1, 50, "", Title);
            return (IsValid, ErrorMessage);
        }
    }
}
