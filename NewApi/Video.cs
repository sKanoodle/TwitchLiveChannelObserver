using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchApp.NewApi
{
    class Video
    {
        /// <summary>
        /// Date when the video was created.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Description of the video.
        /// </summary>
        [JsonProperty("description")]
        public DateTime Description { get; set; }
        /// <summary>
        /// ID of the video.
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// Language of the video.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }
        /// <summary>
        /// Date when the video was published.
        /// </summary>
        [JsonProperty("published_at")]
        public string PublishedAt { get; set; }
        /// <summary>
        /// Template URL for the thumbnail of the video.
        /// </summary>
        [JsonProperty("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// Title of the video.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// ID of the user who owns the video.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        /// <summary>
        /// Number of times the video has been viewed.
        /// </summary>
        [JsonProperty("view_count")]
        public int ViewCount { get; set; }
    }
}
