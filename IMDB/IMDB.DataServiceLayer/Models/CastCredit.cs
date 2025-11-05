namespace IMDB.DataServiceLayer.Models;

public class CastCredit
{
    public int CastId { get; set; }
    public int MovieId { get; set; }
    public int PersonId { get; set; }
    public int? CastOrder { get; set; }

    // Navigation properties
    public Movie Movie { get; set; } = null!;
    public Person Person { get; set; } = null!;
    public ICollection<CastCharacter> CastCharacters { get; set; } = new List<CastCharacter>();
}
