using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taslak.Services
{
    public interface ISpotifyAccountService
    {
        Task<string> GetToken(string clientId, string clienSecret);
    }
}