namespace IMDB.WebServiceLayer.DTO;

// Enhanced DTOs for detailed information
public class MovieDetailDto : MovieDto
{
    public ICollection<GenreDto> Genres { get; set; } = new List<GenreDto>();
    public ICollection<CastCreditDto> Cast { get; set; } = new List<CastCreditDto>();
    public ICollection<CrewCreditDto> Crew { get; set; } = new List<CrewCreditDto>();
    public ICollection<AlternativeTitleDto> AlternativeTitles { get; set; } = new List<AlternativeTitleDto>();
}

public class PersonDetailDto : PersonDto
{
    public ICollection<ProfessionDto> Professions { get; set; } = new List<ProfessionDto>();
    public ICollection<MovieDto> KnownFor { get; set; } = new List<MovieDto>();
    public ICollection<CastCreditDto> RecentMovies { get; set; } = new List<CastCreditDto>();
}

public class GenreDto
{
    public int GenreId { get; set; }
    public string GenreName { get; set; } = string.Empty;
}

public class ProfessionDto
{
    public int ProfessionId { get; set; }
    public string ProfessionName { get; set; } = string.Empty;
}

public class CastCreditDto
{
    public int CastId { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string Nconst { get; set; } = string.Empty;
    public int? CastOrder { get; set; }
    public ICollection<string> Characters { get; set; } = new List<string>();
    public string MovieTitle { get; set; } = string.Empty;
    public string MovieUri => $"/api/movies/{MovieId}";
    public int MovieId { get; set; }
}

public class CrewCreditDto
{
    public int CrewId { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string Nconst { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Job { get; set; } = string.Empty;
    public int? CreditOrder { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string MovieUri => $"/api/movies/{MovieId}";
    public int MovieId { get; set; }
}

public class AlternativeTitleDto
{
    public int AltId { get; set; }
    public int? Ordering { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsOriginalTitle { get; set; }
    public string? RegionCode { get; set; }
    public string? LanguageCode { get; set; }
    public ICollection<string> Types { get; set; } = new List<string>();
    public ICollection<string> Attributes { get; set; } = new List<string>();
}

public class PersonKnownForDto
{
    public int MovieId { get; set; }
    public string Tconst { get; set; } = string.Empty;
    public string PrimaryTitle { get; set; } = string.Empty;
    public int? StartYear { get; set; }
    public decimal? AverageRating { get; set; }
    public int? VoteCount { get; set; }
    public string Uri => $"/api/movies/{MovieId}";
}
