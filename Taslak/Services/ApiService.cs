using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Taslak.Services
{
    public class ApiService : IApiService
    {
        private readonly ILogger<ApiService> _logger;
        private readonly HttpClient _httpClient;

        public ApiService(ILogger<ApiService> logger, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<string> MakeRecommendation(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "makerecommendation/"+id);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine(responseJson);
            return responseJson;
            }
    }
}