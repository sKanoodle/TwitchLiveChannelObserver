﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitchApp.NewApi;

namespace TwitchApp
{
    class App
    {
        private readonly Client Client = new Client();
        private Config Config;
        private User Self;
        private int LineCount;
        private bool DoUpdateLineCount = true;

        private bool DoCustomSort => CustomSort != null;
        private Func<ChannelInfo, object> CustomSort;
        private Dictionary<ConsoleKey, (Func<ChannelInfo, object> SortConstraint, string Description)> AvailableSorts = new Dictionary<ConsoleKey, (Func<ChannelInfo, object>, string)>
        {
            [ConsoleKey.A] = (null, "Default"),
            [ConsoleKey.S] = (c => c.Stream?.ViewerCount, "Viewers"),
            [ConsoleKey.D] = (c => c.User?.DisplayName, "Name"),
        };

        private ChannelInfo[] FollowedChannels;
        private ChannelInfo[] PriorityChannels;
        private readonly object Lock = new object();

        public App()
        {
            string configPath = System.IO.Path.GetFullPath("settings.json");
            try
            {
                Config = Config.Parse(configPath);
                Config.PriorityChannels = Config.PriorityChannels.Select(s => s.ToLower()).ToArray();
            }
            catch (System.IO.FileNotFoundException)
            {
                Config.WriteExample(configPath);
                Console.WriteLine($"no config found, see example config: {configPath}.example");
                return;
            }
            Self = Client.GetUserAsync(Config.OwnName).Result;
        }

        public void Run()
        {
            if (Config is null)
                return;
            while (true)
                try
                {
                    AppLoop();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.Read();
                }
        }

        private void AppLoop()
        {
            Console.Clear(); //in case we resume after exception
            Console.WriteLine("collecting data...");
            Update().Wait();

            DateTime nextDrawRun = DateTime.UtcNow;
            DateTime nextUpdateRun = DateTime.UtcNow;
            Console.CursorVisible = false;

            while (true)
            {
                HandleKeyPress();
                if (DateTime.UtcNow.CompareTo(nextDrawRun) > 0)
                {
                    Draw();
                    nextDrawRun = nextDrawRun.AddMilliseconds(500);
                }
                if (DateTime.UtcNow.CompareTo(nextUpdateRun) > 0)
                {
                    Update();
                    nextUpdateRun = nextUpdateRun.AddSeconds(60);
                }
                var time = DateTime.UtcNow;
                Thread.Sleep(Math.Max(0, Math.Min((int)(nextDrawRun - time).TotalMilliseconds, (int)(nextUpdateRun - time).TotalMilliseconds)));
            }
        }

        private void HandleKeyPress()
        {
            if (!Console.KeyAvailable)
                return;

            var key = Console.ReadKey(intercept: true);
            if (AvailableSorts.TryGetValue(key.Key, out var pair))
                CustomSort = pair.SortConstraint;
        }

        private async Task Update()
        {
            List<User> users = new List<User>();
            List<Stream> streams = new List<Stream>();

            UsersFollowsParameters parameters = new UsersFollowsParameters();
            parameters.First = 100;
            parameters.FromID = Self.ID;

            List<Task> tasks = new List<Task>();
            ResponseWrapper<Follow[]> result;
            do
            {
                result = await Client.GetUsersFollowsAsync(parameters);
                if (result.Data.Length > 0)
                {
                    var ids = result.Data.Select(u => u.ToID).ToArray();
                    tasks.Add(Client.GetStreamsAsync(new StreamsParameters { UserIDs = ids }).ContinueWith(t =>
                        {
                            lock (streams)
                                streams.AddRange(t.Result.Data);
                        }));
                    tasks.Add(Client.GetUsersAsync(new UsersParameters { IDs = ids }).ContinueWith(t =>
                        {
                            lock (users)
                                users.AddRange(t.Result.Data);
                        }));
                }
                parameters.After = result.Pagination.Cursor;
            }
            while (result.Data.Length > 0);
            await Task.WhenAll(tasks);

            var gamesTask = Client.GetGamesAsync(new GamesParameters { IDs = streams.Select(s => s.GameID).Distinct() });
            var channels = users.GroupJoin(streams, u => u.ID, s => s.UserID, (u, s) => new ChannelInfo { User = u, Stream = s.SingleOrDefault() }).ToArray();
            var priority = channels
                .Where(c => Config.PriorityChannels.Contains(c.User.DisplayName.ToLower()))
                .OrderBy(c => c.User.DisplayName)
                .ToArray();

            await Task.WhenAll(gamesTask);
            var gamesDict = gamesTask.Result.Data.ToDictionary(g => g.ID, g => g.Name);

            var followed = channels
                .Where(c => !Config.PriorityChannels.Contains(c.User.DisplayName.ToLower()))
                .OrderBy(c => c.GameName)
                .ThenBy(c => c.User.DisplayName)
                .ToArray();

            lock (Lock)
            {
                ChannelInfo.GameNames = gamesDict;
                PriorityChannels = priority;
                FollowedChannels = followed;
            }
        }

        private void Draw()
        {
            int count = 0;
            Console.SetCursorPosition(0, 0);
            lock (Lock)
            {
                foreach (ChannelInfo info in PriorityChannels)
                    info.DrawInConsole(true, ref count);
                Console.WriteLine();
                Interlocked.Increment(ref count);
                foreach (ChannelInfo info in DoCustomSort ? FollowedChannels.OrderBy(CustomSort) : (IEnumerable<ChannelInfo>)FollowedChannels)
                    info.DrawInConsole(false, ref count);
                DrawEmptyLine();
                Console.WriteLine($"Order: {String.Join(", ", AvailableSorts.Select(kvp => $"[{kvp.Key}] ({kvp.Value.Description})"))}");

                int difference= LineCount - count;
                while (difference-- > 0)
                    DrawEmptyLine();
                if (DoUpdateLineCount)
                    LineCount = count;
            }
        }

        private void DrawEmptyLine()
        {
            int count = Console.WindowWidth;
            string filler = new String(' ', count);
            Console.Write(filler);
        }
    }
}
