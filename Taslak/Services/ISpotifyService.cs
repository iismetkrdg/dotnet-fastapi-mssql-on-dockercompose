using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Taslak.Models;

namespace Taslak.Services
{
    public interface ISpotifyService
    {
        Task<Sarki> GetSarki(string id, string token);
        Task<TracksAudioFeatures> GetTracksAudioFeatures(string[] id, string token);
        Task<Recommendations> GetRecommendations(RecommendationData data, string token);
        Task<Playlist> GetPlaylist(string playlisturl, string token);
        Task<TracksAudioFeatures> GetRecommendationDataUsingAPlaylist(Playlist playlist);
    }
    
}