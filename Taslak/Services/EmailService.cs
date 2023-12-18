using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Taslak.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient ;
        public EmailService(IConfiguration configuration,HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string> SendEmail(string name,string email, string subject, string message)
        {
            var api_key = _configuration["Brevo:ApiKey"];
            var request = new HttpRequestMessage(HttpMethod.Post, "");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("api-key", api_key);
            request.Content = new StringContent(JsonConvert.SerializeObject(new
            {
                sender = new
                {
                    name = "7Taste",
                    email = "noreply@7taste.io"
                },
                to = new[]
                {
                    new
                    {
                        email = email,
                        name = name
                    }
                },
                subject = subject,
                htmlContent = message
            }), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;

        }
    }
}