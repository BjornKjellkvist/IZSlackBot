using Newtonsoft.Json.Linq;
using IZSlack.Core.Messengers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Events.EventHandlers {
    class ParseError : IEventHandler {
        public bool HandleEvent(string payload) {
            var jsonObj = JObject.Parse(payload);
            ConsoleMessenger.PrintError("Could not parse Unknown Type: " + jsonObj["type"].ToString());
            return true;
        }
    }
}
