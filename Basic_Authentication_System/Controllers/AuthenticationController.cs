using Basic_Authentication_System.Helpers;
using Basic_Authentication_System.Models;
using Basic_Authentication_System.Models.Database;
using Basic_Authentication_System.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Common;
using System.Security.Claims;

namespace Basic_Authentication_System.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly PasswordHelper _passwordHelper;
        private readonly JWTServiceHelper _jwtServiceHelper;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationController(AuthDbContext context, PasswordHelper passwordHelper, JWTServiceHelper jWTServiceHelper, SignInManager<User> signInManager)
        {
            _context = context;
            _passwordHelper = passwordHelper;
            _jwtServiceHelper = jWTServiceHelper;
            _signInManager = signInManager;
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View(new RegisterVM());
        }
        [HttpPost]
        public async Task<IActionResult> RegisterSubmited(RegisterVM user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Something went wrong! Try again...";
                return View(new RegisterVM());
            }
            if(await IsEmailExistAsync(user.Email))
            {
                ViewBag.ErrorMessage = "<strong>Warning!</strong> <br/> This email has already been used! Try another.";
                return View("Register", new RegisterVM());
            }
            var password = _passwordHelper.HashPassword(user.Password);
            var newUser = new User()
            {
                Email = user.Email,
                PasswordHash = password,
                FullName = user.FirstName + " " + user.LastName,
                LockoutEnabled = true,
                RegisteredAt = DateTime.UtcNow
            };
            _context.tblUsers.Add(newUser);
            await _context.SaveChangesAsync();
            ViewBag.Message = "<strong>Congratulations!</strong> <br/> Account has been created successfully.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
           return View(new LoginVM());
        }
        [HttpPost]
        public async Task<IActionResult> LoginSubmited(LoginVM loginVm)
        {
            var maxAccessFail = 3;
            if (!ModelState.IsValid) 
            {
                ViewBag.ErrorMessage = "Something went wrong! Try again...";
                return View("Login", loginVm);
            }
            var user = await _context.tblUsers.FirstOrDefaultAsync(u=>u.Email == loginVm.Email);
            
            if(user == null)
            {
                ViewBag.ErrorMessage = "Invalid Email or Password! Try again...";
                return View("Login", loginVm);
            }
            var isCorrectPassword = _passwordHelper.VerifyPassword(user.PasswordHash, loginVm.Password);
            if (user != null && !isCorrectPassword && maxAccessFail> user.AccessFailedCount)
            {
                
                var lockInMinutes = 5;
                user.AccessFailedCount++;
                if (user.AccessFailedCount >= maxAccessFail)
                {
                    user.LockOutEnd = DateTime.UtcNow.AddMinutes(lockInMinutes);
                }
                await _context.SaveChangesAsync();
                ViewBag.ErrorMessage = $"Invalid Email or Password! You have {maxAccessFail-user.AccessFailedCount} attempt(s).";
                return View("Login", loginVm);
            }

            if (user.LockOutEnd.HasValue && user.LockOutEnd>DateTime.UtcNow)
            {
                ViewBag.ErrorMessage = $"Account is locked until {user.LockOutEnd.Value.ToLocalTime()}.";
                return View("Login", loginVm);
            }

            user.AccessFailedCount = 0;
            await _context.SaveChangesAsync();

            var loggedInUser = await _signInManager.PasswordSignInAsync(user.Email, user.PasswordHash, false, false);
            var token = await _jwtServiceHelper.GenerateTokenAsync(user);
            HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions { HttpOnly = true });

            return RedirectToAction("TokenView", "Authentication");
        }

        public IActionResult TokenView()
        {
            /*ViewBag.Token = TempData["Token"];
            return View();*/

            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.ErrorMessage = "Session expired. Please log in again.";
                return RedirectToAction("Login");
            }

            ViewBag.Token = TempData["Token"]?.ToString();
            if (string.IsNullOrEmpty(ViewBag.Token))
            {
                return RedirectToAction("Login");
            }

            var fullName = User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            /*return Content($"Authenticated user: {fullName} ({email})");*/
            /*ViewBag.Token = TempData["Token"];*/
            return RedirectToAction("Index","Home");
        }
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Test()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userEmail = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
                return Ok($"Authenticated! Email: {userEmail}");
            }
            else
            {
                return Unauthorized("Not Authenticated!");
            }
        }

        [NonAction]
        private async Task<bool> IsEmailExistAsync(string email)
        {
            return await _context.tblUsers.AnyAsync(u => u.Email == email);
        }
    }
}
