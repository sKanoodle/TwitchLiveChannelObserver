using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchApp.NewApi
{
    class Game
    {
        /// <summary>
        /// Template URL for the game’s box art.
        /// </summary>
        [JsonProperty("box_art_url")]
        public string BoxArtUrl { get; set; }
        /// <summary>
        /// Game ID.
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// Game name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
