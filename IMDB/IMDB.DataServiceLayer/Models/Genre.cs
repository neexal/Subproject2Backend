namespace IMDB.DataServiceLayer.Models;

public class Genre
{
    public int GenreId { get; set; }
    public string GenreName { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
}
