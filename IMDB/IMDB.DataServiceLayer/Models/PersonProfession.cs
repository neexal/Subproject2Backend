namespace IMDB.DataServiceLayer.Models;

public class PersonProfession
{
    public int PersonId { get; set; }
    public int ProfessionId { get; set; }

    // Navigation properties
    public Person Person { get; set; } = null!;
    public Profession Profession { get; set; } = null!;
}
