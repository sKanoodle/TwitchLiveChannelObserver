using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TwitchApp.NewApi
{
    class Client
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private static string ClientID => ApiKey.ClientID;
        private static readonly string BaseUrl = "https://api.twitch.tv/helix";

        public Client()
        {
            HttpClient.DefaultRequestHeaders.Add("Client-ID", ClientID);
        }

        /// <summary>
        /// Parallel to <see cref="GetUsersAsync(UsersParameters)"/> to only get a single user.
        /// </summary>
        public async Task<User> GetUserAsync(string nameOrId)
        {
            UsersParameters parameters = new UsersParameters();
            if (int.TryParse(nameOrId, out _))
                parameters.IDs = new[] { nameOrId };
            else
                parameters.Logins = new[] { nameOrId };
            var users = await GetUsersAsync(parameters);
            return users.Data.Single();
        }

        /// <summary>
        /// Creates a URL where you can upload a manifest file and notify users that they have an entitlement.
        /// Entitlements are digital items that users are entitled to use. Twitch entitlements are granted to users gratis or as part of a purchase on Twitch.
        /// </summary>
        public async Task CreateEntitlementGrantsUploadUrlAsync(EntitlementsParameters parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets game information by game ID or name.
        /// </summary>
        public async Task<ResponseWrapper<Game[]>> GetGamesAsync(GamesParameters parameters)
        {
            string url = CombineUrls(BaseUrl, $"/games{parameters}");
            return await GetTypedResponseAsync<Game[]>(url);
        }

        /// <summary>
        /// Gets information about active streams. Streams are returned sorted by number of current viewers, in descending order.
        /// Across multiple pages of results, there may be duplicate or missing streams, as viewers join and leave streams.
        /// </summary>
        public async Task<ResponseWrapper<Stream[]>> GetStreamsAsync(StreamsParameters parameters)
        {
            string url = CombineUrls(BaseUrl, $"/streams{parameters}");
            return await GetTypedResponseAsync<Stream[]>(url);
        }

        /// <summary>
        /// Gets metadata information about active streams playing Overwatch or Hearthstone. Streams are sorted by number of current viewers, in descending order.
        /// Across multiple pages of results, there may be duplicate or missing streams, as viewers join and leave streams.
        /// </summary>
        public async Task GetStreamsMetadataAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets information about one or more specified Twitch users.
        /// Users are identified by optional user IDs and/or login name. If neither a user ID nor a login name is specified, the user is looked up by Bearer token.
        /// </summary>
        public async Task<ResponseWrapper<User[]>> GetUsersAsync(UsersParameters parameters)
        {
            string url = CombineUrls(BaseUrl, $"/users{parameters}");
            return await GetTypedResponseAsync<User[]>(url);
        }

        /// <summary>
        /// Gets information on follow relationships between two Twitch users. Information returned is sorted in order, most recent follow first.
        /// This can return information like "who is lirik following," "who is following lirik,” or “is user X following user Y.”
        /// </summary>
        public async Task<ResponseWrapper<Follow[]>> GetUsersFollowsAsync(UsersFollowsParameters parameters)
        {
            string url = CombineUrls(BaseUrl, $"/users/follows{parameters}");
            return await GetTypedResponseAsync<Follow[]>(url);
        }

        /// <summary>
        /// Gets video information by video ID (one or more), user ID (one only), or game ID (one only).
        /// </summary>
        public async Task GetVideosAsync()
        {
            throw new NotImplementedException();
        }

        private async Task<ResponseWrapper<T>> GetTypedResponseAsync<T>(string url)
        {
            string response = await GetResponseAsync(url);
            return await Task.Run(() => JsonConvert.DeserializeObject<ResponseWrapper<T>>(response));
        }

        private async Task<string> GetResponseAsync(string url)
        {
            using (var response = await HttpClient.GetAsync(url))
            {
                //TODO: rate limit
                return await response.Content.ReadAsStringAsync();
            }
        }


        private string CombineUrls(string url) => url;

        private string CombineUrls(string url1, string url2)
        {
            if (String.IsNullOrWhiteSpace(url1))
                return url2;
            if (String.IsNullOrWhiteSpace(url2))
                return url1;

            url1 = url1.TrimEnd('/', '\\');
            url2 = url2.TrimStart('/', '\\');

            return $"{url1}/{url2}";
        }

        private string CombineUrls(params string[] parts)
        {
            if (parts.Length < 2) return null;

            string result = parts[0];
            for (int i = 1; i < parts.Length - 1; i++)
                result = CombineUrls(result, parts[i]);
            return result;
        }
    }
}
