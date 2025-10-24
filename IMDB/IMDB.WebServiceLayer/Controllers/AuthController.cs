using IMDB.DataServiceLayer;
using IMDB.WebServiceLayer.DTO;
using IMDB.WebServiceLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMDB.WebServiceLayer.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IDataService _dataService;
    private readonly IJwtService _jwtService;
    private readonly IPasswordService _passwordService;

    public AuthController(IDataService dataService, IJwtService jwtService, IPasswordService passwordService)
    {
        _dataService = dataService;
        _jwtService = jwtService;
        _passwordService = passwordService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Username) || 
                string.IsNullOrWhiteSpace(request.Email) || 
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "Username, email, and password are required" });
            }

            if (request.Password.Length < 6)
            {
                return BadRequest(new { Message = "Password must be at least 6 characters long" });
            }

            // Check if user already exists
            var existingUser = _dataService.GetUserByEmail(request.Email);
            if (existingUser != null)
            {
                return Conflict(new { Message = "User with this email already exists" });
            }

            // Hash password
            var hashedPassword = _passwordService.HashPassword(request.Password);

            // Register user
            var userId = _dataService.RegisterUser(request.Username, hashedPassword, request.Email);

            return Ok(new { UserId = userId, Message = "User registered successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "Email and password are required" });
            }

            // Get user by email
            var user = _dataService.GetUserByEmail(request.Email);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            // Verify password
            if (!_passwordService.VerifyPassword(request.Password, user.Password))
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            // Generate JWT token
            var token = _jwtService.GenerateToken(user.UserId.ToString(), user.Username, user.Email);

            // Update last login
            _dataService.UpdateLastLogin(user.UserId);

            // Return token and user info
            return Ok(new AuthResponse
            {
                Token = token,
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60), // Default 60 minutes
                Message = "Login successful"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("change-password")]
    [Authorize]
    public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            // Get user ID from JWT token
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new { Message = "Invalid token" });
            }

            // Validate input
            if (string.IsNullOrWhiteSpace(request.CurrentPassword) || 
                string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest(new { Message = "Current password and new password are required" });
            }

            if (request.NewPassword.Length < 6)
            {
                return BadRequest(new { Message = "New password must be at least 6 characters long" });
            }

            // Get user
            var user = _dataService.GetUserById(userId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Verify current password
            if (!_passwordService.VerifyPassword(request.CurrentPassword, user.Password))
            {
                return BadRequest(new { Message = "Current password is incorrect" });
            }

            // Hash new password
            var hashedNewPassword = _passwordService.HashPassword(request.NewPassword);

            // Update password
            var success = _dataService.UpdateUserPassword(userId, hashedNewPassword);
            if (!success)
            {
                return BadRequest(new { Message = "Failed to update password" });
            }

            return Ok(new { Message = "Password changed successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("verify-token")]
    [Authorize]
    public IActionResult VerifyToken()
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var usernameClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Name);
            var emailClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Email);

            if (userIdClaim == null || usernameClaim == null || emailClaim == null)
            {
                return Unauthorized(new { Message = "Invalid token" });
            }

            return Ok(new
            {
                UserId = int.Parse(userIdClaim.Value),
                Username = usernameClaim.Value,
                Email = emailClaim.Value,
                Message = "Token is valid"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
