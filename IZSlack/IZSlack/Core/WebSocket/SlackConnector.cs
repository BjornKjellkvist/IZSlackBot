using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Configuration;
using IZSlack.Core.Messengers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IZSlack.Core.WebAPI;

namespace IZSlack.Core.WebSocket {
	public static class SlackConnector {

		public static WebSocketSharp.WebSocket ConnectedSocket;
		public static string AuthToken = ConfigurationManager.AppSettings.Get("RTM-Token");
		public readonly static int ConnectionAttempts = 3;
		private static int CurrentAttempt = 0;

		public static void Connect() {
			Console.Title = "Bot is starting";
			var ws = new WebSocketSharp.WebSocket(GetWSConnectionString());
			ws.OnMessage += (sender, e) => new RTMParseIncoming().OnIncomingMessage(sender, e);
			ws.OnOpen += (sender, e) => OnOpen(sender, e);
			ws.Connect();
			ConnectedSocket = ws;
			Console.Title = "Bot is running";
		}

		private static void OnOpen(object sender, EventArgs e) {
			ConsoleMessenger.PrintSuccess("Connected to slack websocket");

		}


		//THIS IS A RECURSIVE FUNCTION
		public static WebSocketSharp.WebSocket GetSocket() {
			if (CurrentAttempt <= ConnectionAttempts) {
				if (ConnectedSocket != null) {
					CurrentAttempt = 0;
					return ConnectedSocket;
				}
				else {
					CurrentAttempt++;
					ConsoleMessenger.PrintError("Trying to reconnect");
					Connect();
					return GetSocket();
				}
			}
			ConsoleMessenger.PrintError("Connection To Slack Failed\nPress ENTER to exit or type \"retry\" to try again");
			var input = Console.ReadLine();
			if (input == "retry") {
				CurrentAttempt = 0;
				GetSocket();
			}
			Environment.Exit(0);
			return null;
		}

		public static void JoinChannel(string channelId) {
			APICaller AC = new APICaller();
			var content = new FormUrlEncodedContent(new[] {
				new KeyValuePair<string, string>("token", AuthToken),
				new KeyValuePair<string, string>("name", channelId)
			});
			AC.CallAPI(content, "https://slack.com/api/channels.join");
		}

		private static string GetWSConnectionString() {
			string ConnectionString = null;
			var content = new FormUrlEncodedContent(new[] {
				new KeyValuePair<string, string>("token", AuthToken),
				new KeyValuePair<string, string>("simple_latest", ""),
				new KeyValuePair<string, string>("no_unreads", "")
			});
			APICaller AC = new APICaller();
			ConnectionString = AC.CallAPI(content, "https://slack.com/api/rtm.start")["url"].ToString();
			return ConnectionString;
		}
	}
}

