namespace IMDB.DataServiceLayer.Models;

public class AlternativeTitle
{
    public int AltId { get; set; }
    public int MovieId { get; set; }
    public int? Ordering { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsOriginalTitle { get; set; }
    public string? RegionCode { get; set; }
    public string? LanguageCode { get; set; }

    // Navigation properties
    public Movie Movie { get; set; } = null!;
    public ICollection<AltTitleType> AltTitleTypes { get; set; } = new List<AltTitleType>();
    public ICollection<AltTitleAttribute> AltTitleAttributes { get; set; } = new List<AltTitleAttribute>();
}
