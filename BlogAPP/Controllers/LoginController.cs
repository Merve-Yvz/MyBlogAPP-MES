using BlogAPP.Models;
using BlogAPP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using BlogAPP.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserEmail == email && u.UserPassword == password);

            if (appUserInfo != null)
            {
                var role = appUserInfo.Role.RoleName; 
                var jwtToken = _authentication.GenerateJWTAuthentication(email, role);
                var validUserName = _authentication.ValidateToken(jwtToken);

                if (string.IsNullOrEmpty(validUserName))
                {
                    ModelState.AddModelError("", "Unauthorized login attempt");
                    return View();
                }

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    Expires = DateTimeOffset.Now.AddDays(30)
                };
                Response.Cookies.Append("jwt", jwtToken, cookieOptions);

                HttpContext.Session.SetString("UserName", appUserInfo.UserName);
                HttpContext.Session.SetString("UserSurname", appUserInfo.UserSurname);
                HttpContext.Session.SetString("UserID", appUserInfo.UserID.ToString());
                HttpContext.Session.SetString("UserRole", appUserInfo.Role.RoleName);




                return RedirectToAction("Index","Home");
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View();
        }
        public async Task<IActionResult> LogOut()
        {

            Response.Cookies.Delete("jwt");
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }

        //public IActionResult LoggedIn()
        //{
        //    var userName = HttpContext.Session.GetString("UserName");
        //    if (!string.IsNullOrEmpty(userName))
        //    {
        //        ViewBag.UserName = userName;
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login");
        //    }
        //}
    }
}
