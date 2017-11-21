using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchApp.NewApi
{
    public class ResponseWrapper<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        /// <summary>
        /// A cursor value, to be used in a subsequent request to specify the starting point of the next set of results.
        /// </summary>
        [JsonProperty("cursor")]
        public string Cursor { get; set; }
    }
}
