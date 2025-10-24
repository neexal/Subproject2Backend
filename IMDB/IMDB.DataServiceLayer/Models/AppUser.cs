namespace IMDB.DataServiceLayer.Models;

public class AppUser
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string Status { get; set; } = "active";
}