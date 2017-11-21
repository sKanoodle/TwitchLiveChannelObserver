using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchApp.NewApi
{
    class Follow
    {
        /// <summary>
        /// Date and time when the from_id user followed the to_id user.
        /// </summary>
        [JsonProperty("followed_at")]
        public DateTime FollowedAt { get; set; }
        /// <summary>
        /// ID of the user following the to_id user.
        /// </summary>
        [JsonProperty("from_id")]
        public string FromID { get; set; }
        /// <summary>
        /// ID of the user being followed by the from_id user.
        /// </summary>
        [JsonProperty("to_id")]
        public string ToID { get; set; }
    }
}
