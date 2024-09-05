﻿using BlogAPP.Models;
using BlogAPP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BlogAPP.Data;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure;

namespace BlogAPP.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;
        private readonly Authentication _authentication;

        public LoginController(IConfiguration configuration, ApplicationDbContext dbContext, Authentication authentication)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _authentication = authentication;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(User user)
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
                var tokenString = _authentication.GenerateJwtToken(email,role);
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
                return Unauthorized();
            }

           
            }
        public IActionResult LogOut()
        {
            Response.Cookies.Delete("AuthToken");
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }
      
        
    }

   


}
