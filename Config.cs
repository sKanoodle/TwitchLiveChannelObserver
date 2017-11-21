using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchApp
{
    class Config
    {
        public string OwnName { get; set; }
        public bool ObserveFollowedChannels { get; set; }
        public string[] PriorityChannels { get; set; }

        public static Config Parse(string path)
        {
            using (StreamReader reader = new StreamReader(path))
                return JsonConvert.DeserializeObject<Config>(reader.ReadToEnd());
        }

        public static void WriteExample(string path)
        {
            Config example = new Config()
            {
                OwnName = "sevadus",
                ObserveFollowedChannels = true,
                PriorityChannels = new[] { "swift_m0ti0n", "mailand121", "roym18" },
            };
            using (StreamWriter writer = new StreamWriter(File.Create(path + ".example")))
                writer.Write(JsonConvert.SerializeObject(example));
        }
    }
}
