namespace IMDB.DataServiceLayer.Models;

public class UserMovieBookmark
{
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public DateTime BookmarkedAt { get; set; }
    public string? Folder { get; set; }

    // Navigation properties
    public AppUser User { get; set; } = null!;
    public Movie Movie { get; set; } = null!;
}
