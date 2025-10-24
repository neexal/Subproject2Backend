namespace IMDB.DataServiceLayer.Models;

public class UserTitleRating
{
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public decimal Rating { get; set; }
    public DateTime RatedAt { get; set; }
    public string? Source { get; set; }

    // Navigation properties
    public AppUser User { get; set; } = null!;
    public Movie Movie { get; set; } = null!;
}
