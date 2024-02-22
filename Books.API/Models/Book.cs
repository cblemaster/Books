namespace Books.API.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public int AuthorId { get; set; }

    public int PageCount { get; set; }

    public int? PublicationYear { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
