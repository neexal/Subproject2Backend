# Backend and Database Modifications

## Search Functionality
- **Case-Insensitive Search**: Implemented using `EF.Functions.ILike` in `DataService.cs` for both Movies and Persons. This ensures that search queries are not case-sensitive (e.g., "batman" matches "Batman").

## Bookmarking Functionality
- **Person Bookmarking**: Refactored `TogglePersonBookmark` in `DataService.cs` to use Entity Framework Core directly instead of calling the `toggle_person_bookmark` SQL function. This fixes an issue where the SQL function call was failing or returning unexpected results.

## Advanced Analysis Features
- **Co-Players**: Added `GetCoPlayers` endpoint to `PersonController`, exposing `FindCoPlayers` service method.
- **Person Word Cloud**: Added `GetPersonWords` endpoint to `PersonController`, exposing `GetPersonWords` service method.
- **Popular Cast Ordering**: Added `GetPopularCast` endpoint to `MovieController`, exposing `GetPopularActorsInMovie` service method.
- **Similar Movies**: Added `GetSimilarMovies` endpoint to `MovieController`, exposing `GetSimilarMovies` service method.

## Search History
- **Search Logging**: Implemented `AddSearchHistory` in `DataService.cs` using Entity Framework Core.
- **Controller Integration**: Updated `MovieController` and `PersonController` search endpoints to log searches when a valid `UserId` is provided in the request body.
- **DTO Update**: Added optional `UserId` property to `MovieSearchRequest` and `PersonSearchRequest`.

## User Notes
- **Refactored Note Saving**: Replaced calls to stored procedures `add_movie_note` and `add_person_note` with direct Entity Framework Core insertion into `UserTitleNotes` and `UserPersonNotes` tables.
- **Manual ID Generation**: Implemented manual ID generation (`Max(NoteId) + 1`) for new notes to handle database tables lacking auto-increment configuration. This ensures notes are successfully saved without SQL integration errors.
