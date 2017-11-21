using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchApp.NewApi;

namespace TwitchApp
{
    class ChannelInfo
    {
        public static Dictionary<string, string> GameNames;
        public User User;
        public Stream Stream;
        public bool IsOnline
        {
            get
            {
                if (Stream is null) return false;
                return Stream.StartedAt != DateTime.MinValue;
            }
        }

        public string GameName
        {
            get
            {
                if (Stream is null || GameNames is null) return String.Empty;
                if (GameNames.TryGetValue(Stream.GameID, out string name))
                    return name;
                else
                    return Stream.GameID;
            }
        }

        public void DrawInConsole(bool drawIfOffline, ref int count)
        {
            Func<string, int, string> formatString = (s, l) => (s.Length > l - 1 ? s.Substring(0, l - 2) : s).PadLeft(l);

            if (false == (drawIfOffline || IsOnline))
                return;
            System.Threading.Interlocked.Increment(ref count);
            SetBackAndForeground(ConsoleColor.Black, ConsoleColor.White);
            Console.Write($"{formatString(User.DisplayName, Lengths.Name)}: ");

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
            SetBackAndForeground(ConsoleColor.Black, ConsoleColor.White);
            if (IsOnline)
            {
                TimeSpan uptime = DateTime.UtcNow - Stream.StartedAt;
                uptimeString = $"{(int)uptime.TotalHours:00}{uptime.ToString("\\:mm\\:ss")}";
            }
            Console.Write(uptimeString.PadLeft(Lengths.Uptime));
            Console.Write(formatString(GameName, Lengths.Game));

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
}
