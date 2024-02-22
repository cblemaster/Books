namespace Books.API.DTOs
{
    public class UpdateBook
    {
        public required int BookId { get; init; }
        public required string Title { get; init; }
        public required int AuthorId { get; init; }
        public int PageCount { get; init; }
        public int? PublicationYear { get; init; }
        public required ICollection<ReadGenre> Genres { get; init; }

        public (bool IsValid, string ErrorEmssage) Validate() => BookId < 1
                ? (false, "Invalid book id.")
                : Title.Length < 1 || Title.Length > 50
                ? (false, "Book title is required and must be 50 characters or fewer.")
                : AuthorId < 1 ? (false, "Invalid author.") : (true, string.Empty);
    }
}
