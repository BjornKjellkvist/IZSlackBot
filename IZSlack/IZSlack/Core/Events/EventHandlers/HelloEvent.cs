using IZSlack.Core.Messengers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Events.EventHandlers {
    public class Hello : IEventHandler {
        public bool HandleEvent(string payload) {
            return true;
        }
    }
}
