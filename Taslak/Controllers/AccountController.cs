using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using Taslak.Data;
using Taslak.Models;
using Taslak.ViewModels;
using System.Text.Json;
using System.Net.Http.Headers;
using Taslak.Services;
namespace Taslak.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService emailService;
        public AccountController(MyDbContext context, IConfiguration configuration, IEmailService emailSender)
        {
            _context = context;
            _configuration = configuration;
            emailService = emailSender;
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
                        new Claim("SpotifyLoggedIn","false")
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
        public async Task<IActionResult> Register(User user)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }else{
                var a = _context.User.FirstOrDefault(p => p.Email == user.Email);
                if(a != null)
                {
                    ModelState.AddModelError(nameof(user.Email),"This email address is already in use.");
                    return View(user);
                }else{
                        string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                        string saltedPassword = user.Password + md5Salt;
                        string hashedPassword = saltedPassword.MD5();
                        user.Role = "user";
                        user.Password = hashedPassword;
                        user.Repassword = hashedPassword;
                        _context.Add(user);
                        await _context.SaveChangesAsync();
                        var text = $"Hi new discoverer,\n\nWelcome to the 7Taste! If you wish, you can pair your 7Taste account with your Spotify account and discover new songs with an easier to use interface.\n\nHappy Discoveries.\n7Taste team";
                        await emailService.SendEmail(user.Username,user.Email,"7Taste Team",text);
                        return RedirectToAction("Login","Account");
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
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel data)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError(nameof(data.Email),"E-mail is required.");
                return View(data);
                
            }else{
                var user = _context.User.FirstOrDefault(p => p.Email == data.Email);
                if(user != null)
                {
                    var newPassword = Guid.NewGuid().ToString().Substring(0,12);
                    string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                    string saltedPassword = newPassword + md5Salt;
                    string hashedPassword = saltedPassword.MD5();
                    user.Password = hashedPassword;
                    user.Repassword = hashedPassword;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    var text = $"Hi {user.Username},\n\nYour new password: {newPassword}\n\nYou can change your password from your profile.\n7Taste Team";
                    await emailService.SendEmail(user.Username,user.Email,"7Taste Team",text);
                    return RedirectToAction("Login","Account");
                }else{
                    ModelState.AddModelError(nameof(data.Email),"Nobody has registered with this e-mail address.");
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
        public async Task<IActionResult> DeleteAccount()
        {
            var user = _context.User.FirstOrDefault(p => p.Username == User.Identity.Name);
            _context.Remove(user);
            await _context.SaveChangesAsync();
            _ = AuthenticationHttpContextExtensions.SignOutAsync(HttpContext
                , CookieAuthenticationDefaults.AuthenticationScheme);
            var text = $"Hi {user.Username},\n\nWe are sorry to see you leave us. We hope you'll be here again soon.\n\nWe deleted all your data. We have no data left about you.\n\n7Taste Team";
            await emailService.SendEmail(user.Username,user.Email,"7Taste Team",text);
            return RedirectToAction(nameof(Logout));
        }
        public IActionResult LoginWithSpotify()
        {
            var ClientId = _configuration.GetValue<string>("Spotify:ClientId");
            var redirectUri = _configuration.GetValue<string>("Spotify:RedirectUri");
            var scopes = "user-read-email+user-read-private+playlist-read-private+playlist-modify-private+playlist-modify-public";  
            var url = "https://accounts.spotify.com/authorize";
            url += "?response_type=code";
            url += "&client_id="+ClientId;
            url += "&scope="+scopes;
            url += "&show_dialog=1";
            url += "&redirect_uri="+redirectUri;
            return Redirect(url);
        }
        public async Task<IActionResult> Callback(string code)
        {
            var ClientId = _configuration.GetValue<string>("Spotify:ClientId");
            var ClientSecret = _configuration.GetValue<string>("Spotify:ClientSecret");
            var redirectUri = _configuration.GetValue<string>("Spotify:RedirectUri");
            var url = "https://accounts.spotify.com/api/token";
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = redirectUri,
                ["client_id"] = ClientId,
                ["client_secret"] = ClientSecret,
            };
            request.Content = new FormUrlEncodedContent(form);
            var response = await client.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<SpotifyToken>(responseJson);
            if (User.Identity.IsAuthenticated)
            {
                var user = _context.User.FirstOrDefault(p => p.Username == User.Identity.Name);
                var client2 = new HttpClient();
                var url2 = "https://api.spotify.com/v1/me";
                var request2 = new HttpRequestMessage(HttpMethod.Get, url2);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
                var response2 = await client2.SendAsync(request2);
                var responseJson2 = await response2.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<SpotifyUserData>(responseJson2);
                user.SpotifyId = data.id;
                user.SpotifyToken = token.access_token.ToString();
                user.Expires_in = DateTime.Now.AddSeconds(token.expires_in);
                user.Refresh_token = token.refresh_token;
                user.Scope = token.scope;
                user.Role = "user";
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(5),
                    HttpOnly = true,
                    Secure = true,
                };
                Response.Cookies.Append("access_token", token.access_token, options);   
                Response.Cookies.Append("refresh_token", token.refresh_token, options);
                Response.Cookies.Append("expires_in", token.expires_in.ToString(), options);
                _context.Update(user);
                _context.SaveChanges();
                var text = $"Hi {user.Username},\n\nYou are connected to 7Taste with your Spotify account.\n\nHappy Discoveries.\n7Taste Team";
                await emailService.SendEmail(user.Username,user.Email,"7Taste Team",text);
                return RedirectToAction("UserPlaylists","x");
            }
            else
            {
                var client2 = new HttpClient();
                var url2 = "https://api.spotify.com/v1/me";
                var request2 = new HttpRequestMessage(HttpMethod.Get, url2);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
                var response2 = await client2.SendAsync(request2);
                var responseJson2 = await response2.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<SpotifyUserData>(responseJson2);
                var user = _context.User.FirstOrDefault(p => p.Email == data.email);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("username",user.Username),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role),
                        new Claim("SpotifyLoggedIn","true")
                    };
                    var claimsIdentity = new ClaimsIdentity(
                        claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    var authproperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.Now.AddDays(30),
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authproperties);
                    user.SpotifyToken = token.access_token.ToString();
                    user.Expires_in = DateTime.Now.AddSeconds(token.expires_in);
                    user.Refresh_token = token.refresh_token;
                    user.Scope = token.scope;
                    user.SpotifyId = data.id;
                    user.Role = "user";
                    var options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(5),
                        HttpOnly = true,
                        Secure = true,
                    };
                    Response.Cookies.Append("access_token", token.access_token, options);   
                    Response.Cookies.Append("refresh_token", token.refresh_token, options);
                    Response.Cookies.Append("expires_in", token.expires_in.ToString(), options);
                    _context.Update(user);
                    _context.SaveChanges();
                    var text = $"Hi {user.Username},\n\nYou have paired your 7Taste account with your Spotify account.\n\nHappy Discoveries.\n7Taste Team";
                    await emailService.SendEmail(user.Username,user.Email,"7Taste Team",text);
                    return RedirectToAction("UserPlaylists","x");
                }
                else
                {
                    var newUser = new User();
                    newUser.Email = data.email;
                    newUser.Username = data.display_name;
                    newUser.Role = "user";
                    newUser.Password = "SpotifyUserasdasdads";
                    newUser.Repassword = "SpotifyUser11111111sadasdhhh";
                    newUser.SpotifyToken = token.access_token.ToString();
                    newUser.Expires_in = DateTime.Now.AddSeconds(token.expires_in);
                    newUser.Refresh_token = token.refresh_token;
                    newUser.Scope = token.scope;
                    newUser.SpotifyId = data.id;
                    var options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(5),
                        HttpOnly = true,
                        Secure = true,
                    };
                    Response.Cookies.Append("access_token", token.access_token, options);   
                    Response.Cookies.Append("refresh_token", token.refresh_token, options);
                    Response.Cookies.Append("expires_in", token.expires_in.ToString(), options);
                    _context.Add(newUser);
                    _context.SaveChanges();
                    var claims = new List<Claim>
                    {
                        new Claim("username",newUser.Username),
                        new Claim(ClaimTypes.Name, newUser.Username),
                        new Claim(ClaimTypes.Role, newUser.Role),
                        new Claim("SpotifyLoggedIn","true")
                    };
                    var claimsIdentity = new ClaimsIdentity(
                        claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    var authproperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.Now.AddDays(30),
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authproperties);
                    var text = $"Hi {user.Username},\n\nWelcome to the 7Taste. It's nice to see you among us. We wish you good discoveries.\n\n7Taste Team";
                    await emailService.SendEmail(user.Username,user.Email,"7Taste Team",text);
                    return RedirectToAction("UserPlaylists","x");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel data)
        {
            System.Console.WriteLine("selam");
            if(!ModelState.IsValid)
            {   
                return Json(new { success = false, message = "Şifreler uyuşmuyor." });
            }else{
                var user = _context.User.FirstOrDefault(p => p.Username == User.Identity.Name);
                string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                string saltedPassword = data.NewP + md5Salt;
                string hashedPassword = saltedPassword.MD5();
                user.Password = hashedPassword;
                user.Repassword = hashedPassword;
                _context.Update(user);
                await _context.SaveChangesAsync();
                var text = $"Hi {user.Username},\n\nYour password has been changed. \n\nHave a nice day.\n7Taste Team";
                await emailService.SendEmail(user.Username,user.Email,"7Taste Team",text);
                return Json(new { success = true, message = "Şifreniz başarıyla değiştirildi." });
            }
        }
        
        public async Task<IActionResult> RefreshToken()
        {
            var ClientId = _configuration.GetValue<string>("Spotify:ClientId");
            var ClientSecret = _configuration.GetValue<string>("Spotify:ClientSecret");
            var redirectUri = _configuration.GetValue<string>("Spotify:RedirectUri");
            var url = "https://accounts.spotify.com/api/token";
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var form = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = Request.Cookies["refresh_token"],
                ["redirect_uri"] = redirectUri,
                ["client_id"] = ClientId,
                ["client_secret"] = ClientSecret,
            };
            request.Content = new FormUrlEncodedContent(form);
            var response = await client.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<SpotifyToken>(responseJson);
            var user = _context.User.FirstOrDefault(p => p.Username == User.Identity.Name);
            user.SpotifyToken = token.access_token.ToString();
            user.Expires_in = DateTime.Now.AddSeconds(token.expires_in);
            user.Refresh_token = token.refresh_token;
            user.Scope = token.scope;
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(5),
                HttpOnly = true,
                Secure = true,
            };
            Response.Cookies.Append("access_token", token.access_token, options);   
            Response.Cookies.Append("refresh_token", token.refresh_token, options);
            Response.Cookies.Append("expires_in", token.expires_in.ToString(), options);
            _context.Update(user);
            _context.SaveChanges();
            return RedirectToAction("UserPlaylists","x");
        }
        
    }
}