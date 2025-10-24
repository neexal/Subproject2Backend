namespace IMDB.DataServiceLayer.Models;

public class AppUser
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string Status { get; set; } = "active";

    // Navigation properties
    public ICollection<SearchHistory> SearchHistories { get; set; } = new List<SearchHistory>();
    public ICollection<UserTitleRating> UserTitleRatings { get; set; } = new List<UserTitleRating>();
    public ICollection<UserMovieBookmark> UserMovieBookmarks { get; set; } = new List<UserMovieBookmark>();
    public ICollection<UserPersonBookmark> UserPersonBookmarks { get; set; } = new List<UserPersonBookmark>();
    public ICollection<UserTitleNote> UserTitleNotes { get; set; } = new List<UserTitleNote>();
    public ICollection<UserPersonNote> UserPersonNotes { get; set; } = new List<UserPersonNote>();
}