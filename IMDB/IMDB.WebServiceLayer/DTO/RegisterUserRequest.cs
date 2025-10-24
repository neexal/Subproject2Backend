namespace IMDB.WebServiceLayer.DTO;

public class RegisterUserRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserValidate
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
}