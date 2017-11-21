using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchApp.NewApi
{
    class User
    {
        /// <summary>
        /// User’s broadcaster type: "partner", "affiliate", or "".
        /// </summary>
        [JsonProperty("broadcaster_type")]
        public string BroadcasterType { get; set; }
        /// <summary>
        /// User's channel description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// User's display name.
        /// </summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        /// <summary>
        /// User’s email address. Returned if the request includes the user:read:edit scope.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
        /// <summary>
        /// User's ID.
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// User's login name.
        /// </summary>
        [JsonProperty("login")]
        public string Login { get; set; }
        /// <summary>
        /// URL of the user's offline image.
        /// </summary>
        [JsonProperty("offline_image_url")]
        public string OfflineImageUrl { get; set; }
        /// <summary>
        /// URL of the user's profile image.
        /// </summary>
        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }
        /// <summary>
        /// User’s type: "staff", "admin", "global_mod", or "".
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// Total number of views of the user’s channel.
        /// </summary>
        [JsonProperty("view_count")]
        public string ViewCount { get; set; }
    }
}
