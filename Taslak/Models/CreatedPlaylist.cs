using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Taslak.Models
{
    public class CreatedPlaylist
    {
        [JsonPropertyName("collaborative")]
        public bool Collaborative { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }
        [JsonPropertyName("followers")]
        public Followers Followers { get; set; }
        [JsonPropertyName("href")]
        public string Href { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("images")]
        public Image[] Images { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("owner")]
        public POwner Owner { get; set; }
        [JsonPropertyName("primary_color")]
        public object PrimaryColor { get; set; }
        [JsonPropertyName("public")]
        public bool Public { get; set; }
        [JsonPropertyName("snapshot_id")]
        public string SnapshotId { get; set; }
        [JsonPropertyName("tracks")]
        public UPlaylists Tracks { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("uri")]
        public string Uri { get; set; }

        public class POwner
        {
            [JsonPropertyName("display_name")]
            public string DisplayName { get; set; }
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
}