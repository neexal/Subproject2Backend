namespace IMDB.DataServiceLayer.Models;

public class Episode
{
    public int MovieId { get; set; }
    public int ParentSeriesId { get; set; }
    public int? SeasonNumber { get; set; }
    public int? EpisodeNumber { get; set; }

    // Navigation properties
    public Movie Movie { get; set; } = null!;
    public Movie ParentSeries { get; set; } = null!;
}
