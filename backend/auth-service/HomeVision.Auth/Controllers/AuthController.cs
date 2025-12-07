using Microsoft.AspNetCore.Mvc;
using HomeVision.Auth.Models;
using HomeVision.Auth.Services;

namespace HomeVision.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        // In-memory user store for demo - use database in production
        private static List<User> _users = new()
        {
            new User 
            { 
                Id = 1, 
                Username = "admin", 
                Email = "admin@homevision.com",
                FullName = "Administrator",
                PasswordHash = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", // "admin123" hashed
                Role = "Admin"
            }
        };

        public AuthController(TokenService tokenService, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login attempt for user: {Username}", request.Username);

            var user = _users.FirstOrDefault(u => 
                u.Username == request.Username && u.IsActive);

            if (user == null || !_tokenService.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Failed login attempt for user: {Username}", request.Username);
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            user.LastLoginAt = DateTime.UtcNow;

            var token = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var response = new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = new User
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role
                }
            };

            _logger.LogInformation("User {Username} logged in successfully", request.Username);
            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Registration attempt for user: {Username}", request.Username);

            if (_users.Any(u => u.Username == request.Username))
            {
                return BadRequest(new { Message = "Username already exists" });
            }

            if (_users.Any(u => u.Email == request.Email))
            {
                return BadRequest(new { Message = "Email already registered" });
            }

            var newUser = new User
            {
                Id = _users.Count + 1,
                Username = request.Username,
                Email = request.Email,
                PasswordHash = _tokenService.HashPassword(request.Password),
                FullName = request.FullName,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Role = "User"
            };

            _users.Add(newUser);

            var token = _tokenService.GenerateAccessToken(newUser);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var response = new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                User = new User
                {
                    Id = newUser.Id,
                    Username = newUser.Username,
                    Email = newUser.Email,
                    FullName = newUser.FullName,
                    Role = newUser.Role
                }
            };

            _logger.LogInformation("User {Username} registered successfully", request.Username);
            return Created($"/api/users/{newUser.Id}", response);
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            _logger.LogInformation("Token refresh attempt");

            // TODO: Validar refresh token desde la base de datos
            // Para demostración, solo devolver nuevos tokens

            var user = _users.FirstOrDefault();
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid refresh token" });
            }

            var token = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return Ok(new 
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            });
        }

        [HttpGet("validate")]
        public IActionResult ValidateToken([FromHeader(Name = "Authorization")] string authorization)
        {
            if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
            {
                return Unauthorized(new { Message = "Invalid authorization header" });
            }

            var token = authorization.Substring("Bearer ".Length).Trim();
            var principal = _tokenService.ValidateToken(token);

            if (principal == null)
            {
                return Unauthorized(new { Message = "Invalid token" });
            }

            return Ok(new { Valid = true, Claims = principal.Claims.Select(c => new { c.Type, c.Value }) });
        }
    }
}
