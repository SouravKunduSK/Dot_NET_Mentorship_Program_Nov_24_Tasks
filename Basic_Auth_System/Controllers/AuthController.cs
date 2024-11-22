using Basic_Auth_System.Models;
using Basic_Auth_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Basic_Auth_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;
        public AuthController(AuthDbContext context, 
            PasswordService passwordService, JwtService jwtService)
        {
                _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            // Password validation regex: at least one lowercase, one uppercase, one number, one special character
            var passwordRegex = new Regex(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9])");

            // Check if the password matches the policy
            if (!passwordRegex.IsMatch(password))
            {
                return BadRequest("Password must contain at least one lowercase letter, one uppercase letter, one number, and one special character.");
            }

            // Check if the username already exists in the database
            if (_context.tblUsers.Any(u => u.Username == username))
            {
                return BadRequest("Username already exists.");
            }


            var hashedPassword = _passwordService.HashPassword(password);
            var user = new User { Username = username, PasswordHash = hashedPassword };
            _context.tblUsers.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.tblUsers.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return Unauthorized("Invalid username or password.");

            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.Now)
                return Forbid($"Account is locked until {user.LockoutEnd}.");

            if (!_passwordService.ValidatePassword(user.PasswordHash, password))
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= 3)
                {
                    user.LockoutEnd = DateTime.Now.AddMinutes(5);
                    user.FailedLoginAttempts = 0;
                }
                await _context.SaveChangesAsync();
                return Unauthorized("Invalid username or password.");
            }

            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user.Username);
            return Ok(new { Token = token, User = user });
        }

        [Authorize]
        [HttpGet("secure-data")]
        public IActionResult SecureData()
        {
            return Ok("You have access to this secured endpoint!");
        }
    }
}
