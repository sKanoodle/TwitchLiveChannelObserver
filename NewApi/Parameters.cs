using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchApp.NewApi
{
    public abstract class ParametersBase
    {
        protected string ToStringHelper(params (object value, string name)[] parameters)
        {
            List<string> parts = new List<string>();
            foreach (var parameter in parameters)
                if (false == parameter.value is null)
                {
                    string part;
                    switch (parameter.value)
                    {
                        case IEnumerable<string> list: part = String.Join("&", list.Select(s => $"{parameter.name}={s}")); break;
                        case int i when i is 0: continue; //ignoring value types by hand...
                        case int i: part = $"{parameter.name}={i}"; break;
                        case string s when parameter.name == null: part = s; break;
                        case string s: part = $"{parameter.name}={s}"; break;
                        default: throw new ArgumentException($"unkown type of parameter ({parameter.value} : {parameter.value.GetType()})");
                    }
                    parts.Add(part);

                }
            return String.Join("&", parts);
        }
    }

    public abstract class PageParameters : ParametersBase
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
            return ToStringHelper(
                (After, "after"),
                (Before, "before"),
                (First, "first"));
        }
    }

    public class EntitlementsParameters : ParametersBase
    {
        /// <summary>
        /// manifest_id: Unique identifier of the manifest file to be uploaded. Must be 1-64 characters.
        /// </summary>
        public string ManifestID { get; set; }
        /// <summary>
        /// type: Type of entitlement being granted. Only bulk_drops_grant is supported.
        /// </summary>
        public string Type => "bulk_drops_grant";

        public override string ToString()
        {
            return "?" + ToStringHelper(
                (ManifestID, "manifest_id"),
                (Type, "type"));
        }
    }

    public class GamesParameters : ParametersBase
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
            return "?" + ToStringHelper(
                (IDs, "id"),
                (Names, "name"));
        }
    }

    public class StreamsParameters : PageParameters
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
            return "?" + ToStringHelper(
                (base.ToString(), null),
                (CommunityIDs, "community_id"),
                (GameIDs, "game_id"),
                (Languages, "language"),
                (Type, "type"),
                (UserIDs, "user_id"),
                (UserLogins, "user_login"));
        }
    }

    public class UsersParameters : ParametersBase
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
            return "?" + ToStringHelper(
                (IDs, "id"),
                (Logins, "login"));
        }
    }

    public class UsersFollowsParameters : PageParameters
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
            return "?" + ToStringHelper(
                (base.ToString(), null),
                (FromID, "from_id"),
                (ToID, "to_id"));
        }
    }
}
