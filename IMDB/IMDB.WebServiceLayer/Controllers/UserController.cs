using IMDB.DataServiceLayer;
using Microsoft.AspNetCore.Mvc;
using IMDB.WebServiceLayer.DTO;

namespace IMDB.WebServiceLayer.Controllers;

[ApiController]
[Route("api/users")]
public class UserController: ControllerBase
{
    private readonly IDataService _service;
    public UserController(IDataService service) => _service = service;

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _service.GetUsers();
        var userModel = users.Select(u => new UserModel
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            Status = u.Status
        }).ToList();
        return Ok(userModel);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetUserById(int id)
    {
        var user = _service.GetUserById(id);
        if (user == null) return NotFound();
        var userModel = new UserModel
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Status = user.Status
        };
        return Ok(userModel);
        
    }
    [HttpPost("signin")]
    public IActionResult Login([FromBody] UserValidate uservalidate)
    {
        var user = _service.UserLogin(uservalidate.Email, uservalidate.Password);
        if (user != null) return Ok(new { Message = "Login Successful!", UserId = user.Id});
        return Unauthorized(new { Message = "Invalid email or password!"});
    }
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterUserRequest request)
    {
        var userId = _service.RegisterUser(request.Username, request.Password, request.Email);
        return Ok(new { UserId = userId, Message = $"User {request.Username} registered successfully"});
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteUser(int id)
    {
        var result = _service.DeleteUserById(id);
        if (result == true)
        {
            return Ok($"User Deleted Successfully!");
        }
        else
        {
            return BadRequest($"User Not Found!");
        }
    }
}