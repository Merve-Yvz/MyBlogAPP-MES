using BlogAPP.Data;
using BlogAPP.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogAPP.Controllers
{
    [AllowAnonymous]

    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LoginController(ApplicationDbContext db)
        {
            _db= db;    
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
           
            var data = await _db.Users.FirstOrDefaultAsync(x=>x.UserEmail==user.UserEmail && x.UserPassword==user.UserPassword);

            if (data != null)
            {
                //HttpContext.Session.SetString("username", user.UserEmail);
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserEmail) };
                var useridentity = new ClaimsIdentity(claims, "a");
                ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                await HttpContext.SignInAsync(principal);
                var userRoleId = data.RoleID;

                return userRoleId == 1 ? RedirectToAction("Index", "Dashboard") : RedirectToAction("Index", "Home");


            }
            else
            {
                return View();

            }
        }
    }
}
