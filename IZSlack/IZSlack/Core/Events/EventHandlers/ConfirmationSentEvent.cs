using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Events.EventHandlers {
    class ConfirmationSent : IEventHandler {
        public bool HandleEvent(string payload) {
            return true;
        }
    }
}
