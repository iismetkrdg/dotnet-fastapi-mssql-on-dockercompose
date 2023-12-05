using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Taslak.Models
{
    public class Recommendations
    {
        [JsonPropertyName("recommendations_id")]
        public string RecommendationsId { get; set; }
        [JsonPropertyName("tracks")]
        public Track[] Tracks { get; set; }
        [JsonPropertyName("seeds")]
        public Seed[] Seeds { get; set; }
    }
    public class Seed
    {
        [JsonPropertyName("initialPoolSize")]
        public int InitialPoolSize { get; set; }
        [JsonPropertyName("afterFilteringSize")]
        public int AfterFilteringSize { get; set; }
        [JsonPropertyName("afterRelinkingSize")]
        public int AfterRelinkingSize { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("href")]
        public string Href { get; set; }
    }
    public class Track
    {
        [JsonPropertyName("album")]
        public Album Album { get; set; }
        [JsonPropertyName("artists")]
        public Artist[] Artists { get; set; }
        [JsonPropertyName("available_markets")]
        public string[] AvailableMarkets { get; set; }
        [JsonPropertyName("disc_number")]
        public int DiscNumber { get; set; }
        [JsonPropertyName("duration_ms")]
        public int DurationMs { get; set; }
        [JsonPropertyName("explicit")]
        public bool Explicit { get; set; }
        [JsonPropertyName("external_ids")]
        public ExternalIds ExternalIds { get; set; }
        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }
        [JsonPropertyName("href")]
        public string Href { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("is_local")]
        public bool IsLocal { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("popularity")]
        public int Popularity { get; set; }
        [JsonPropertyName("preview_url")]
        public string PreviewUrl { get; set; }
        [JsonPropertyName("track_number")]
        public int TrackNumber { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
    

}