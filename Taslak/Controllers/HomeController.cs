using System.Collections;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Taslak.Models;
using Taslak.Services;
using Microsoft.AspNetCore.Http;
namespace Taslak.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ISpotifyAccountService _spotifyAccountService;
    private readonly ISpotifyService _spotifyService;

    public HomeController( ISpotifyAccountService spotifyAccountService,IConfiguration configuration,ISpotifyService spotifyService )
    {
        _configuration = configuration;
        _spotifyAccountService = spotifyAccountService;
        _spotifyService = spotifyService;
    }

    public IActionResult Index()
    {

        return View();
    }
    public async Task<IActionResult> AudioFeatures()
    {
        string token = await _spotifyAccountService.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);
        var tracksAudioFeatures = await _spotifyService.GetTracksAudioFeatures(new string[]{"7ouMYWpwJ422jRcDASZB7P","4VqPOruhp5EdPBeR92t6lQ","2takcwOaAZWiXQijPHIx7B","1u9oZzM8CTeCMXsdTXaOtY"}, token);
        ViewBag.TracksAudioFeatures = tracksAudioFeatures.AudioFeatures.ToList();
        return View();
    }
    public async Task<IActionResult> Recommendations()
    {
        string token = await _spotifyAccountService.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);
        var recommendations = await _spotifyService.GetRecommendations(new RecommendationData{
            seed_tracks = new string[]{"6iVUUdlKrs3OWF4tSwIXc7,1i4aPgWCSvl0htp3Izkxct,155kLkYkxwagprqesFC6aK"},
            min_popularity = "50",
            limit = "10"
        });
        ViewBag.Recommendations = recommendations.Tracks;
        return View();
    }
    public async Task<IActionResult> GetPlaylist(string id)
    {
        string SpotifyUrl = id.Split("/").Last();
        string token = await _spotifyAccountService.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);
        var playlist = await _spotifyService.GetPlaylist($"https://open.spotify.com/playlist/{SpotifyUrl}", token);
        ViewBag.Playlist = playlist;

        return View();
    }
    
    
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
