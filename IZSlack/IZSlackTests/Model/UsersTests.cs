using Microsoft.VisualStudio.TestTools.UnitTesting;
using IZSlack.Core.Module.FistBump.Tests;
using IZSlack.Core.WebSocket;
using IZSlack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slackBF;

namespace IZSlack.Model.Tests {
    [TestClass()]
    public class UsersTests {

        [ClassInitialize()]
        public static void ClassInit(TestContext testContext) {
			EnviromentLoader.Load();
			SlackConnector.Connect();
        }

        [ClassCleanup()]
        public static void ClassClean() {
            SlackConnector.GetSocket().Close();
        }

        [TestMethod()]
        public void FetchAllUsersTest() {
            Users.FetchAllUsers();
            Assert.IsTrue(Users.GetUsers().Count() > 0);
        }

        [TestMethod()]
        public void PutUserTest() {
            User user = DataMocker.newStandardUser();
            Users.PutUser(user);
            Assert.IsTrue(Users.GetUsers().Contains(user));
        }

        [TestMethod()]
        public void FindUserTest() {
            User user = DataMocker.newStandardUser();
            Users.PutUser(user);
            Assert.IsNotNull(Users.FindUser(user.name));
        }
    }
}