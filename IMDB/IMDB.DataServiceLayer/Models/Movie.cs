namespace IMDB.DataServiceLayer.Models;

public class Movie
{
    public int MovieId { get; set; }
    public string Tconst { get; set; } = string.Empty;
    public string TitleType { get; set; } = string.Empty;
    public string PrimaryTitle { get; set; } = string.Empty;
    public string? OriginalTitle { get; set; }
    public bool IsAdult { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public int? RunTimeMinutes { get; set; }
    public string? PlotSummary { get; set; }
    public string? PosterUrl { get; set; }

    // Navigation properties
    public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    public ICollection<PersonKnownFor> PersonKnownFor { get; set; } = new List<PersonKnownFor>();
    public ICollection<CastCredit> CastCredits { get; set; } = new List<CastCredit>();
    public ICollection<CrewCredit> CrewCredits { get; set; } = new List<CrewCredit>();
    public ICollection<AlternativeTitle> AlternativeTitles { get; set; } = new List<AlternativeTitle>();
    public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
    public ICollection<Episode> ParentEpisodes { get; set; } = new List<Episode>();
    public ImdbRating? ImdbRating { get; set; }
    public ICollection<UserTitleRating> UserTitleRatings { get; set; } = new List<UserTitleRating>();
    public ICollection<UserMovieBookmark> UserMovieBookmarks { get; set; } = new List<UserMovieBookmark>();
    public ICollection<UserTitleNote> UserTitleNotes { get; set; } = new List<UserTitleNote>();
}