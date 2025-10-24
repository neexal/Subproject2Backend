namespace IMDB.DataServiceLayer.Models;

public class UserPersonNote
{
    public long NoteId { get; set; }
    public int UserId { get; set; }
    public int PersonId { get; set; }
    public string NoteBody { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsPrivate { get; set; }

    // Navigation properties
    public AppUser User { get; set; } = null!;
    public Person Person { get; set; } = null!;
}
