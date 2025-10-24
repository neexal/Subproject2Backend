namespace IMDB.DataServiceLayer.Models;

public class AltTitleType
{
    public int AltId { get; set; }
    public string TypeName { get; set; } = string.Empty;

    // Navigation properties
    public AlternativeTitle AlternativeTitle { get; set; } = null!;
}
