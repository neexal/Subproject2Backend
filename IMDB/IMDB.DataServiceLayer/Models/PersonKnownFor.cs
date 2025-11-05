namespace IMDB.DataServiceLayer.Models;

public class PersonKnownFor
{
    public int MovieId { get; set; }
    public int PersonId { get; set; }

    // Navigation properties
    public Movie Movie { get; set; } = null!;
    public Person Person { get; set; } = null!;
}
