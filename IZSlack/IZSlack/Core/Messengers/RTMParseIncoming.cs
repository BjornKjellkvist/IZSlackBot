using Newtonsoft.Json.Linq;
using IZSlack.Core.Events;
using IZSlack.Core.Events.EventHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace IZSlack.Core.Messengers {
	public class RTMParseIncoming {
		public void OnIncomingMessage(object sender, MessageEventArgs e) {
			string payload = e.Data;
			EventType _EventType = ParseEvent(payload);
			IEventHandler Handler;
			HandlerMap.TryGetValue(_EventType, out Handler);
			Handler.HandleEvent(payload);
		}

		private EventType ParseEvent(string data) {
			var jsonObj = JObject.Parse(data);
			try {
				string EventString = jsonObj["type"].ToString();
				EventType _EventType;
				if (Enum.TryParse(EventString, out _EventType)) {
					return _EventType;
				}
				return EventType.ParseError;
			}
			catch (NullReferenceException) {
				try {
					if (jsonObj.ToString().Contains("reply_to")) {
						return EventType.SentConfirmation;
					}
				}
				catch (Exception) {
					return EventType.ParseError;
				}
			}
			return EventType.ParseError;
		}

		private Dictionary<EventType, IEventHandler> HandlerMap = new Dictionary<EventType, IEventHandler> {
			{EventType.hello, new Hello()},
			{EventType.message, new Message()},
			{EventType.ParseError, new ParseError()},
			{EventType.SentConfirmation, new ConfirmationSent()},
			{EventType.precencechanged, new PrecenceChanged()}
		};

	}
}
