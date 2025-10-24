namespace IMDB.DataServiceLayer.Models;

public class CrewCredit
{
    public int CrewId { get; set; }
    public int MovieId { get; set; }
    public int PersonId { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Job { get; set; } = string.Empty;
    public int? CreditOrder { get; set; }

    // Navigation properties
    public Movie Movie { get; set; } = null!;
    public Person Person { get; set; } = null!;
}
