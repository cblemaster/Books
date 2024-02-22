namespace Books.Core.Entities
{
    public partial class Genre
    {
        public static Genre NotFound => new()
        {
            GenreId = -1,
            GenreName = "not found",
            Books = Enumerable.Empty<Book>().ToList(),
        };

        public static IEnumerable<Genre> CollectionNotFound => Enumerable.Empty<Genre>();

        public (bool IsValid, string ErrorMessage) Validate() =>
            GuardClauses.GuardClauses.StringLengthIsValid(1, 50, "Genre name is required and must be 50 characters or fewer.", GenreName);
    }
}
