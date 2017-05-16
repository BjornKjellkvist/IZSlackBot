using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slack.Webhooks;
using IZSlack.Core.WebSocket;
using IZSlack.Core.Messengers;
using IZSlack.Core.WebAPI;
using IZSlack.Model;
using slackBF;

namespace IZSlack {
	class Program {
		static void Main(string[] args) {
			Console.Title = "IZSlackBot";
			EnviromentLoader.Load();
			AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnClose);
			SlackConnector.Connect();
			Console.ReadLine();
		}

		static void OnClose(object sender, EventArgs e) {
			ConsoleMessenger.PrintWarning("Shutting Down");
			SlackConnector.GetSocket().CloseAsync();
		}

		static void StartUp() {
			RTMSender ms = new RTMSender();
			ms.SendMessage(new RTMMessageOut {
				channel = "C31UPGH33",
				text = "And now I'm talking for myself"
			});
		}
	}
}
