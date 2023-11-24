using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Taslak.Models;
using PandasNet;
using Swan.Formatters;
namespace Taslak.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly ILogger<SpotifyService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly IConfiguration _configuration;


        public SpotifyService(ILogger<SpotifyService> logger, HttpClient httpClient, ISpotifyAccountService spotifyAccountService, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _spotifyAccountService = spotifyAccountService;
            _configuration = configuration;
        }
        public async Task<Sarki> GetSarki(string id, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"tracks/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var sarki = JsonSerializer.Deserialize<Sarki>(responseJson);
            return sarki;
        }
        public async Task<TracksAudioFeatures> GetTracksAudioFeatures(string[] id, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"audio-features?ids={string.Join(",", id)}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStreamAsync();
            var sarki = await JsonSerializer.DeserializeAsync<TracksAudioFeatures>(responseJson);
            return sarki;
        }
        public async Task<Recommendations> GetRecommendations(RecommendationData data, string token)
        {
            //create an url with query string
            var url = "recommendations?"+data.ToQueryString();
            //create a request
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            //add token to request
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //send request
            var response = await _httpClient.SendAsync(request);
            //ensure success
            response.EnsureSuccessStatusCode();
            //get response as string
            var responseJson = await response.Content.ReadAsStreamAsync();
            //deserialize response
            var recommendations = await JsonSerializer.DeserializeAsync<Recommendations>(responseJson);
            return recommendations;
        }
        public async Task<Playlist> GetPlaylist(string playlisturl, string token)
        { 
            var id = playlisturl.Split("/").Last();
            var request = new HttpRequestMessage(HttpMethod.Get, $"playlists/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            var playlist = JsonSerializer.Deserialize<Playlist>(responseJson);
            return playlist;
        }
        public async Task<TracksAudioFeatures> GetRecommendationDataUsingAPlaylist(Playlist playlist)
        {
            //create a blank of TracksAudioFeatures
            TracksAudioFeatures tracksAudioFeatures = new TracksAudioFeatures();
            //get all tracks in playlist
            String[] TrackIds = playlist.Tracks.Items.Select(x => x.Track.Id).ToArray();
            //get token
            string token = await _spotifyAccountService.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);
            //get all tracks audio features
            tracksAudioFeatures = await GetTracksAudioFeatures(TrackIds, token);
            System.Console.WriteLine(tracksAudioFeatures.AudioFeatures[0].Acousticness);
            return tracksAudioFeatures;
        }

    }
}
