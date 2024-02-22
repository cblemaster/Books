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

        public (bool IsValid, string ErrorEmssage) Validate()
        {
            if (Title.Length < 1 || Title.Length > 50)
            {
                return (false, "Book title is required and must be 50 characters or fewer.");
            }
            if (AuthorId < 1 || Author is null)
            {
                return (false, "Invalid author.");
            }
            return (true, string.Empty);
        }
    }
}
