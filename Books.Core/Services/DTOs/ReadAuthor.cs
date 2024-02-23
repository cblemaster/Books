namespace Books.Core.Services.DTOs
{
    public class ReadAuthor
    {
        public required int AuthorId { get; init; }
        public required string AuthorName { get; init; }

        public static ReadAuthor NotFound =>
            new() { AuthorId = -1, AuthorName = "not found" };

        public static IEnumerable<ReadAuthor> CollectionNotFound => Enumerable.Empty<ReadAuthor>();
    }
}
