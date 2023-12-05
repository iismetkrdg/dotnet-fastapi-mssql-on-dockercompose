using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swan.Formatters;

namespace Taslak.Services
{
    public interface IApiService
    {
        Task<string> MakeRecommendation(string id, string random_id);
    }
}