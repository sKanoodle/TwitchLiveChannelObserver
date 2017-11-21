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
            App app = new App();
            app.Run();
            Console.Read();
        }
    }
}
