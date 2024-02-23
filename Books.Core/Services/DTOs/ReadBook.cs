namespace Books.Core.Services.DTOs
{
    public class ReadBook
    {
        public required int BookId { get; init; }
        public required string Title { get; init; }
        public required int AuthorId { get; init; }
        public required ReadAuthor Author { get; init; }
        public required int PageCount { get; init; }
        public int? PublicationYear { get; init; }
        public required IEnumerable<ReadGenre> Genres { get; init; }

        public static ReadBook NotFound => new()
        {
            BookId = -1,
            Title = "not found",
            AuthorId = -1,
            Author = null!,
            PageCount = 0,
            PublicationYear = 0,
            Genres = Enumerable.Empty<ReadGenre>(),
        };

        public static IEnumerable<ReadBook> CollectionNotFound => Enumerable.Empty<ReadBook>();
    }
}
