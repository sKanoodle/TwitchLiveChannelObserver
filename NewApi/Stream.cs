using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchApp.NewApi
{
    class Stream
    {
        /// <summary>
        /// Array of community IDs.
        /// </summary>
        [JsonProperty("community_ids")]
        public string[] CommunityIDs { get; set; }
        /// <summary>
        /// ID of the game being played on the stream.
        /// </summary>
        [JsonProperty("game_id")]
        public string GameID { get; set; }
        /// <summary>
        /// Stream ID.
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// Stream language.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }
        /// <summary>
        /// UTC timestamp.
        /// </summary>
        [JsonProperty("started_at")]
        public DateTime StartedAt { get; set; }
        /// <summary>
        /// Thumbnail URL of the stream. All image URLs have variable width and height. You can replace {width} and {height} with any values to get that size image
        /// </summary>
        [JsonProperty("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// Stream title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// Stream type: "live", "vodcast", "playlist", or "".
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// ID of the user who is streaming.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        /// <summary>
        /// Number of viewers watching the stream at the time of the query.
        /// </summary>
        [JsonProperty("viewer_count")]
        public int ViewerCount { get; set; }
    }
}
