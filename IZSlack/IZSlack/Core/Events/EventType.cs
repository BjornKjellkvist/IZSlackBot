using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Events {
    public enum EventType {
        channel_joined,
        channel_left,
        goodbye,
        hello,
        message,
        ParseError,
        SentConfirmation,
        precencechanged
    }
}
