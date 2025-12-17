using IMDB.DataServiceLayer.Models;
namespace IMDB.DataServiceLayer;

public interface IDataService
{
    // Users
    int RegisterUser(string username, string password, string email);
    IList<AppUser> GetUsers();
    AppUser? UserLogin(string email, string password);
    AppUser? GetUserById(int id);
    bool DeleteUserById(int id);
    
    // Authentication
    AppUser? GetUserByEmail(string email);
    bool UpdateUserPassword(int userId, string hashedPassword);
    bool UpdateLastLogin(int userId);
    
    // Movies
    IList<Movie> GetMovies(int page = 1, int pageSize = 50);
    Movie? GetMovieById(int id);
    Movie? GetMovieByTconst(string tconst);
    IList<Movie> SearchMovies(string searchTerm, int page = 1, int pageSize = 50);
    
    // Persons
    IList<Person> GetPersons(int page = 1, int pageSize = 50);
    Person? GetPersonById(int id);
    Person? GetPersonByNconst(string nconst);
    IList<Person> SearchPersons(string searchTerm, int page = 1, int pageSize = 50);
    
    // Enhanced detailed information methods
    Movie? GetMovieWithDetails(int id);
    Person? GetPersonWithDetails(int id);
    IList<Movie> GetPersonKnownForMovies(int personId);
    IList<CastCredit> GetMovieCast(int movieId);
    IList<CrewCredit> GetMovieCrew(int movieId);
    IList<Genre> GetMovieGenres(int movieId);
    IList<AlternativeTitle> GetMovieAlternativeTitles(int movieId);
    
    // Count methods for proper pagination
    int GetTotalMovieCount();
    int GetTotalPersonCount();
    int GetMovieSearchCount(string searchTerm);
    int GetPersonSearchCount(string searchTerm);
    
    // Framework functionality using PostgreSQL functions
    int RegisterUserFunction(string email, string username, string password);
    string ToggleMovieBookmark(int userId, int movieId);
    string TogglePersonBookmark(int userId, int personId);
    int AddMovieNote(int userId, int movieId, string note);
    int AddPersonNote(int userId, int personId, string note);
    IList<UserMovieBookmarkResult> GetUserMovieBookmarks(int userId);
    IList<UserPersonBookmarkResult> GetUserPersonBookmarks(int userId);
    IList<UserNoteResult> GetUserNotes(int userId);
    IList<SearchHistoryResult> GetSearchHistory(int userId);
    IList<UserRatingResult> GetRatingHistory(int userId);
    IList<MovieSearchResult> StringSearch(int userId, string searchString);
    string RateMovie(int userId, int movieId, int rating);
    UserTitleRating? GetUserMovieRating(int userId, int movieId);
    IList<MovieSearchResult> StructuredStringSearch(int userId, string? title, string? plot, string? character, string? person);
    IList<PersonSearchResult> FindName(int userId, string searchString);
    IList<CoPlayerResult> FindCoPlayers(string actorName);
    IList<PopularActorResult> GetPopularActorsInMovie(int movieId);
    IList<SimilarMovieResult> GetSimilarMovies(int movieId);
    IList<PersonWordResult> GetPersonWords(string personName, int topN = 20);
    IList<MovieSearchResult> ExactMatchTitles(params string[] keywords);
    IList<BestMatchResult> BestMatchTitles(params string[] keywords);
    IList<WordFrequencyResult> GetKeywordExpansionWords(params string[] keywords);
    void AddSearchHistory(int userId, string queryText);
}

