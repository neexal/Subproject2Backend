using IMDB.DataServiceLayer;
using IMDB.DataServiceLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Testing;

public class DataServiceTests
{
    [Fact]
    public void GetMovieById_ShouldReturnMovie_WhenMovieExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ImdbContext>()
            .UseInMemoryDatabase(databaseName: "GetMovieById_TestDatabase")
            .Options;

        // Seed the database
        using (var context = new ImdbContext(options))
        {
            context.Movies.Add(new Movie { MovieId = 1, PrimaryTitle = "Test Movie" });
            context.SaveChanges();
        }

        // Use a clean context for the test
        using (var context = new ImdbContext(options))
        {
            var service = new DataService(context);

            // Act
            var result = service.GetMovieById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.MovieId);
            Assert.Equal("Test Movie", result.PrimaryTitle);
        }
    }
}
