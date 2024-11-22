using Basic_Authentication_System.Helpers;
using Basic_Authentication_System.Models;
using Basic_Authentication_System.Models.Database;
using Basic_Authentication_System.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Basic_Authentication_System.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly PasswordHelper _passwordHelper;

        public AuthenticationController(AuthDbContext context, PasswordHelper passwordHelper)
        {
            _context = context;
            _passwordHelper = passwordHelper;
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
        [NonAction]
        private async Task<bool> IsEmailExistAsync(string email)
        {
            return await _context.tblUsers.AnyAsync(u => u.Email == email);
        }
    }
}
