using System.Net;
using System.Threading.Tasks;

namespace Testing;

public class ApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetMovies_PublicEndpoint_ReturnsSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/movies");

        // Assert
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task GetUsers_ProtectedEndpoint_WithoutToken_ReturnsUnauthorized()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/users");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_ProtectedEndpoint_WithValidToken_ReturnsSuccess()
    {
        // Arrange
        var client = _factory.CreateClientWithAuth();

        // Act
        var response = await client.GetAsync("/api/users");

        // Assert
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
