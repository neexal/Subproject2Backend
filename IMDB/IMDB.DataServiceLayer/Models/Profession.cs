namespace IMDB.DataServiceLayer.Models;

public class Profession
{
    public int ProfessionId { get; set; }
    public string ProfessionName { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<PersonProfession> PersonProfessions { get; set; } = new List<PersonProfession>();
}
