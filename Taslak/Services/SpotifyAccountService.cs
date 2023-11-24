using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Taslak.Models;
using Taslak.Services;

namespace Taslak.Services;

public class SpotifyAccountService : ISpotifyAccountService
{
    private readonly ILogger<SpotifyAccountService> _logger;
    private readonly HttpClient _httpClient;

    public SpotifyAccountService(ILogger<SpotifyAccountService> logger, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    public async Task<string> GetToken(string clientId, string clienSecret){
        var request = new HttpRequestMessage(HttpMethod.Post, "token");
        //basic authentication
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clienSecret}")));
        //form data
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"grant_type", "client_credentials"}
        });
        //send request
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        //get response as string
        using var responseJson = await response.Content.ReadAsStreamAsync();
        //deserialize response
        var authresult = await JsonSerializer.DeserializeAsync<AuthResult>(responseJson);
        //return token
        return authresult.access_token;
        
    }
}
