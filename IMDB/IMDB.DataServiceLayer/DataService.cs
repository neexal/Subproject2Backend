using IMDB.DataServiceLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IMDB.DataServiceLayer;

public class DataService : IDataService
{
    private readonly ImdbContext _imdbContext;

    public DataService(ImdbContext imdbContext)
    {
        _imdbContext = imdbContext;
    }

    public int RegisterUser(string username, string password, string email)
    {
        var user = new AppUser
        {
            Username = username,
            Password = password, // This will be hashed in the controller
            Email = email
        };
        _imdbContext.AppUsers.Add(user);
        _imdbContext.SaveChanges();
        return user.UserId;
    }

    public IList<AppUser> GetUsers()
    {
        return _imdbContext.AppUsers.ToList();
    }

    public AppUser? UserLogin(string email, string password)
    {
        return _imdbContext.AppUsers
            .FirstOrDefault(u => u.Email == email && u.Password == password);
    }

    public AppUser? GetUserByEmail(string email)
    {
        return _imdbContext.AppUsers.FirstOrDefault(u => u.Email == email);
    }

    public bool UpdateUserPassword(int userId, string hashedPassword)
    {
        var user = _imdbContext.AppUsers.FirstOrDefault(u => u.UserId == userId);
        if (user != null)
        {
            user.Password = hashedPassword;
            _imdbContext.SaveChanges();
            return true;
        }
        return false;
    }

    public bool UpdateLastLogin(int userId)
    {
        var user = _imdbContext.AppUsers.FirstOrDefault(u => u.UserId == userId);
        if (user != null)
        {
            user.LastLoginAt = System.DateTime.UtcNow;
            _imdbContext.SaveChanges();
            return true;
        }
        return false;
    }

    public AppUser? GetUserById(int id)
    {
        return _imdbContext.AppUsers.FirstOrDefault(u => u.UserId == id);
    }

    public bool DeleteUserById(int id)
    {
        var query = _imdbContext.AppUsers.FirstOrDefault(u => u.UserId == id);
        if (query != null)
        {
             _imdbContext.AppUsers.Remove(query);
             _imdbContext.SaveChanges();
             return true;
        }
        return false;
    }

