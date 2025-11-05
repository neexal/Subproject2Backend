using IMDB.DataServiceLayer;
using IMDB.DataServiceLayer.Models;
using IMDB.WebServiceLayer.Controllers;
using IMDB.WebServiceLayer.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Testing;

public class MovieControllerTests
{
    [Fact]
    public void GetMovieById_ShouldReturnOk_WhenMovieExists()
    {
        // Arrange
        var mockService = new Mock<IDataService>();
        mockService.Setup(service => service.GetMovieById(1))
            .Returns(new Movie { MovieId = 1, PrimaryTitle = "Test Movie" });

        var controller = new MovieController(mockService.Object);

        // Act
        var result = controller.GetMovieById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var movieDto = Assert.IsType<MovieDto>(okResult.Value);
        Assert.Equal(1, movieDto.MovieId);
        Assert.Equal("Test Movie", movieDto.PrimaryTitle);
    }

    [Fact]
    public void GetMovieById_ShouldReturnNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<IDataService>();
        mockService.Setup(service => service.GetMovieById(It.IsAny<int>()))
            .Returns((Movie?)null);

        var controller = new MovieController(mockService.Object);

        // Act
        var result = controller.GetMovieById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
