namespace IMDB.DataServiceLayer.Models;

public class SearchHistory
{
    public int SearchId { get; set; }
    public int UserId { get; set; }
    public string QueryText { get; set; } = string.Empty;
    public DateTime ExecutedAt { get; set; }
    public int? ResultsCount { get; set; }
    public int? DurationMs { get; set; }

    // Navigation properties
    public AppUser User { get; set; } = null!;
}
