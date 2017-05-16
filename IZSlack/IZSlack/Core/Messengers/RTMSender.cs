using Newtonsoft.Json;
using IZSlack.Core.WebSocket;
using IZSlack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Messengers {
    public class RTMSender {
        WebSocketSharp.WebSocket ws = SlackConnector.GetSocket();
        public bool SendMessage(RTMMessageOut message) {
            try {
                ws.Send(JsonConvert.SerializeObject(message));
                ConsoleMessenger.PrintSuccess($@"Sent message: ""{message.text}"" to channel ""{message.channel}""");
                return true;
            } catch (Exception) {
                ConsoleMessenger.PrintError("Could not send message");
                return false;
            }
        }
    }
}
