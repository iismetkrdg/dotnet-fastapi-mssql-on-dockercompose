namespace Taslak.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    public partial class TracksAudioFeatures
    {
        [JsonPropertyName("audio_features")]
        public AudioFeature[] AudioFeatures { get; set; }
    }
    public partial class AudioFeature
    {
        [JsonPropertyName("danceability")]
        public double Danceability { get; set; }

        [JsonPropertyName("energy")]
        public double Energy { get; set; }

        [JsonPropertyName("key")]
        public long Key { get; set; }

        [JsonPropertyName("loudness")]
        public double Loudness { get; set; }

        [JsonPropertyName("mode")]
        public long Mode { get; set; }

        [JsonPropertyName("speechiness")]
        public double Speechiness { get; set; }

        [JsonPropertyName("acousticness")]
        public double Acousticness { get; set; }

        [JsonPropertyName("instrumentalness")]
        public double Instrumentalness { get; set; }

        [JsonPropertyName("liveness")]
        public double Liveness { get; set; }

        [JsonPropertyName("valence")]
        public double Valence { get; set; }

        [JsonPropertyName("tempo")]
        public double Tempo { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("uri")]
        public Uri Uri { get; set; }

        [JsonPropertyName("track_href")]
        public Uri TrackHref { get; set; }

        [JsonPropertyName("analysis_url")]
        public Uri AnalysisUrl { get; set; }

        [JsonPropertyName("duration_ms")]
        public long DurationMs { get; set; }

        [JsonPropertyName("time_signature")]
        public long TimeSignature { get; set; }
    }

    
}
