using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchApp.ApiV3
{
    class StreamResponse
    {
        [JsonProperty("stream")]
        public Stream Stream { get; set; }
        [JsonProperty("_links")]
        public Links Links { get; set; }
    }

    class StreamsResponse
    {
        [JsonProperty("streams")]
        public Stream[] Streams { get; set; }
        [JsonProperty("_total")]
        public int Total { get; set; }
        [JsonProperty("_links")]
        public Links Links { get; set; }
    }

    class Stream
    {
        [JsonProperty("_id")]
        public long ID { get; set; }
        [JsonProperty("game")]
        public string Game { get; set; }
        [JsonProperty("viewers")]
        public int ViewerCount { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("video_height")]
        public int VideoHeight { get; set; }
        [JsonProperty("average_fps")]
        public float AverageFps { get; set; }
        [JsonProperty("delay")]
        public int Delay { get; set; }
        [JsonProperty("is_playlist")]
        public bool IsPlaylist { get; set; }
        [JsonProperty("_links")]
        public Links Links { get; set; }
        [JsonProperty("preview")]
        public Preview Preview { get; set; }
        [JsonProperty("channel")]
        public Channel Channel { get; set; }
    }

    class Links
    {
        [JsonProperty("self")]
        public string Self { get; set; }
        [JsonProperty("follows")]
        public string Follows { get; set; }
        [JsonProperty("commercial")]
        public string Commercials { get; set; }
        [JsonProperty("stream_key")]
        public string StreamKey { get; set; }
        [JsonProperty("chat")]
        public string Chat { get; set; }
        [JsonProperty("subscriptions")]
        public string Subscriptions { get; set; }
        [JsonProperty("editors")]
        public string Editors { get; set; }
        [JsonProperty("teams")]
        public string Teams { get; set; }
        [JsonProperty("videos")]
        public string Videos { get; set; }
        [JsonProperty("prev")]
        public string Previous { get; set; }
        [JsonProperty("next")]
        public string Next { get; set; }
        [JsonProperty("featured")]
        public string Featured { get; set; }
        [JsonProperty("summary")]
        public string Summary { get; set; }
        [JsonProperty("followed")]
        public string Followed { get; set; }
    }

    class Preview
    {
        [JsonProperty("small")]
        public string Small { get; set; }
        [JsonProperty("medium")]
        public string Medium { get; set; }
        [JsonProperty("large")]
        public string Large { get; set; }
        [JsonProperty("template")]
        public string Template { get; set; }
    }

    class Channel
    {
        [JsonProperty("mature")]
        public bool Mature { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("broadcaster_language")]
        public string BroadcasterLanguage { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("game")]
        public string Game { get; set; }
        [JsonProperty("language")]
        public string Language { get; set; }
        [JsonProperty("_id")]
        public long ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("delay")]
        public string Delay { get; set; }
        [JsonProperty("logo")]
        public string Logo { get; set; }
        [JsonProperty("banner")]
        public string Banner { get; set; }
        [JsonProperty("video_banner")]
        public string VideoBanner { get; set; }
        [JsonProperty("background")]
        public string Background { get; set; }
        [JsonProperty("profile_banner")]
        public string ProfileBanner { get; set; }
        [JsonProperty("profile_banner_background_color")]
        public string ProfileBannerBackgroundColor { get; set; }
        [JsonProperty("partner")]
        public bool IsPartner { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("views")]
        public int ViewCount { get; set; }
        [JsonProperty("followers")]
        public int FollowerCount { get; set; }
        [JsonProperty("_links")]
        public Links Links { get; set; }
    }

    class Follow
    {
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("_links")]
        public Links Links { get; set; }
        [JsonProperty("notifications")]
        public bool SendNotifications { get; set; }
        [JsonProperty("channel")]
        public Channel Channel { get; set; }

        public override string ToString()
        {
            return Channel.DisplayName;
        }
    }

    class FollowResponse
    {
        [JsonProperty("follows")]
        public Follow[] Follows { get; set; }
        [JsonProperty("_total")]
        public int FollowCount { get; set; }
        [JsonProperty("_links")]
        public Links Links { get; set; }
    }
}
