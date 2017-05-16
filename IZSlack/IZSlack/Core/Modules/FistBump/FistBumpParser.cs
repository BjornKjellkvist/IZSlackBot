using Newtonsoft.Json;
using IZSlack.Core.Messengers;
using IZSlack.Core.WebAPI;
using IZSlack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Modules.FistBump {
    public class Parser {

        Message _Message;

        public Parser(string payload) {
            _Message = JsonConvert.DeserializeObject<Message>(payload);
        }

        public string ParsePlayerId() {
            return string.IsNullOrEmpty(_Message.user) ? null : _Message.user;
        }

        public string ParsePlayerName() {
            User User = Users.FindUser(_Message.user);
            if (User != null) {
                return string.IsNullOrEmpty(User.name) ? null : User.name;
            }
            return null;
        }

        public User ParseTargetPlayer() {
            //message.text should look like this "!fistbump @<user>" || "!fistbumnp username"
            var Args = _Message.text.Trim().Split(' ');
            if (Args.Count() < 2) {
                return null;
            }
            string UserName;
            if (Args[1].Contains("@")) {
                UserName = Args[1].Replace("@", string.Empty)
                .Replace("<", string.Empty)
                .Replace(">", string.Empty);
                return Users.FindUser(UserName);
            }
            return Users.FindUser(Args[1]);
        }

        public string ParseChannel() {
            //this is not how I normal write, but I like it here
            if (!string.IsNullOrWhiteSpace(_Message.channel))
                return _Message.channel;
            else
                return null;
        }

        public GameType ParseGameType() {
            var Args = _Message.text.Trim().Split(' ').Where(x => !string.IsNullOrEmpty(x));
            //Open games only has one arguemnt, so if we have more then 1 arguemnt the game must be player specific
            //We do not know if that player exist or not, or if there are any trailing arguments
            if (Args.Count() > 1) {
                return GameType.PlayerSpecific;
            }
            return GameType.Open;
        }
    }
}
