using Newtonsoft.Json;
using IZSlack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Modules.MetaInfo {
    public class Parser {
        Message _Message;

        public Parser(string payload) {
            _Message = JsonConvert.DeserializeObject<Message>(payload);
        }

        public string GetChannel() {
            return _Message.channel;
        }
    }
}
