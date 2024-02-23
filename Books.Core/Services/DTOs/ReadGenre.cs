namespace Books.Core.Services.DTOs
{
    public class ReadGenre
    {
        public required int GenreId { get; init; }
        public required string GenreName { get; init; }

        public static ReadGenre NotFound =>
            new() { GenreId = -1, GenreName = "not found" };

        public static IEnumerable<ReadGenre> CollectionNotFound => Enumerable.Empty<ReadGenre>();
    }
}
