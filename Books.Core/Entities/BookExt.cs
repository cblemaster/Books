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

        public (bool IsValid, string ErrorEmssage) Validate() => Title.Length < 1 || Title.Length > 50
                ? ((bool IsValid, string ErrorEmssage))(false, "Book title is required and must be 50 characters or fewer.")
                : AuthorId < 1 || Author is null ? ((bool IsValid, string ErrorEmssage))(false, "Invalid author.") : ((bool IsValid, string ErrorEmssage))(true, string.Empty);
    }
}
