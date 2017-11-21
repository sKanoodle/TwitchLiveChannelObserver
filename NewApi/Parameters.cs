using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchApp.NewApi
{
    public abstract class BaseParameters
    {
        /// <summary>
        /// Cursor for forward pagination: tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        public string After { get; set; }
        /// <summary>
        /// Cursor for backward pagination: tells the server where to start fetching the next set of results, in a multi-page response.
        /// </summary>
        public string Before { get; set; }
        /// <summary>
        /// Maximum number of objects to return. Maximum: 100. Default: 20.
        /// </summary>
        public int First { get; set; }

        public override string ToString()
        {
            List<string> parts = new List<string>();
            if (false == After is null)
                parts.Add($"after={After}");
            if (false == Before is null)
                parts.Add($"before={Before}");
            if (First != 0)
                parts.Add($"first={First}");
            return String.Join("&", parts);
        }
    }

    public class EntitlementsParameters
    {
        /// <summary>
        /// manifest_id: Unique identifier of the manifest file to be uploaded. Must be 1-64 characters.
        /// </summary>
        public string ManifestID { get; set; }
        /// <summary>
        /// type: Type of entitlement being granted. Only bulk_drops_grant is supported.
        /// </summary>
        public string Type => "bulk_drops_grant";
    }

    public class GamesParameters
    {
        /// <summary>
        /// User ID. Multiple user IDs can be specified. Limit: 100.
        /// </summary>
        public IEnumerable<string> IDs { get; set; }
        /// <summary>
        /// User login name. Multiple login names can be specified. Limit: 100.
        /// </summary>
        public IEnumerable<string> Names { get; set; }

        public override string ToString()
        {
            List<string> parts = new List<string>();
            if (false == IDs is null)
                parts.Add(String.Join("&", IDs.Select(s => $"id={s}")));
            if (false == Names is null)
                parts.Add($"name={String.Join(",", Names)}");
            return $"?{String.Join("&", parts)}";
        }
    }

    public class StreamsParameters : BaseParameters
    {
        /// <summary>
        /// Returns streams in a specified community ID. You can specify up to 100 IDs.
        /// </summary>
        public IEnumerable<string> CommunityIDs { get; set; }
        /// <summary>
        /// Returns streams broadcasting a specified game ID. You can specify up to 100 IDs.
        /// </summary>
        public IEnumerable<string> GameIDs { get; set; }
        /// <summary>
        /// Stream language. You can specify up to 100 languages.
        /// </summary>
        public IEnumerable<string> Languages { get; set; }
        /// <summary>
        /// Stream type: "all", "live", "vodcast". Default: "all".
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Returns streams broadcast by one or more specified user IDs. You can specify up to 100 IDs.
        /// </summary>
        public IEnumerable<string> UserIDs { get; set; }
        /// <summary>
        /// Returns streams broadcast by one or more specified user login names. You can specify up to 100 names.
        /// </summary>
        public IEnumerable<string> UserLogins { get; set; }

        public override string ToString()
        {
            List<string> parts = new List<string>();
            string baseParts = base.ToString();
            if (!String.IsNullOrWhiteSpace(baseParts))
                parts.Add(baseParts);
            if (false == CommunityIDs is null)
                parts.Add($"community_id={String.Join(",", CommunityIDs)}");
            if (false == GameIDs is null)
                parts.Add($"game_id={String.Join(",", GameIDs)}");
            if (false == Languages is null)
                parts.Add($"language={String.Join(",", Languages)}");
            if (false == Type is null)
                parts.Add($"type={Type}");
            if (false == UserIDs is null)
                parts.Add(String.Join("&", UserIDs.Select(s => $"user_id={s}")));
            if (false == UserLogins is null)
                parts.Add($"user_login={String.Join(",", UserLogins)}");
            return $"?{String.Join("&", parts)}";
        }
    }

    public class UsersParameters
    {
        /// <summary>
        /// User ID. Multiple user IDs can be specified. Limit: 100.
        /// </summary>
        public IEnumerable<string> IDs { get; set; }
        /// <summary>
        /// User login name. Multiple login names can be specified. Limit: 100.
        /// </summary>
        public IEnumerable<string> Logins { get; set; }

        public override string ToString()
        {
            List<string> parts = new List<string>();
            if (false == IDs is null)
                parts.Add(String.Join("&", IDs.Select(s => $"id={s}")));
            if (false == Logins is null)
                parts.Add(String.Join("&", Logins.Select(s => $"login={s}")));
            return $"?{String.Join("&", parts)}";
        }
    }

    public class UsersFollowsParameters : BaseParameters
    {
        /// <summary>
        /// User ID. The request returns information about users who are being followed by the from_id user.
        /// </summary>
        public string FromID { get; set; }
        /// <summary>
        /// User ID. The request returns information about users who are following the to_id user.
        /// </summary>
        public string ToID { get; set; }

        public override string ToString()
        {
            List<string> parts = new List<string>();
            string baseParts = base.ToString();
            if (!String.IsNullOrWhiteSpace(baseParts))
                parts.Add(baseParts);
            if (!String.IsNullOrWhiteSpace(FromID))
                parts.Add($"from_id={FromID}");
            if (!String.IsNullOrWhiteSpace(ToID))
                parts.Add($"to_id={ToID}");
            return $"?{String.Join("&", parts)}";
        }
    }
}
