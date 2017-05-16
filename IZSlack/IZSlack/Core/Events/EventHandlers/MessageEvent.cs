using Newtonsoft.Json.Linq;
using IZSlack.Core.Modules;
using IZSlack.Core.Modules.FistBump;
using IZSlack.Core.Modules.LightChanger;
using IZSlack.Core.Modules.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Events.EventHandlers {
    class Message : IEventHandler {
        public bool HandleEvent(string payload) {
            ParseMessage(payload);
            return true;
        }

        private void ParseMessage(string payload) {
            string text = JObject.Parse(payload)["text"].ToString();
            string InitCheck = text.Split(' ')[0];
            foreach (IModule module in RegisteredModules) {
                foreach (var cmd in module.Commands) {
                    if (cmd.Key.ToUpper() == InitCheck.ToUpper()) {
                        //we pass through the invoking object to the handling cmd
                        cmd.Value.Invoke(payload);
                        return;
                    }
                }
            }
        }

        //This is were you register you module
        public static List<IModule> RegisteredModules = new List<IModule> {
        new MetaInfoModule(),
        new FistBumpModule(),
        new LightChangerModule(),
        };
    }
}
