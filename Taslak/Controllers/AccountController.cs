using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NETCore.Encrypt.Extensions;
using Taslak.Data;
using Taslak.Models;
using Taslak.ViewModels;

namespace Taslak.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;
        public AccountController(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index","home");
            }
            return View();
        }
        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Login(LoginViewModel data, string ReturnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index","home");
            }
            if (ModelState.IsValid)
            {
                string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                string saltedPassword = data.Password + md5Salt;
                string hashedPassword = saltedPassword.MD5();
                var user = _context.User.FirstOrDefault(p=>p.Email == data.Email && p.Password == hashedPassword);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("username",user.Username),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role),
                    };
                    var claimsIdentity = new ClaimsIdentity(
                        claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    var authproperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.Now.AddDays(30),
                        RedirectUri = ReturnUrl,
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authproperties);
                    user.LastLoginDate2 = user.LastLoginDate;
                    user.LastLoginDate = DateTime.Now;
                    _context.Update(user);
                    _context.SaveChanges();

                    return RedirectToAction("index","home");

                }
                ModelState.AddModelError(nameof(user.Password),"Şifre veya parola hatalı.");
            }

            return View(data);
        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index","home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }else{
                var a = _context.User.FirstOrDefault(p => p.Email == user.Email);
                if(a != null)
                {
                    ModelState.AddModelError(nameof(user.Email),"Bu mail adresi zaten kullanılıyor.");
                    return View(user);
                }else{
                    var b = _context.User.FirstOrDefault(p => p.Email == user.Email);
                    if(a != null)
                    {
                        ModelState.AddModelError(nameof(user.Email),"Bu mail adresi zaten kullanılıyor.");
                        return View(user);
                    }else{
                        string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                        string saltedPassword = user.Password + md5Salt;
                        string hashedPassword = saltedPassword.MD5();
                        user.Role = "user";
                        user.Password = hashedPassword;
                        user.Repassword = hashedPassword;
                        _context.Add(user);
                        _context.SaveChanges();
                        return RedirectToAction("Login","Account");
                    }
                }
            }
        }
        public IActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index","home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPasswordViewModel data)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError(nameof(data.Email),"E-mail zorunludur.");
                return View(data);
            }else{
                var a = _context.User.FirstOrDefault(p => p.Email == data.Email);
                if(a != null)
                {
                    return View("ForgotPassword");
                }else{
                    ModelState.AddModelError(nameof(data.Email),"Bu mail adresi kayıtlı değil.");
                    return View(data);
                }
            }
        }
        public IActionResult Logout()
        {
            AuthenticationHttpContextExtensions.SignOutAsync(HttpContext
                , CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        public IActionResult Profile()
        {
            return View(_context.User.FirstOrDefault(p => p.Username == User.Identity.Name));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAccount()
        {
            var user = _context.User.FirstOrDefault(p => p.Username == User.Identity.Name);
            _context.Remove(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Logout));
        }
        
    }
}