using Microsoft.VisualStudio.TestTools.UnitTesting;
using IZSlack.Core.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slackBF;

namespace IZSlack.Core.WebSocket.Tests {
	[TestClass()]
	public class SlackConnectorTests {

		[ClassInitialize()]
		public static void ClassInit(TestContext testContext) {
			EnviromentLoader.Load();
		}

		[ClassCleanup()]
		public static void ClassClean() {
			SlackConnector.GetSocket().Close();
		}

		[TestMethod()]
		public void GetSocketTest() {
			Assert.IsNotNull(SlackConnector.GetSocket());
		}
	}
}