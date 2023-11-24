using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Taslak.Models
{
    public class TrackInPlaylist
    {
        [JsonPropertyName("added_at")]
        public DateTime AddedAt { get; set; }
        [JsonPropertyName("added_by")]
        public PublicUser AddedBy { get; set; }
        [JsonPropertyName("is_local")]
        public bool IsLocal { get; set; }
        [JsonPropertyName("primary_color")]
        public string PrimaryColor { get; set; }
        [JsonPropertyName("track")]
        public Track Track { get; set; }
        
    }
    public class PublicUser
    {
        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }
        [JsonPropertyName("href")]
        public string Href { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
        
}