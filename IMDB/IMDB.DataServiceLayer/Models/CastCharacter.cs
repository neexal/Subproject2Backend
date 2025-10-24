namespace IMDB.DataServiceLayer.Models;

public class CastCharacter
{
    public int CastId { get; set; }
    public string CharacterName { get; set; } = string.Empty;
    public int Position { get; set; }

    // Navigation properties
    public CastCredit CastCredit { get; set; } = null!;
}
