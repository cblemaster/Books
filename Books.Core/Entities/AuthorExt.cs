namespace Books.Core.Entities
{
    public partial class Author
    {
        public static Author NotFound => new()
        {
            AuthorId = -1,
            AuthorName = "not found",
            Books = Enumerable.Empty<Book>().ToList(),
        };

        public static IEnumerable<Author> CollectionNotFound => Enumerable.Empty<Author>();

        public (bool IsValid, string ErrorMessage) Validate() =>
            GuardClauses.GuardClauses.StringLengthIsValid(1, 50, "Author name is required and must be 50 characters or fewer.", AuthorName);
    }
}
