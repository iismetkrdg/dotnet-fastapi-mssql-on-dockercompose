using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taslak.Models
{
    public class RecommendationData
    {

        public string[] seed_tracks { get; set; }
        public string[] seed_artists { get; set; }
        public string[] seed_genres { get; set; }
        public string[] target_genres { get; set; }
        public string[] target_artists { get; set; }
        public string[] target_tracks { get; set; }
        public string min_acousticness { get; set; }
        public string max_acousticness { get; set; }
        public string target_acousticness { get; set; }
        public string min_danceability { get; set; }
        public string max_danceability { get; set; }
        public string target_danceability { get; set; }
        public string min_duration_ms { get; set; }
        public string max_duration_ms { get; set; }
        public string target_duration_ms { get; set; }
        public string min_energy { get; set; }
        public string max_energy { get; set; }
        public string target_energy { get; set; }
        public string min_instrumentalness { get; set; }
        public string max_instrumentalness { get; set; }
        public string target_instrumentalness { get; set; }
        public string min_key { get; set; }
        public string max_key { get; set; }
        public string target_key { get; set; }
        public string min_liveness { get; set; }
        public string max_liveness { get; set; }
        public string target_liveness { get; set; }
        public string min_loudness { get; set; }
        public string max_loudness { get; set; }
        public string target_loudness { get; set; }
        public string min_mode { get; set; }
        public string max_mode { get; set; }
        public string target_mode { get; set; }
        public string min_popularity { get; set; }
        public string max_popularity { get; set; }
        public string target_popularity { get; set; }
        public string min_speechiness { get; set; }
        public string max_speechiness { get; set; }
        public string target_speechiness { get; set; }
        public string min_tempo { get; set; }
        public string max_tempo { get; set; }
        public string target_tempo { get; set; }
        public string min_time_signature { get; set; }
        public string max_time_signature { get; set; }
        public string target_time_signature { get; set; }
        public string min_valence { get; set; }
        public string max_valence { get; set; }
        public string target_valence { get; set; }
        public string limit { get; set; }
        public string[] market { get; set; }

        //write a method to convert this object to query string
        public string ToQueryString()
        {
            var properties = this.GetType().GetProperties()
                .Where(x => x.GetValue(this, null) != null)
                .Where(x => x.GetValue(this, null).ToString() != "System.String[]")
                .Select(x => x.Name + "=" + string.Join(",", x.GetValue(this, null)));
            var stringArrays = this.GetType().GetProperties()
                .Where(x => x.GetValue(this, null) != null)
                .Where(x => x.GetValue(this, null).ToString() == "System.String[]")
                .Select(x => 
                {
                    var arrayValue = (string[])x.GetValue(this, null);
                    var arrayAsString = x.Name + "=" + string.Join(",", arrayValue);
                    return arrayAsString;
                });
            return string.Join("&", properties.Concat(stringArrays));
        }
    }
    
}