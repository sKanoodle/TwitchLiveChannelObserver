using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
                try
                {
                    Foo();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.Read();
                }
        }

        private static void Foo()
        {
            Console.Clear();
            Console.WriteLine("collecting data ...");
            Data data = new Data();

            if (false == PrepareData(data)) return;
            DateTime nextDrawRun = DateTime.Now;
            DateTime nextUpdateRun = DateTime.Now;
            Console.CursorVisible = false;

            while (true)
            {
                if (DateTime.Now.CompareTo(nextDrawRun) > 0)
                {
                    Draw(data);
                    nextDrawRun = nextDrawRun.AddMilliseconds(500);
                }
                if (DateTime.Now.CompareTo(nextUpdateRun) > 0)
                {
                    ThreadPool.QueueUserWorkItem(o => Update(data));
                    nextUpdateRun = nextUpdateRun.AddSeconds(60);
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(100));
            }
        }

        private static bool PrepareData(Data data)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (false == File.Exists(configPath))
            {
                Config.WriteExample(configPath);
                Console.WriteLine("no config found, check program dir for an example");
                return Error();
            }
            data.Config = Config.Parse(configPath);

            bool nameError = false;
            for (int i = 0; i < data.Config.PriorityChannels.Length; i++)
            {
                string channelName = data.Config.PriorityChannels[i];
                Channel channel = TwitchClient.GetChannel(channelName);
                if (channel == null)
                {
                    Console.WriteLine($"channel \"{channelName}\" not found");
                    nameError = true;
                    continue;
                }
                data.Config.PriorityChannels[i] = channel.DisplayName;

            }
            if (nameError)
                return Error();

            Update(data);
            return true;
        }

        private static bool Error()
        {
            Console.WriteLine("press any key to exit ...");
            Console.ReadKey(intercept: true);
            return false;
        }

        private static void Update(Data data)
        {
            Stream[] livePriorityChannels = TwitchClient.GetLiveChannels(data.Config.PriorityChannels);
            data.PriorityChannels = data.Config.PriorityChannels
                .Select(c => new ChannelInfo() { Name = c, Stream = livePriorityChannels.FirstOrDefault(s => s.Channel.DisplayName == c) }).ToArray();

            if (!data.Config.ObserveFollowedChannels)
            {
                data.Follows = data.Follows ?? new Follow[0];
                return;
            }

            data.Follows = data.Follows ?? TwitchClient.GetFollows(data.Config.OwnName);
            data.FollowedChannels = TwitchClient
                .GetLiveChannels(data.Follows.Select(f => f.Channel.Name))
                .Distinct()
                .OrderBy(s => s.Game)
                .ThenBy(s => s.Channel.DisplayName)
                .Select(s => new ChannelInfo() { Name = s.Channel.DisplayName, Stream = s })
                .ToArray();
        }

        private static void Draw(Data data)
        {
            lock (data.Lock)
            {
                Console.SetCursorPosition(0, 0);
                foreach (ChannelInfo info in data.PriorityChannels)
                    info.DrawInConsole(true);
                Console.WriteLine();
                foreach (ChannelInfo info in data.FollowedChannels.Where(f => data.PriorityChannels.Any(p => p.Name == f.Name) == false))
                    info.DrawInConsole(false);

                int difference = data.PreviousLineCount - data.LineCount;
                while (difference-- > 0)
                    DrawEmptyLine();
                data.PreviousLineCount = data.LineCount;
            }
        }

        private static void DrawEmptyLine()
        {
            int count = Console.WindowWidth;
            string filler = new String(' ', count);
            Console.Write(filler);
        }
    }

    class IntermediateData
    {
        public object Lock = new object();
        private List<ChannelInfo> _channels = new List<ChannelInfo>();
        public ChannelInfo[] Channels { get { return _channels.ToArray(); } }

        public void AddChannel(ChannelInfo channel)
        {
            lock (Lock)
            {
                _channels.Add(channel);
            }
        }
    }

    class Data
    {
        public int UpdateCount = 0;
        public int PreviousLineCount;
        public object Lock = new object();
        public Config Config;
        public ChannelInfo[] PriorityChannels;
        public ChannelInfo[] FollowedChannels;
        public Follow[] Follows;

        public int LineCount
        {
            get
            {
                lock(Lock)
                    return (PriorityChannels?.Length ?? 0) + (FollowedChannels?.Count(c => c.IsOnline) ?? 0);
            }
        }
    }

    class ChannelInfo
    {
        public string Name;
        public Stream Stream;
        public bool IsOnline
        {
            get
            {
                if (Stream == null) return false;
                return Stream.CreatedAt != DateTime.MinValue;
            }
        }

        public void DrawInConsole(bool drawIfOffline)
        {
            Func<string, int, string> formatString = (s, l) => (s.Length > l - 1 ? s.Substring(0, l - 2) : s).PadLeft(l);

            if (false == (drawIfOffline || IsOnline))
                return;
            SetBackAndForeground(ConsoleColor.Black, ConsoleColor.White);
            Console.Write($"{formatString(Name, Lengths.Name)}: ");

            if (IsOnline)
            {
                SetBackAndForeground(ConsoleColor.DarkGreen, ConsoleColor.White);
                Console.Write(" online ".PadLeft(Lengths.Live));
            }
            else
            {
                SetBackAndForeground(ConsoleColor.DarkRed, ConsoleColor.White);
                Console.Write(" offline ".PadLeft(Lengths.Live));
            }

            string uptimeString = String.Empty;
            string game = String.Empty;
            SetBackAndForeground(ConsoleColor.Black, ConsoleColor.White);
            if (IsOnline)
            {
                TimeSpan uptime = DateTime.UtcNow - Stream.CreatedAt;
                uptimeString = $"{(int)uptime.TotalHours:00}{uptime.ToString("\\:mm\\:ss")}";
                game = Stream.Game;
            }
            Console.Write(uptimeString.PadLeft(Lengths.Uptime));
            Console.Write(formatString(game, Lengths.Game));

            Console.Write('\n');
        }

        private void SetBackAndForeground(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
        }

        private static class Lengths
        {
            public static int Name = 20;
            public static int Live = 9;
            public static int Uptime = 10;
            public static int Game = 30;
        }
    }

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
