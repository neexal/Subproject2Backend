namespace IMDB.DataServiceLayer.Models;

public class UserPersonBookmark
{
    public int UserId { get; set; }
    public int PersonId { get; set; }
    public DateTime BookmarkedAt { get; set; }
    public string? Folder { get; set; }

    // Navigation properties
    public AppUser User { get; set; } = null!;
    public Person Person { get; set; } = null!;
}
