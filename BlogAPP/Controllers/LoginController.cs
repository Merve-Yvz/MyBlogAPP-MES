using BlogAPP.Models;
using BlogAPP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using BlogAPP.Data;

namespace BlogAPP.Controllers
{
    public class LoginController : Controller
    {
        private readonly Authentication _authentication;
        private readonly ApplicationDbContext _dbContext;

        public LoginController(Authentication authentication, ApplicationDbContext dbContext)
        {
            _authentication = authentication;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var email = user.UserEmail;
            var password = user.UserPassword;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Please enter valid email and password");
                return View();
            }

            var appUserInfo = _dbContext.Users
                .Where(u => u.UserEmail == email && u.UserPassword == password)
                .FirstOrDefault();

            if (appUserInfo != null)
            {
                var role = appUserInfo.Role; // Kullanıcı rolü
                var jwtToken = _authentication.GenerateJWTAuthentication(email, password);
                var validUserName = _authentication.ValidateToken(jwtToken);

                if (string.IsNullOrEmpty(validUserName))
                {
                    ModelState.AddModelError("", "Unauthorized login attempt");
                    return View();
                }

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // HTTPS üzerinden çalışıyorsanız bu seçeneği aktif edin
                    Expires = DateTimeOffset.Now.AddDays(30)
                };
                Response.Cookies.Append("jwt", jwtToken, cookieOptions);

                HttpContext.Session.SetString("UserName", appUserInfo.UserName);
                HttpContext.Session.SetString("UserSurname", appUserInfo.UserSurname);

               

                return RedirectToAction("Index","Home");
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View();
        }

        [HttpGet]
        public IActionResult LoggedIn()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (!string.IsNullOrEmpty(userName))
            {
                ViewBag.UserName = userName;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}
