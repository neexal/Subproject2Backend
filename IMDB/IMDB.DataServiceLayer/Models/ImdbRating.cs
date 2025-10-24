namespace IMDB.DataServiceLayer.Models;

public class ImdbRating
{
    public int MovieId { get; set; }
    public decimal Average { get; set; }
    public int Votes { get; set; }

    // Navigation properties
    public Movie Movie { get; set; } = null!;
}
