using IMDB.DataServiceLayer;
using IMDB.WebServiceLayer.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IMDB.WebServiceLayer.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly IDataService _service;
    
    public MovieController(IDataService service) => _service = service;

    [HttpGet]
    public IActionResult GetMovies([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        if (pageSize > 100) pageSize = 50; // Limit page size
        
        var movies = _service.GetMovies(page, pageSize);
        var movieDtos = movies.Select(MapToMovieDto).ToList();
        
        var totalCount = _service.GetTotalMovieCount();
        
        return Ok(new PagedResponse<MovieDto>
        {
            Data = movieDtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }

    [HttpGet("{id:int}")]
    public IActionResult GetMovieById(int id)
    {
        var movie = _service.GetMovieById(id);
        if (movie == null) return NotFound();
        
        return Ok(MapToMovieDto(movie));
    }

    [HttpGet("tconst/{tconst}")]
    public IActionResult GetMovieByTconst(string tconst)
    {
        var movie = _service.GetMovieByTconst(tconst);
        if (movie == null) return NotFound();
        
        return Ok(MapToMovieDto(movie));
    }

    [HttpPost("search")]
    public IActionResult SearchMovies([FromBody] MovieSearchRequest request)
    {
        if (request.PageSize > 100) request.PageSize = 50;
        
        var movies = _service.SearchMovies(request.SearchTerm, request.Page, request.PageSize);
        var movieDtos = movies.Select(MapToMovieDto).ToList();
        
        var totalCount = _service.GetMovieSearchCount(request.SearchTerm);
        
        return Ok(new PagedResponse<MovieDto>
        {
            Data = movieDtos,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        });
    }

    [HttpGet("{id:int}/details")]
    public IActionResult GetMovieDetails(int id)
    {
        var movie = _service.GetMovieWithDetails(id);
        if (movie == null) return NotFound();
        
        return Ok(MapToMovieDetailDto(movie));
    }

    [HttpGet("{id:int}/cast")]
    public IActionResult GetMovieCast(int id)
    {
        var cast = _service.GetMovieCast(id);
        var castDtos = cast.Select(MapToCastCreditDto).ToList();
        
        return Ok(castDtos);
    }

    [HttpGet("{id:int}/crew")]
    public IActionResult GetMovieCrew(int id)
    {
        var crew = _service.GetMovieCrew(id);
        var crewDtos = crew.Select(MapToCrewCreditDto).ToList();
        
        return Ok(crewDtos);
    }

    [HttpGet("{id:int}/genres")]
    public IActionResult GetMovieGenres(int id)
    {
        var genres = _service.GetMovieGenres(id);
        var genreDtos = genres.Select(g => new GenreDto
        {
            GenreId = g.GenreId,
            GenreName = g.GenreName
        }).ToList();
        
        return Ok(genreDtos);
    }

    [HttpGet("{id:int}/alternative-titles")]
    public IActionResult GetMovieAlternativeTitles(int id)
    {
        var altTitles = _service.GetMovieAlternativeTitles(id);
        var altTitleDtos = altTitles.Select(MapToAlternativeTitleDto).ToList();
        
        return Ok(altTitleDtos);
    }

    private MovieDto MapToMovieDto(IMDB.DataServiceLayer.Models.Movie movie)
    {
        return new MovieDto
        {
            MovieId = movie.MovieId,
            Tconst = movie.Tconst,
            TitleType = movie.TitleType,
            PrimaryTitle = movie.PrimaryTitle,
            OriginalTitle = movie.OriginalTitle,
            IsAdult = movie.IsAdult,
            StartYear = movie.StartYear,
            EndYear = movie.EndYear,
            RunTimeMinutes = movie.RunTimeMinutes,
            PlotSummary = movie.PlotSummary,
            PosterUrl = movie.PosterUrl,
            AverageRating = movie.ImdbRating?.Average,
            VoteCount = movie.ImdbRating?.Votes
        };
    }

    private MovieDetailDto MapToMovieDetailDto(IMDB.DataServiceLayer.Models.Movie movie)
    {
        return new MovieDetailDto
        {
            MovieId = movie.MovieId,
            Tconst = movie.Tconst,
            TitleType = movie.TitleType,
            PrimaryTitle = movie.PrimaryTitle,
            OriginalTitle = movie.OriginalTitle,
            IsAdult = movie.IsAdult,
            StartYear = movie.StartYear,
            EndYear = movie.EndYear,
            RunTimeMinutes = movie.RunTimeMinutes,
            PlotSummary = movie.PlotSummary,
            PosterUrl = movie.PosterUrl,
            AverageRating = movie.ImdbRating?.Average,
            VoteCount = movie.ImdbRating?.Votes,
            Genres = movie.MovieGenres.Select(mg => new GenreDto
            {
                GenreId = mg.Genre.GenreId,
                GenreName = mg.Genre.GenreName
            }).ToList(),
            Cast = movie.CastCredits.Select(MapToCastCreditDto).ToList(),
            Crew = movie.CrewCredits.Select(MapToCrewCreditDto).ToList(),
            AlternativeTitles = movie.AlternativeTitles.Select(MapToAlternativeTitleDto).ToList()
        };
    }

    private CastCreditDto MapToCastCreditDto(IMDB.DataServiceLayer.Models.CastCredit castCredit)
    {
        return new CastCreditDto
        {
            CastId = castCredit.CastId,
            PersonId = castCredit.PersonId,
            PersonName = castCredit.Person.PrimaryName,
            Nconst = castCredit.Person.Nconst,
            CastOrder = castCredit.CastOrder,
            Characters = castCredit.CastCharacters.Select(cc => cc.CharacterName).ToList(),
            MovieTitle = castCredit.Movie.PrimaryTitle,
            MovieId = castCredit.MovieId
        };
    }

    private CrewCreditDto MapToCrewCreditDto(IMDB.DataServiceLayer.Models.CrewCredit crewCredit)
    {
        return new CrewCreditDto
        {
            CrewId = crewCredit.CrewId,
            PersonId = crewCredit.PersonId,
            PersonName = crewCredit.Person.PrimaryName,
            Nconst = crewCredit.Person.Nconst,
            Department = crewCredit.Department,
            Job = crewCredit.Job,
            CreditOrder = crewCredit.CreditOrder,
            MovieTitle = crewCredit.Movie.PrimaryTitle,
            MovieId = crewCredit.MovieId
        };
    }

    private AlternativeTitleDto MapToAlternativeTitleDto(IMDB.DataServiceLayer.Models.AlternativeTitle altTitle)
    {
        return new AlternativeTitleDto
        {
            AltId = altTitle.AltId,
            Ordering = altTitle.Ordering,
            Title = altTitle.Title,
            IsOriginalTitle = altTitle.IsOriginalTitle,
            RegionCode = altTitle.RegionCode,
            LanguageCode = altTitle.LanguageCode,
            Types = altTitle.AltTitleTypes.Select(att => att.TypeName).ToList(),
            Attributes = altTitle.AltTitleAttributes.Select(ata => ata.AttributeName).ToList()
        };
    }

    [HttpGet("{id:int}/popular-cast")]
    public IActionResult GetPopularCast(int id)
    {
        var popularCast = _service.GetPopularActorsInMovie(id);
        return Ok(popularCast);
    }

    [HttpGet("{id:int}/similar")]
    public IActionResult GetSimilarMovies(int id)
    {
        var similarMovies = _service.GetSimilarMovies(id);
        return Ok(similarMovies);
    }
}