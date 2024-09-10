using BlogAPP.Models;
using BlogAPP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPP.Data;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace BlogAPP.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;
        private readonly Authentication _authentication;
		private readonly ILogger<User> _logger;
		private readonly IDistributedCache _cache;



		public LoginController(IConfiguration configuration, ApplicationDbContext dbContext, Authentication authentication, ILogger<User> logger, IDistributedCache cache)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _authentication = authentication;
			_logger = logger;
			_cache = cache;


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
				var name = appUserInfo.UserName + " " + appUserInfo.UserSurname;
				var tokenString = _authentication.GenerateJwtToken(email, role, name);

				
				var cacheOptions = new DistributedCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(Convert.ToDouble(_configuration["Jwt:Expires"]))
				};
				await _cache.SetStringAsync($"UserToken:{email}", tokenString, cacheOptions);

				var cookieOptions = new CookieOptions
				{
					HttpOnly = true,
					Secure = true,
					Expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:Expires"])),
					SameSite = SameSiteMode.Strict
				};

				Response.Cookies.Append("AuthToken", tokenString, cookieOptions);
				HttpContext.Session.SetString("UserName", appUserInfo.UserName);
				HttpContext.Session.SetString("UserSurname", appUserInfo.UserSurname);
				HttpContext.Session.SetString("UserID", appUserInfo.UserID.ToString());
				HttpContext.Session.SetString("UserRole", appUserInfo.Role.RoleName);

				return RedirectToAction("Index", "Home");
			}
			else
			{
				_logger.LogWarning("Failed login attempt for {UserEmail}.", email);
				return Unauthorized();
			}
		}

		public async Task<IActionResult> LogOut()
        {
			var email = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (!string.IsNullOrEmpty(email))
			{
				await _cache.RemoveAsync($"UserToken:{email}");
			}
			Response.Cookies.Delete("AuthToken");
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }
      
        
    }

   


}