    public IList<Movie> GetMovies(int page = 1, int pageSize = 50)
    {
        return _imdbContext.Movies
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public Movie? GetMovieById(int id)
    {
        return _imdbContext.Movies.FirstOrDefault(m => m.MovieId == id);
    }

    public Movie? GetMovieByTconst(string tconst)
    {
        return _imdbContext.Movies.FirstOrDefault(m => m.Tconst == tconst);
    }

    public IList<Movie> SearchMovies(string searchTerm, int page = 1, int pageSize = 50)
    {
        return _imdbContext.Movies
            .Where(m => EF.Functions.ILike(m.PrimaryTitle, $"%{searchTerm}%") ||
                       (m.PlotSummary != null && EF.Functions.ILike(m.PlotSummary, $"%{searchTerm}%")))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public IList<Person> GetPersons(int page = 1, int pageSize = 50)
    {
        return _imdbContext.Persons
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public Person? GetPersonById(int id)
    {
        return _imdbContext.Persons.FirstOrDefault(p => p.PersonId == id);
    }

    public Person? GetPersonByNconst(string nconst)
    {
        return _imdbContext.Persons.FirstOrDefault(p => p.Nconst == nconst);
    }

    public IList<Person> SearchPersons(string searchTerm, int page = 1, int pageSize = 50)
    {
        return _imdbContext.Persons
            .Where(p => EF.Functions.ILike(p.PrimaryName, $"%{searchTerm}%"))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public Movie? GetMovieWithDetails(int id)
    {
        return _imdbContext.Movies
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .Include(m => m.CastCredits)
                .ThenInclude(cc => cc.Person)
            .Include(m => m.CastCredits)
                .ThenInclude(cc => cc.CastCharacters)
            .Include(m => m.CrewCredits)
                .ThenInclude(cc => cc.Person)
            .Include(m => m.AlternativeTitles)
            .Include(m => m.ImdbRating)
            .FirstOrDefault(m => m.MovieId == id);
    }

    public Person? GetPersonWithDetails(int id)
    {
        return _imdbContext.Persons
            .Include(p => p.PersonProfessions)
                .ThenInclude(pp => pp.Profession)
            .Include(p => p.PersonKnownFor)
                .ThenInclude(pkf => pkf.Movie)
            .Include(p => p.CastCredits)
                .ThenInclude(cc => cc.Movie)
            .Include(p => p.CrewCredits)
                .ThenInclude(cc => cc.Movie)
            .FirstOrDefault(p => p.PersonId == id);
    }

    public IList<Movie> GetPersonKnownForMovies(int personId)
    {
        return _imdbContext.PersonKnownFor
            .Where(pkf => pkf.PersonId == personId)
            .Select(pkf => pkf.Movie)
            .Include(m => m.ImdbRating)
            .ToList();
    }

    public IList<CastCredit> GetMovieCast(int movieId)
    {
        return _imdbContext.CastCredits
            .Where(cc => cc.MovieId == movieId)
            .Include(cc => cc.Person)
            .Include(cc => cc.CastCharacters)
            .OrderBy(cc => cc.CastOrder)
            .ToList();
    }

    public IList<CrewCredit> GetMovieCrew(int movieId)
    {
        return _imdbContext.CrewCredits
            .Where(cc => cc.MovieId == movieId)
            .Include(cc => cc.Person)
            .OrderBy(cc => cc.Department)
            .ThenBy(cc => cc.Job)
            .ToList();
    }

    public IList<Genre> GetMovieGenres(int movieId)
    {
        return _imdbContext.MovieGenres
            .Where(mg => mg.MovieId == movieId)
            .Select(mg => mg.Genre)
            .ToList();
    }

    public IList<AlternativeTitle> GetMovieAlternativeTitles(int movieId)
    {
        return _imdbContext.AlternativeTitles
            .Where(at => at.MovieId == movieId)
            .Include(at => at.AltTitleTypes)
            .Include(at => at.AltTitleAttributes)
            .ToList();
    }

    public int GetTotalMovieCount()
    {
        return _imdbContext.Movies.Count();
    }

    public int GetTotalPersonCount()
    {
        return _imdbContext.Persons.Count();
    }

    public int GetMovieSearchCount(string searchTerm)
    {
        return _imdbContext.Movies
            .Where(m => EF.Functions.ILike(m.PrimaryTitle, $"%{searchTerm}%") ||
                       (m.PlotSummary != null && EF.Functions.ILike(m.PlotSummary, $"%{searchTerm}%")))
            .Count();
    }

    public int GetPersonSearchCount(string searchTerm)
    {
        return _imdbContext.Persons
            .Where(p => EF.Functions.ILike(p.PrimaryName, $"%{searchTerm}%"))
            .Count();
    }

    public int RegisterUserFunction(string email, string username, string password)
    {
        return _imdbContext.Database.SqlQueryRaw<int>(
            "SELECT register_user({0}, {1}, {2}) AS result", email, username, password).First();
    }

    public string ToggleMovieBookmark(int userId, int movieId)
    {
        var existingBookmark = _imdbContext.UserMovieBookmarks
            .FirstOrDefault(umb => umb.UserId == userId && umb.MovieId == movieId);

        if (existingBookmark != null)
        {
            _imdbContext.UserMovieBookmarks.Remove(existingBookmark);
            _imdbContext.SaveChanges();
            return "Movie bookmark removed";
        }
        else
        {
            var newBookmark = new UserMovieBookmark
            {
                UserId = userId,
                MovieId = movieId,
                BookmarkedAt = System.DateTime.UtcNow
            };
            _imdbContext.UserMovieBookmarks.Add(newBookmark);
            _imdbContext.SaveChanges();
            return "Movie bookmarked";
        }
    }

    public string TogglePersonBookmark(int userId, int personId)
    {
        return _imdbContext.Database.SqlQueryRaw<string>(
            "SELECT toggle_person_bookmark({0}, {1}) AS result", userId, personId).First();
    }

    public int AddMovieNote(int userId, int movieId, string note)
    {
        return _imdbContext.Database.SqlQueryRaw<int>(
            "SELECT add_movie_note({0}, {1}, {2}) AS result", userId, movieId, note).First();
    }

    public int AddPersonNote(int userId, int personId, string note)
    {
        return _imdbContext.Database.SqlQueryRaw<int>(
            "SELECT add_person_note({0}, {1}, {2}) AS result", userId, personId, note).First();
    }

    public IList<UserMovieBookmarkResult> GetUserMovieBookmarks(int userId)
    {
        return _imdbContext.Database.SqlQueryRaw<UserMovieBookmarkResult>(
            "SELECT movie_id AS MovieId, primary_title AS PrimaryTitle, start_year AS StartYear, bookmarked_at AS BookmarkedAt FROM get_user_movie_bookmarks({0})", userId).ToList();
    }

    public IList<UserPersonBookmarkResult> GetUserPersonBookmarks(int userId)
    {
        return _imdbContext.Database.SqlQueryRaw<UserPersonBookmarkResult>(
            "SELECT person_id AS PersonId, primary_name AS PrimaryName, bookmarked_at AS BookmarkedAt FROM get_user_person_bookmarks({0})", userId).ToList();
    }

    public IList<UserNoteResult> GetUserNotes(int userId)
    {
        return _imdbContext.Database.SqlQueryRaw<UserNoteResult>(
            "SELECT note_type AS NoteType, title_or_name AS TitleOrName, note_body AS NoteBody, created_at AS CreatedAt FROM get_user_notes({0})", userId).ToList();
    }

    public IList<SearchHistoryResult> GetSearchHistory(int userId)
    {
        return _imdbContext.Database.SqlQueryRaw<SearchHistoryResult>(
            "SELECT search_id AS SearchId, query_text AS QueryText, executed_at AS ExecutedAt, results_count AS ResultsCount, duration_ms AS DurationMs FROM get_search_history({0})", userId).ToList();
    }

    public IList<UserRatingResult> GetRatingHistory(int userId)
    {
        return _imdbContext.Database.SqlQueryRaw<UserRatingResult>(
            "SELECT movie_id AS MovieId, primary_title AS PrimaryTitle, rating AS Rating, rated_at AS RatedAt FROM get_rating_history({0})", userId).ToList();
    }

    public IList<MovieSearchResult> StringSearch(int userId, string searchString)
    {
        return _imdbContext.Database.SqlQueryRaw<MovieSearchResult>(
            "SELECT tconst, primary_title AS PrimaryTitle FROM string_search({0}, {1})", userId, searchString).ToList();
    }

    public string RateMovie(int userId, int movieId, int rating)
    {
        var existingRating = _imdbContext.UserTitleRatings
            .FirstOrDefault(utr => utr.UserId == userId && utr.MovieId == movieId);

        if (existingRating != null)
        {
            existingRating.Rating = rating;
            existingRating.RatedAt = System.DateTime.UtcNow;
        }
        else
        {
            var newRating = new UserTitleRating
            {
                UserId = userId,
                MovieId = movieId,
                Rating = rating,
                RatedAt = System.DateTime.UtcNow
            };
            _imdbContext.UserTitleRatings.Add(newRating);
        }

        _imdbContext.SaveChanges();

        var avgRating = _imdbContext.UserTitleRatings
            .Where(utr => utr.MovieId == movieId)
            .Average(utr => utr.Rating);

        var voteCount = _imdbContext.UserTitleRatings
            .Count(utr => utr.MovieId == movieId);

        var imdbRating = _imdbContext.ImdbRatings.FirstOrDefault(ir => ir.MovieId == movieId);
        if (imdbRating != null)
        {
            imdbRating.Average = System.Math.Round(avgRating, 1);
            imdbRating.Votes = voteCount;
        }
        else
        {
            var newImdbRating = new ImdbRating
            {
                MovieId = movieId,
                Average = System.Math.Round(avgRating, 1),
                Votes = voteCount
            };
            _imdbContext.ImdbRatings.Add(newImdbRating);
        }

        _imdbContext.SaveChanges();

        return $"Movie {movieId} rated {rating}/10 by user {userId}. New average: {avgRating:F1} ({voteCount} votes)";
    }

    public IList<MovieSearchResult> StructuredStringSearch(int userId, string? title, string? plot, string? character, string? person)
    {
        return _imdbContext.Database.SqlQueryRaw<MovieSearchResult>(
            "SELECT tconst, primary_title AS PrimaryTitle FROM structured_string_search({0}, {1}, {2}, {3}, {4})",
            userId, title ?? "", plot ?? "", character ?? "", person ?? "").ToList();
    }

    public IList<PersonSearchResult> FindName(int userId, string searchString)
    {
        return _imdbContext.Database.SqlQueryRaw<PersonSearchResult>(
            "SELECT nconst, primary_name AS PrimaryName FROM find_name({0}, {1})", userId, searchString).ToList();
    }

    public IList<CoPlayerResult> FindCoPlayers(string actorName)
    {
        return _imdbContext.Database.SqlQueryRaw<CoPlayerResult>(
            "SELECT nconst, primary_name AS PrimaryName, frequency AS Frequency FROM find_coplayers({0})", actorName).ToList();
    }

    public IList<PopularActorResult> GetPopularActorsInMovie(int movieId)
    {
        return _imdbContext.Database.SqlQueryRaw<PopularActorResult>(
            "SELECT nconst, primary_name AS PrimaryName, weighted_average AS WeightedAverage, total_movies AS TotalMovies FROM get_popular_actors_in_movie({0})", movieId).ToList();
    }

    public IList<SimilarMovieResult> GetSimilarMovies(int movieId)
    {
        return _imdbContext.Database.SqlQueryRaw<SimilarMovieResult>(
            "SELECT tconst, primary_title AS PrimaryTitle, shared_genres AS SharedGenres, year_diff AS YearDiff, similarity_score AS SimilarityScore FROM similar_movies_by_genre_year({0})", movieId).ToList();
    }

    public IList<PersonWordResult> GetPersonWords(string personName, int topN = 20)
    {
        return _imdbContext.Database.SqlQueryRaw<PersonWordResult>(
            "SELECT word AS Word, frequency AS Frequency FROM person_words({0}, {1})", personName, topN).ToList();
    }

    public IList<MovieSearchResult> ExactMatchTitles(params string[] keywords)
    {
        var placeholders = string.Join(", ", Enumerable.Range(0, keywords.Length).Select(i => $"{{{i}}}"));
        var sql = $"SELECT tconst, primary_title AS PrimaryTitle FROM exact_match_titles({placeholders})";
        return _imdbContext.Database.SqlQueryRaw<MovieSearchResult>(sql, keywords).ToList();
    }

    public IList<BestMatchResult> BestMatchTitles(params string[] keywords)
    {
        var placeholders = string.Join(", ", Enumerable.Range(0, keywords.Length).Select(i => $"{{{i}}}"));
        var sql = $"SELECT tconst, primary_title AS PrimaryTitle, match_count AS MatchCount FROM best_match_titles({placeholders})";
        return _imdbContext.Database.SqlQueryRaw<BestMatchResult>(sql, keywords).ToList();
    }

    public IList<WordFrequencyResult> GetKeywordExpansionWords(params string[] keywords)
    {
        var placeholders = string.Join(", ", Enumerable.Range(0, keywords.Length).Select(i => $"{{{i}}}"));
        var sql = $"SELECT word AS Word, freq AS Freq FROM keyword_expansion_words({placeholders})";
        return _imdbContext.Database.SqlQueryRaw<WordFrequencyResult>(sql, keywords).ToList();
    }
}
