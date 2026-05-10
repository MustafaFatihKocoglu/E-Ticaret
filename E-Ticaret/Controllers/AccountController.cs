using E_Ticaret.Data;
using E_Ticaret.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);

            if (user != null)
            {
                TempData["User"] = user.FullName; 
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "E-posta veya şifre hatalı";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password)
        {
            var exists = await _db.Users.AnyAsync(x => x.Email == email);
            if (exists)
            {
                ViewBag.Error = "Bu e-posta zaten kayıtlı.";
                return View();
            }

            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Password = password,
                CreatedAt = DateTime.Now
            };

            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            ViewBag.Success = "Kayıt başarılı. Şimdi giriş yapabilirsiniz.";
            return View();
        }

        public IActionResult Logout()
        {
            TempData.Remove("User");
            return RedirectToAction("Index", "Home");
        }
    }
}
