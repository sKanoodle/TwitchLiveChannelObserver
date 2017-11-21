using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TwitchApp.ApiV3
{
    class TwitchClientV3
    {
        private static string ClientID => ApiKey.ClientID;
        private static string BaseUrl = "https://api.twitch.tv/kraken";

        private static T GetResponse<T>(string endPoint, string stuff)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlCombine(BaseUrl, endPoint, stuff));
            request.Headers.Add("Client-ID", ClientID);

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException)
            {
                return default(T);
            }

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            T result;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                result = JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), settings);
            return result;
        }
        private static void GetResponse(string endPoint, string stuff)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlCombine(BaseUrl, endPoint, stuff));
            request.Headers.Add("Client-ID", ClientID);

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                HttpStatusCode statusCode = ((HttpWebResponse)e.Response).StatusCode;

                switch (statusCode)
                {
                    case (HttpStatusCode)400:
                    case (HttpStatusCode)404:
                    case (HttpStatusCode)500:
                    case (HttpStatusCode)502:
                    case (HttpStatusCode)503:
                    case (HttpStatusCode)504: return;
                    default: break;
                }
                throw;
            }

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            string foo;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                foo = reader.ReadToEnd();
        }


        private static string UrlCombine(params string[] parts)
        {
            var trimmed = parts.Select(s => s?.Trim(' ', '/')).Where(s => false == String.IsNullOrWhiteSpace(s));
            return String.Join("/", trimmed);
        }

        public static bool IsLive(string channel)
        {
            return IsLive(channel, out _);
        }

        public static bool IsLive(string channel, out Stream stream)
        {
            StreamResponse response = GetResponse<StreamResponse>("streams", channel);
            stream = response?.Stream;
            return stream != null;
        }

        public static bool ChannelExists(string channel)
        {
            return GetChannel(channel) != null;
        }

        public static Channel GetChannel(string channel)
        {
            return GetResponse<Channel>("channels", channel);
        }

        public static Follow[] GetFollows(string user, int limit = 25, int offset = 0, string direction = "DESC", string sortBy = "created_at")
        {
            Func<string, FollowResponse> getFollows = s => GetResponse<FollowResponse>("users", s);
            Func<int, string> query = (o) => $"{user}/follows/channels?direction={direction}&limit={limit}&offset={o}&sortby={sortBy}";
            List<Follow> result = new List<Follow>();
            FollowResponse response;

            while ((response = getFollows(query(offset))).Follows.Length > 0)
            {
                result.AddRange(response.Follows);
                offset += limit;
            }

            //if (response.FollowCount != result.Count)
            //    throw new Exception("follows not matching");
            return result.ToArray();
        }

        public static Stream[] GetLiveChannels(IEnumerable<string> channels)
        {
            //string channelString = String.Join(",", channels);
            //int limit = 100;
            //int offset = 0;
            //Func<string, StreamsResponse> getStreams = s => GetResponse<StreamsResponse>(null, s);
            //Func<int, string> query = o => $"streams?channel={channelString}&limit={limit}&offset={o}";
            //List<Stream> result = new List<Stream>();
            //StreamsResponse response;

            //while ((response = getStreams(query(offset))).Streams.Length > 0)
            //{
            //    result.AddRange(response.Streams);
            //    offset += limit;
            //}
            //return result.ToArray();

            string channelString = String.Join(",", channels);
            Func<string, StreamsResponse> getStreams = s => GetResponse<StreamsResponse>(null, s);
            Func<int, int, string> query = (l, o) => $"streams?channel={channelString}&limit={l}&offset={o}";
            Func<StreamsResponse, Stream[]> getData = r => r.Streams ?? new Stream[] { };

            return Loop(getStreams, query, getData);
        }

        private static T[] Loop<T, U>(Func<string, U> getResponse, Func<int, int, string> getQuery, Func<U, T[]> getData, int offset = 0, int limit = 100)
        {
            List<T> result = new List<T>();
            T[] data;
            int remainingTries = 10;
            int start = offset;
            bool error = true;
            while (error && remainingTries > 0)
            {
                error = false;
                remainingTries -= 1;
                offset = start;
                do
                {
                    U response = getResponse(getQuery(limit, offset));
                    bool? foo = response?.Equals(default(U));
                    if (!foo.HasValue || foo.Value)
                    {
                        error = true;
                        break;
                    }
                    data = getData(response);
                    result.AddRange(data);
                    offset += limit;
                }
                while (data.Length > 0);
            }

            if (error)
                throw new Exception("number of max retries exhausted while trying to get a response");

            return result.ToArray();
        }
    }
}
