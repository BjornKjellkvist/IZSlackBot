using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Events {
    interface IEventHandler {
        bool HandleEvent(string payload);
    }
}
