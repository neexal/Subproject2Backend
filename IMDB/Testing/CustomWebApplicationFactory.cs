using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Testing;

public class CustomWebApplicationFactory : WebApplicationFactory<IMDB.WebServiceLayer.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Configure test services if needed
        });
    }

    public HttpClient CreateClientWithAuth(int userId = 1, string username = "TestUser", string email = "test@example.com")
    {
        var client = CreateClient();
        var token = GenerateTestJwt(userId, username, email);
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    private string GenerateTestJwt(int userId, string username, string email)
    {
        var configuration = Services.GetRequiredService<IConfiguration>();
        var secretKey = configuration["Jwt:SecretKey"] ?? "your-super-secret-key-that-is-at-least-32-characters-long";
        var issuer = configuration["Jwt:Issuer"] ?? "IMDB-API";
        var audience = configuration["Jwt:Audience"] ?? "IMDB-Users";

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
