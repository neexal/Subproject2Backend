using IMDB.DataServiceLayer;
using IMDB.WebServiceLayer.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IMDB.WebServiceLayer.Controllers;

[ApiController]
[Route("api/persons")]
public class PersonController : ControllerBase
{
    private readonly IDataService _service;
    
    public PersonController(IDataService service) => _service = service;

    [HttpGet]
    public IActionResult GetPersons([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        if (pageSize > 100) pageSize = 50; // Limit page size
        
        var persons = _service.GetPersons(page, pageSize);
        var personDtos = persons.Select(MapToPersonDto).ToList();
        
        var totalCount = _service.GetTotalPersonCount();
        
        return Ok(new PagedResponse<PersonDto>
        {
            Data = personDtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }

    [HttpGet("{id:int}")]
    public IActionResult GetPersonById(int id)
    {
        var person = _service.GetPersonById(id);
        if (person == null) return NotFound();
        
        return Ok(MapToPersonDto(person));
    }

    [HttpGet("nconst/{nconst}")]
    public IActionResult GetPersonByNconst(string nconst)
    {
        var person = _service.GetPersonByNconst(nconst);
        if (person == null) return NotFound();
        
        return Ok(MapToPersonDto(person));
    }

    [HttpPost("search")]
    public IActionResult SearchPersons([FromBody] PersonSearchRequest request)
    {
        if (request.PageSize > 100) request.PageSize = 50;

        if (request.UserId.HasValue && request.UserId.Value > 0)
        {
            _service.AddSearchHistory(request.UserId.Value, request.SearchTerm);
        }
        
        var persons = _service.SearchPersons(request.SearchTerm, request.Page, request.PageSize);
        var personDtos = persons.Select(MapToPersonDto).ToList();
        
        var totalCount = _service.GetPersonSearchCount(request.SearchTerm);
        
        return Ok(new PagedResponse<PersonDto>
        {
            Data = personDtos,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        });
    }

    [HttpGet("{id:int}/details")]
    public IActionResult GetPersonDetails(int id)
    {
        var person = _service.GetPersonWithDetails(id);
        if (person == null) return NotFound();
        
        return Ok(MapToPersonDetailDto(person));
    }

    [HttpGet("{id:int}/known-for")]
    public IActionResult GetPersonKnownFor(int id)
    {
        var knownForMovies = _service.GetPersonKnownForMovies(id);
        var knownForDtos = knownForMovies.Select(m => new PersonKnownForDto
        {
            MovieId = m.MovieId,
            Tconst = m.Tconst,
            PrimaryTitle = m.PrimaryTitle,
            StartYear = m.StartYear,
            AverageRating = m.ImdbRating?.Average,
            VoteCount = m.ImdbRating?.Votes
        }).ToList();
        
        return Ok(knownForDtos);
    }

    [HttpGet("{id:int}/recent-movies")]
    public IActionResult GetPersonRecentMovies(int id)
    {
        var person = _service.GetPersonWithDetails(id);
        if (person == null) return NotFound();

        var recentMovies = person.CastCredits
            .OrderByDescending(cc => cc.Movie.StartYear)
            .Take(10)
            .Select(MapToCastCreditDto)
            .ToList();
        
        return Ok(recentMovies);
    }

    private PersonDto MapToPersonDto(IMDB.DataServiceLayer.Models.Person person)
    {
        return new PersonDto
        {
            PersonId = person.PersonId,
            Nconst = person.Nconst,
            PrimaryName = person.PrimaryName,
            BirthYear = person.BirthYear,
            DeathYear = person.DeathYear
        };
    }

    private PersonDetailDto MapToPersonDetailDto(IMDB.DataServiceLayer.Models.Person person)
    {
        return new PersonDetailDto
        {
            PersonId = person.PersonId,
            Nconst = person.Nconst,
            PrimaryName = person.PrimaryName,
            BirthYear = person.BirthYear,
            DeathYear = person.DeathYear,
            Professions = person.PersonProfessions.Select(pp => new ProfessionDto
            {
                ProfessionId = pp.Profession.ProfessionId,
                ProfessionName = pp.Profession.ProfessionName
            }).ToList(),
            KnownFor = person.PersonKnownFor.Select(pkf => new MovieDto
            {
                MovieId = pkf.Movie.MovieId,
                Tconst = pkf.Movie.Tconst,
                TitleType = pkf.Movie.TitleType,
                PrimaryTitle = pkf.Movie.PrimaryTitle,
                OriginalTitle = pkf.Movie.OriginalTitle,
                IsAdult = pkf.Movie.IsAdult,
                StartYear = pkf.Movie.StartYear,
                EndYear = pkf.Movie.EndYear,
                RunTimeMinutes = pkf.Movie.RunTimeMinutes,
                PlotSummary = pkf.Movie.PlotSummary,
                PosterUrl = pkf.Movie.PosterUrl,
                AverageRating = pkf.Movie.ImdbRating?.Average,
                VoteCount = pkf.Movie.ImdbRating?.Votes
            }).ToList(),
            RecentMovies = person.CastCredits
                .OrderByDescending(cc => cc.Movie.StartYear)
                .Take(5)
                .Select(MapToCastCreditDto)
                .ToList()
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

    [HttpGet("coplayers/{name}")]
    public IActionResult GetCoPlayers(string name)
    {
        var coPlayers = _service.FindCoPlayers(name);
        return Ok(coPlayers);
    }

    [HttpGet("name/{name}/words")]
    public IActionResult GetPersonWords(string name)
    {
        var words = _service.GetPersonWords(name);
        return Ok(words);
    }
}
