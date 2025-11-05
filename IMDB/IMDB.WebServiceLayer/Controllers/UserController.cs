using IMDB.DataServiceLayer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public IActionResult GetAllUsers()
    {
        var users = _service.GetUsers();
        var userModel = users.Select(u => new UserModel
        {
            Id = u.UserId,
            Username = u.Username,
            Email = u.Email,
            Status = u.Status
        }).ToList();
        return Ok(userModel);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public IActionResult GetUserById(int id)
    {
        var user = _service.GetUserById(id);
        if (user == null) return NotFound();
        var userModel = new UserModel
        {
            Id = user.UserId,
            Username = user.Username,
            Email = user.Email,
            Status = user.Status
        };
        return Ok(userModel);
        
    }
    [HttpDelete("{id:int}")]
    [Authorize]
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