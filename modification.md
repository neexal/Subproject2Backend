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
