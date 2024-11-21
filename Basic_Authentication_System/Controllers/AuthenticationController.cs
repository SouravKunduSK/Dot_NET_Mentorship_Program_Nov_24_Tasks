using Basic_Authentication_System.Models;
using Basic_Authentication_System.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Basic_Authentication_System.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthDbContext _context;

        public AuthenticationController(AuthDbContext context)
        {
            _context = context;
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
            if (ModelState.IsValid)
            {
                var existsEmail = IsEmailExist(user.Email);
            }
            return View("Register", new RegisterVM());
        }
        [NonAction]
        private bool IsEmailExist(string email)
        {
            var existsEmail = _context.tblUsers.Any(u=>u.Email == email);
            return existsEmail; 
        }
    }
}
