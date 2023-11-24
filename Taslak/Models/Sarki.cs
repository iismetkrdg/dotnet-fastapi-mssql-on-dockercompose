// Generated by https://quicktype.io

namespace Taslak.Models
{
    using System;
    using System.Collections.Generic;

    using System.Text.Json;
    using System.Text.Json.Serialization;

    public partial class Sarki
    {
        [JsonPropertyName("album")]
        public Album Album { get; set; }

        [JsonPropertyName("artists")]
        public Artist[] Artists { get; set; }

        [JsonPropertyName("available_markets")]
        public string[] AvailableMarkets { get; set; }

        [JsonPropertyName("disc_number")]
        public long DiscNumber { get; set; }

        [JsonPropertyName("duration_ms")]
        public long DurationMs { get; set; }

        [JsonPropertyName("explicit")]
        public bool Explicit { get; set; }

        [JsonPropertyName("external_ids")]
        public ExternalIds ExternalIds { get; set; }

        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonPropertyName("href")]
        public Uri Href { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("is_local")]
        public bool IsLocal { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("popularity")]
        public long Popularity { get; set; }

        [JsonPropertyName("preview_url")]
        public Uri PreviewUrl { get; set; }

        [JsonPropertyName("track_number")]
        public long TrackNumber { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("uri")]
        public Uri Uri { get; set; }
    }
    public partial class Album
    {
        [JsonPropertyName("album_type")]
        public string AlbumType { get; set; }

        [JsonPropertyName("artists")]
        public Artist[] Artists { get; set; }

        [JsonPropertyName("available_markets")]
        public string[] AvailableMarkets { get; set; }

        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonPropertyName("href")]
        public Uri Href { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("images")]
        public Image[] Images { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; }

        [JsonPropertyName("release_date_precision")]
        public string ReleaseDatePrecision { get; set; }

        [JsonPropertyName("total_tracks")]
        public long TotalTracks { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("uri")]
        public Uri Uri { get; set; }
    }
    public partial class Artist
    {
        [JsonPropertyName("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonPropertyName("href")]
        public Uri Href { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("uri")]
        public Uri Uri { get; set; }
    }
    public partial class ExternalUrls
    {
        [JsonPropertyName("spotify")]
        public Uri Spotify { get; set; }
    }
    public partial class Image
    {
        [JsonPropertyName("height")]
        public long? Height { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("width")]
        public long? Width { get; set; }
    }
    public partial class ExternalIds
    {
        [JsonPropertyName("isrc")]
        public string Isrc { get; set; }
    }
}
