using IMDB.DataServiceLayer;
using IMDB.WebServiceLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMDB.WebServiceLayer.Controllers;

[ApiController]
[Route("api/framework")]
public class FrameworkController : ControllerBase
{
    private readonly IDataService _service;
    
    public FrameworkController(IDataService service) => _service = service;

    // D.1 Basic framework functionality
    [HttpPost("register")]
    public IActionResult RegisterUser([FromBody] RegisterUserRequest request)
    {
        try
        {
            var userId = _service.RegisterUserFunction(request.Email, request.Username, request.Password);
            return Ok(new { UserId = userId, Message = $"User {request.Username} registered successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("bookmarks/movies/toggle")]
    [Authorize]
    public IActionResult ToggleMovieBookmark([FromBody] BookmarkRequest request)
    {
        try
        {
            var result = _service.ToggleMovieBookmark(request.UserId, request.MovieId);
            return Ok(new { Message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("bookmarks/persons/toggle")]
    [Authorize]
    public IActionResult TogglePersonBookmark([FromBody] PersonBookmarkRequest request)
    {
        try
        {
            var result = _service.TogglePersonBookmark(request.UserId, request.PersonId);
            return Ok(new { Message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("notes/movies")]
    [Authorize]
    public IActionResult AddMovieNote([FromBody] NoteRequest request)
    {
        try
        {
            var noteId = _service.AddMovieNote(request.UserId, request.MovieId, request.Note);
            return Ok(new { NoteId = noteId, Message = "Note added successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("notes/persons")]
    [Authorize]
    public IActionResult AddPersonNote([FromBody] PersonNoteRequest request)
    {
        try
        {
            var noteId = _service.AddPersonNote(request.UserId, request.PersonId, request.Note);
            return Ok(new { NoteId = noteId, Message = "Note added successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("bookmarks/movies/{userId}")]
    [Authorize]
    public IActionResult GetUserMovieBookmarks(int userId)
    {
        try
        {
            var bookmarks = _service.GetUserMovieBookmarks(userId);
            return Ok(bookmarks);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("bookmarks/persons/{userId}")]
    public IActionResult GetUserPersonBookmarks(int userId)
    {
        try
        {
            var bookmarks = _service.GetUserPersonBookmarks(userId);
            return Ok(bookmarks);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("notes/{userId}")]
    public IActionResult GetUserNotes(int userId)
    {
        try
        {
            var notes = _service.GetUserNotes(userId);
            return Ok(notes);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("search-history/{userId}")]
    public IActionResult GetSearchHistory(int userId)
    {
        try
        {
            var history = _service.GetSearchHistory(userId);
            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("rating-history/{userId}")]
    public IActionResult GetRatingHistory(int userId)
    {
        try
        {
            var history = _service.GetRatingHistory(userId);
            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // D.2 Simple search
    [HttpPost("search/string")]
    public IActionResult StringSearch([FromBody] NameSearchRequest request)
    {
        try
        {
            var results = _service.StringSearch(request.UserId, request.SearchString);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // D.3 Title rating
    [HttpPost("rate")]
    [Authorize]
    public IActionResult RateMovie([FromBody] RatingRequest request)
    {
        try
        {
            var result = _service.RateMovie(request.UserId, request.MovieId, request.Rating);
            return Ok(new { Message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // D.4 Structured string search
    [HttpPost("search/structured")]
    public IActionResult StructuredStringSearch([FromBody] StructuredSearchRequest request)
    {
        try
        {
            var results = _service.StructuredStringSearch(request.UserId, request.Title, request.Plot, request.Character, request.Person);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // D.5 Finding names
    [HttpPost("search/names")]
    public IActionResult FindName([FromBody] NameSearchRequest request)
    {
        try
        {
            var results = _service.FindName(request.UserId, request.SearchString);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // D.6 Finding co-players
    [HttpPost("search/coplayers")]
    public IActionResult FindCoPlayers([FromBody] CoPlayerRequest request)
    {
        try
        {
            var results = _service.FindCoPlayers(request.ActorName);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // D.8 Popular actors
    [HttpGet("movies/{movieId}/popular-actors")]
    public IActionResult GetPopularActorsInMovie(int movieId)
    {
        try
        {
            var results = _service.GetPopularActorsInMovie(movieId);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // D.9 Similar movies
    [HttpGet("movies/{movieId}/similar")]
    public IActionResult GetSimilarMovies(int movieId)
    {
        try
        {
            var results = _service.GetSimilarMovies(movieId);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // D.10 Person words
    [HttpPost("search/person-words")]
    public IActionResult GetPersonWords([FromBody] PersonWordRequest request)
    {
        try
        {
            var results = _service.GetPersonWords(request.PersonName, request.TopN);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // D.11 Exact match querying
    [HttpPost("search/exact-match")]
    public IActionResult ExactMatchTitles([FromBody] KeywordSearchRequest request)
    {
        try
        {
            var results = _service.ExactMatchTitles(request.Keywords);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // D.12 Best match querying
    [HttpPost("search/best-match")]
    public IActionResult BestMatchTitles([FromBody] KeywordSearchRequest request)
    {
        try
        {
            var results = _service.BestMatchTitles(request.Keywords);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // D.13 Word-to-words querying
    [HttpPost("search/keyword-expansion")]
    public IActionResult GetKeywordExpansionWords([FromBody] KeywordSearchRequest request)
    {
        try
        {
            var results = _service.GetKeywordExpansionWords(request.Keywords);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
