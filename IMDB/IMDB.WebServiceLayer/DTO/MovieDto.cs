namespace IMDB.WebServiceLayer.DTO;

public class MovieDto
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
    public decimal? AverageRating { get; set; }
    public int? VoteCount { get; set; }
    public string Uri => $"/api/movies/{MovieId}";
}

public class PersonDto
{
    public int PersonId { get; set; }
    public string Nconst { get; set; } = string.Empty;
    public string PrimaryName { get; set; } = string.Empty;
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
    public string Uri => $"/api/persons/{PersonId}";
}

public class MovieSearchRequest
{
    public string SearchTerm { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class PersonSearchRequest
{
    public string SearchTerm { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class PagedResponse<T>
{
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
    public string? NextPageUri => HasNextPage ? $"?page={Page + 1}&pageSize={PageSize}" : null;
    public string? PreviousPageUri => HasPreviousPage ? $"?page={Page - 1}&pageSize={PageSize}" : null;
}

// Framework functionality DTOs
public class BookmarkRequest
{
    public int UserId { get; set; }
    public int MovieId { get; set; }
}

public class PersonBookmarkRequest
{
    public int UserId { get; set; }
    public int PersonId { get; set; }
}

public class NoteRequest
{
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public string Note { get; set; } = string.Empty;
}

public class PersonNoteRequest
{
    public int UserId { get; set; }
    public int PersonId { get; set; }
    public string Note { get; set; } = string.Empty;
}

public class RatingRequest
{
    public int UserId { get; set; }
    public int MovieId { get; set; }
    public int Rating { get; set; }
}

public class StructuredSearchRequest
{
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? Plot { get; set; }
    public string? Character { get; set; }
    public string? Person { get; set; }
}

public class NameSearchRequest
{
    public int UserId { get; set; }
    public string SearchString { get; set; } = string.Empty;
}

public class CoPlayerRequest
{
    public string ActorName { get; set; } = string.Empty;
}

public class PersonWordRequest
{
    public string PersonName { get; set; } = string.Empty;
    public int TopN { get; set; } = 20;
}

public class KeywordSearchRequest
{
    public string[] Keywords { get; set; } = Array.Empty<string>();
}
