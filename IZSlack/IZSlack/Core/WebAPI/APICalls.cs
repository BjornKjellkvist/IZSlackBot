using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IZSlack.Core.Messengers;
using IZSlack.Core.Model;
using IZSlack.Core.WebSocket;
using IZSlack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.WebAPI {
    static class APICalls {
        static string AuthToken = SlackConnector.AuthToken;

        public static void JoinChannel(string channelName) {
            APICaller AC = new APICaller();
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("token", SlackConnector.AuthToken),
                new KeyValuePair<string, string>("name", channelName)
            });
            AC.CallAPI(content, "https://slack.com/api/channels.join");
            ConsoleMessenger.PrintSuccess("Joined Channel: " + channelName);
        }

        public static Channel GetChannelInfo(string channel) {
            APICaller AC = new APICaller();
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("token", SlackConnector.AuthToken),
                new KeyValuePair<string, string>("channel", channel)
            });
            var response = AC.CallAPI(content, "https://slack.com/api/channels.info");
            return response["channel"].ToObject<Channel>();
        }
    }
}
