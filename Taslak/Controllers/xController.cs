using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EmbedIO.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Swan;
using Swan.Formatters;
using Taslak.Models;
using Taslak.Services;
using Taslak.ViewModels;
using Taslak.Data;
namespace Taslak.Controllers
{
    public class xController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly ISpotifyService _spotifyService;
        private readonly MyDbContext _context;
        private readonly IApiService _apiService;
        public xController( ISpotifyAccountService spotifyAccountService,IConfiguration configuration,ISpotifyService spotifyService,MyDbContext context,IApiService apiService)
        {
            _configuration = configuration;
            _spotifyAccountService = spotifyAccountService;
            _spotifyService = spotifyService;
            _context = context;
            _apiService = apiService;

        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PlaylistUrls(SendUrlsViewModel data)
        {
            if (ModelState.IsValid)
            {
                string[] links = data.links.Split(",");
                List<string> ids = new List<string>();
                foreach (var link in links)
                {
                    if (link.Contains("open.spotify.com/playlist/"))
                    {
                        if (link.Contains("?si="))
                        {
                            ids.Add(link.Split("/").Last().Split("?si=").First());
                        }
                        else
                        {
                            ids.Add(link.Split("/").Last());
                        }
                    }else{
                        ModelState.AddModelError(nameof(data.links),"Geçersiz link!");
                        return View("Index",data);
                    }
                }
                Guid randomGuid = Guid.NewGuid();
                string randomId = randomGuid.ToString();
                var model = new RecommendationModel{
                    Id = randomId,
                    PlaylistIds = ids.ToArray()
                };
                string idsString = string.Join(",",ids);
                var res = await _apiService.MakeRecommendation(idsString,randomId);
                System.Console.WriteLine(res);
                _context.RecommendationModel.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Data",new {});
            }
            return RedirectToAction("Index");
        }
        public IActionResult Data(string id)
        {

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
        
    }
}
