using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Taslak.Models;
using Taslak.Services;
using Taslak.ViewModels;
using Taslak.Data;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
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
        public IActionResult PlaylistUrls(SendUrlsViewModel data)
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
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(data.links), "Geçersiz link!");
                        return View("Index", data);
                    }
                }
                Guid randomGuid = Guid.NewGuid();
                string randomId = randomGuid.ToString();
                var model = new RecommendationModel
                {
                    Guid = randomId,
                    PlaylistIds = ids.ToArray()
                };
                _context.RecommendationModel.Add(model);
                _context.SaveChanges();


                return RedirectToAction("Data", new { id = randomId });
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Data(string id)
        {
            var model = _context.RecommendationModel.FirstOrDefault(x=>x.Guid == id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            var r1 = null as Recommendations;
            foreach (var item in model.PlaylistIds)
            {
                var token = _spotifyAccountService.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);
                var playlist = _spotifyService.GetPlaylist($"/{item}",token.Result);
                var data = _spotifyService.GetRecommendationDataUsingAPlaylist(playlist.Result);
                var r = await  _spotifyService.GetRecommendations(new RecommendationData{
                    seed_tracks = new string[]{playlist.Result.Tracks.Items.First().Track.Id},
                    limit = "15",
                    min_popularity = "10",
                    target_acousticness = data.Result.AudioFeatures.Average(x=>x.Acousticness).ToString().Substring(0,3).Replace(",","."),
                    target_danceability = data.Result.AudioFeatures.Average(x=>x.Danceability).ToString().Substring(0,3).Replace(",","."),
                    target_energy = data.Result.AudioFeatures.Average(x=>x.Energy).ToString().Substring(0,3).Replace(",","."),
                    target_instrumentalness = data.Result.AudioFeatures.Average(x=>x.Instrumentalness).ToString().Substring(0,3).Replace(",","."),
                    target_liveness = data.Result.AudioFeatures.Average(x=>x.Liveness).ToString().Substring(0,3).Replace(",","."),
                    target_loudness = data.Result.AudioFeatures.Average(x=>x.Loudness).ToString().Substring(0,3).Replace(",","."),
                    target_speechiness = data.Result.AudioFeatures.Average(x=>x.Speechiness).ToString().Substring(0,3).Replace(",","."),
                    target_tempo = data.Result.AudioFeatures.Average(x=>x.Tempo).ToString().Substring(0,3).Replace(",","."),
                    target_valence = data.Result.AudioFeatures.Average(x=>x.Valence).ToString().Substring(0,3).Replace(",","."),
                });
                r1 = r;
            }
            ViewBag.Recommendations = r1.Tracks;
            return View();
        }
        public async Task<IActionResult> UserPlaylists()
        {
            if (Request.Cookies["access_token"] == null)
            {
                return RedirectToAction("LoginWithSpotify", "Account");
            }
            var user = _context.User.FirstOrDefault(p => p.Username == User.Identity.Name);
            if (user.Expires_in < DateTime.Now)
            {
                return RedirectToAction("RefreshToken", "Account");
            }
            if (user != null & user.SpotifyToken != null)
            {
                var client = new HttpClient();
                var url = "https://api.spotify.com/v1/me/playlists";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.SpotifyToken);
                var response = await client.SendAsync(request);
                var responseJson = response.Content.ReadAsStringAsync();
                var playlists = JsonConvert.DeserializeObject<UPlaylists>(responseJson.Result);
                ViewBag.Playlists = playlists.Items;
                return View();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RecommendationForSUser(string id)
        {
            var user = _context.User.FirstOrDefault(p => p.Username == User.Identity.Name);
            if (user.Expires_in < DateTime.Now)
            {
                return RedirectToAction("RefreshToken", "Account");
            }
            if (user != null & user.SpotifyToken != null)
            {
                var client = new HttpClient();
                var url = $"https://api.spotify.com/v1/playlists/{id}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.SpotifyToken);
                var response = client.SendAsync(request);
                var responseJson = response.Result.Content.ReadAsStringAsync();
                var playlist = JsonConvert.DeserializeObject<Playlist>(responseJson.Result);
                var data = _spotifyService.GetRecommendationDataUsingAPlaylist(playlist);
                var r = await  _spotifyService.GetRecommendations(new RecommendationData{
                    seed_tracks = new string[]{playlist.Tracks.Items.First().Track.Id},
                    limit = "15",
                    min_popularity = "10",
                    target_acousticness = data.Result.AudioFeatures.Average(x=>x.Acousticness).ToString().Substring(0,3).Replace(",","."),
                    target_danceability = data.Result.AudioFeatures.Average(x=>x.Danceability).ToString().Substring(0,3).Replace(",","."),
                    target_energy = data.Result.AudioFeatures.Average(x=>x.Energy).ToString().Substring(0,3).Replace(",","."),
                    target_instrumentalness = data.Result.AudioFeatures.Average(x=>x.Instrumentalness).ToString().Substring(0,3).Replace(",","."),
                    target_liveness = data.Result.AudioFeatures.Average(x=>x.Liveness).ToString().Substring(0,3).Replace(",","."),
                    target_loudness = data.Result.AudioFeatures.Average(x=>x.Loudness).ToString().Substring(0,3).Replace(",","."),
                    target_speechiness = data.Result.AudioFeatures.Average(x=>x.Speechiness).ToString().Substring(0,3).Replace(",","."),
                    target_tempo = data.Result.AudioFeatures.Average(x=>x.Tempo).ToString().Substring(0,3).Replace(",","."),
                    target_valence = data.Result.AudioFeatures.Average(x=>x.Valence).ToString().Substring(0,3).Replace(",","."),
                });
                var chansons = new ToSaveTracksIds
                {
                    UserId = user.UserID,
                    RecommendationId = Guid.NewGuid().ToString(),
                    PlaylistId = id,
                    TrackIds = r.Tracks.Select(x=>x.Id).ToArray(),
                    At_Created = DateTime.Now
                };
                _context.ToSaveTracksIds.Add(chansons);
                _context.SaveChanges();
                ViewBag.ToSaveTracksIds = chansons.RecommendationId;
                ViewBag.Recommendations = r.Tracks;
                return View();
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CreatePlaylistForMe(string id)
        {
            var user = _context.User.FirstOrDefault(p => p.Username == User.Identity.Name);
            if (user.Expires_in < DateTime.Now)
            {
                return RedirectToAction("RefreshToken", "Account");
            }
            if (user != null & user.SpotifyToken != null)
            {
                var client = new HttpClient();
                var url = $"https://api.spotify.com/v1/users/{user.SpotifyId}/playlists";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.SpotifyToken);
                var body = new
                {
                    name = $"7Taste - {DateTime.Now.ToString("dd/MM/yyyy")}",
                    description = "7Taste tarafından oluşturuldu.",
                };
                var json = JsonConvert.SerializeObject(body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                var responseJson = response.Content.ReadAsStringAsync();
                var playlist = JsonConvert.DeserializeObject<CreatedPlaylist>(responseJson.Result);
                if(response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var ids = _context.ToSaveTracksIds.FirstOrDefault(x=>x.RecommendationId == id);
                    var client2 = new HttpClient();
                    var url2 = $"https://api.spotify.com/v1/playlists/{playlist.Id}/tracks";
                    var request2 = new HttpRequestMessage(HttpMethod.Post, url2);
                    request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.SpotifyToken);
                    var body2 = new
                    {
                        uris = ids.TrackIds.Select(x=>$"spotify:track:{x}").ToArray()
                    };
                    var json2 = JsonConvert.SerializeObject(body2);
                    request2.Content = new StringContent(json2, Encoding.UTF8, "application/json");
                    var response2 = await client2.SendAsync(request2);
                    var responseJson2 = response2.Content.ReadAsStringAsync();
                    var playlist2 = JsonConvert.DeserializeObject<CreatedPlaylist>(responseJson2.Result);
                    user.DiscoveredTrack += ids.TrackIds.Length;
                    user.XTimesUsedAlg += 1;
                    _context.User.Update(user);
                    _context.SaveChanges();
                    if(response2.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("YourPlaylistCreated","x",new { id = playlist.Id });
                    }else{
                        return View("Error");
                    }
                }
                else
                {
                    return View("Error");
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult YourPlaylistCreated(string id){
            ViewBag.PlaylistId = id;
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }
        
    }
}
