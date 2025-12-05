namespace IMDB.DataServiceLayer.Models;

// Result classes for PostgreSQL function returns
public class UserMovieBookmarkResult
{
    public int MovieId { get; set; }
    public string PrimaryTitle { get; set; } = string.Empty;
    public int? StartYear { get; set; }
    public DateTime BookmarkedAt { get; set; }
}

public class UserPersonBookmarkResult
{
    public int PersonId { get; set; }
    public string PrimaryName { get; set; } = string.Empty;
    public DateTime BookmarkedAt { get; set; }
}

public class UserNoteResult
{
    public string NoteType { get; set; } = string.Empty;
    public string TitleOrName { get; set; } = string.Empty;
    public string NoteBody { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class SearchHistoryResult
{
    public int SearchId { get; set; }
    public string QueryText { get; set; } = string.Empty;
    public DateTime ExecutedAt { get; set; }
    public int? ResultsCount { get; set; }
    public int? DurationMs { get; set; }
}

public class UserRatingResult
{
    public int MovieId { get; set; }
    public string PrimaryTitle { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public DateTime RatedAt { get; set; }
}

public class MovieSearchResult
{
    public string Tconst { get; set; } = string.Empty;
    public string PrimaryTitle { get; set; } = string.Empty;
}

public class PersonSearchResult
{
    public string Nconst { get; set; } = string.Empty;
    public string PrimaryName { get; set; } = string.Empty;
}

public class CoPlayerResult
{
    public string Nconst { get; set; } = string.Empty;
    public string PrimaryName { get; set; } = string.Empty;
    public int Frequency { get; set; }
}

public class PopularActorResult
{
    public string Nconst { get; set; } = string.Empty;
    public string PrimaryName { get; set; } = string.Empty;
    public decimal? WeightedAverage { get; set; }
    public int? TotalMovies { get; set; }
}

public class SimilarMovieResult
{
    public string Tconst { get; set; } = string.Empty;
    public int MovieId { get; set; }
    public string PrimaryTitle { get; set; } = string.Empty;
    public int? SharedGenres { get; set; }
    public int? YearDiff { get; set; }
    public int? SimilarityScore { get; set; }
    public string? PosterUrl { get; set; }
}

public class PersonWordResult
{
    public string Word { get; set; } = string.Empty;
    public int Frequency { get; set; }
}

public class BestMatchResult
{
    public string Tconst { get; set; } = string.Empty;
    public string PrimaryTitle { get; set; } = string.Empty;
    public int MatchCount { get; set; }
}

public class WordFrequencyResult
{
    public string Word { get; set; } = string.Empty;
    public int Freq { get; set; }
}
