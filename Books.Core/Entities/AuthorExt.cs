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

        public (bool IsValid, string ErrorEmssage) Validate() =>
            AuthorName.Length is > 0 and <= 50
                ? (true, string.Empty)
                : (false, "Author name is required and must be 50 characters or fewer.");
    }
}
