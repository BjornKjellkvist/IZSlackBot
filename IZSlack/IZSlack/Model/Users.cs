using IZSlack.Core.Messengers;
using IZSlack.Core.WebAPI;
using IZSlack.Core.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Model {
    public class Users {
        private static List<User> _Users = new List<User>();

        public static List<User> GetUsers() {
            if (_Users.Count() < 1) {
                FetchAllUsers();
                return _Users;
            } else
                return _Users;
        }

        private static object locked = new object();
        /// <summary>
        /// clears the list and fetches a new list of users from slack
        /// </summary>
        public static void FetchAllUsers() {
            //made this thread safe for no reason
            lock (locked) {
                _Users.RemoveRange(0, _Users.Count());
                APICaller AC = new APICaller();
                var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("token", SlackConnector.AuthToken)
            });
                var response = AC.CallAPI(content, "https://slack.com/api/users.list");
                var Users = response.ToObject<RTMListOfUsers>();
                foreach (var u in Users.members) {
                    _Users.Add(User.convertFromMember(u));
                }
            }
        }

        /// <summary>
        /// updates a user or adds a new one if it does not exists
        /// </summary>
        /// <param name="ChangedUser"></param>
        public static void PutUser(User ChangedUser) {
            //THROWS COLLECTION WAS MODIFIED
            lock (locked) {
                if (GetUsers().Count() != 0) {
                    foreach (User cachedUser in GetUsers().ToList()) {
                        if (cachedUser.id == ChangedUser.id) {
                            GetUsers().Remove(cachedUser);
                            GetUsers().Add(ChangedUser);
                        } else {
                            GetUsers().Add(ChangedUser);
                        }
                    }
                } else {
                    GetUsers().Add(ChangedUser);
                }
            }
        }

        /// <summary>
        /// Returns a user if found, else it returns null
        /// </summary>
        /// <param name="NameOrId"></param>
        /// <returns>a User</returns>
        public static User FindUser(string NameOrId) {
            foreach (var u in GetUsers()) {
                if (u.name.Equals(NameOrId) || u.id.Equals(NameOrId)) {
                    return u;
                }
            }
            ConsoleMessenger.PrintError($"{NameOrId} not found");
            return null;
        }
    }
}