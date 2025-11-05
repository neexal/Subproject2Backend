namespace IMDB.DataServiceLayer.Models;

public class AltTitleAttribute
{
    public int AltId { get; set; }
    public string AttributeName { get; set; } = string.Empty;

    // Navigation properties
    public AlternativeTitle AlternativeTitle { get; set; } = null!;
}
