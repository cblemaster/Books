using Books.Core.Entities;

namespace Books.API.DTOs
{
    public class CreateBook
    {
        public required string Title { get; init; }
        public required int AuthorId { get; init; }
        public int PageCount { get; init; }
        public int? PublicationYear { get; init; }
        public required ICollection<ReadGenre> Genres { get; init; }

        public (bool IsValid, string ErrorEmssage) Validate()
        {
            if (Title.Length < 1 || Title.Length > 50)
            {
                return (false, "Book title is required and must be 50 characters or fewer.");
            }
            if (AuthorId < 1)
            {
                return (false, "Invalid author.");
            }
            return (true, string.Empty);
        }
    }
}
