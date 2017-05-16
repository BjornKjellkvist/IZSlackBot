using Microsoft.VisualStudio.TestTools.UnitTesting;
using IZSlack.Core.Module.FistBump;
using IZSlack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IZSlack.Core.WebSocket;
using IZSlack.Core.Modules.FistBump;
using slackBF;

namespace IZSlack.Core.Module.FistBump.Tests {
    [TestClass()]
    public class ParserTests {

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
        public void ParserTest() {
            Message m = DataMocker.newMessage("", "");
            try {
                Parser p = new Parser(JsonConvert.SerializeObject(m));
            } catch (Exception) {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void ParsePlayerIdTest() {
            //golden path should not throw any exceptions
            Message m1 = DataMocker.newMessage("test", "");
            Parser p1 = new Parser(JsonConvert.SerializeObject(m1));
            Equals(p1.ParsePlayerId() == "test");
            Assert.IsNotNull(p1.ParsePlayerId());

            //emtpy id should be null
            Message m2 = DataMocker.newMessage("", "");
            Parser p2 = new Parser(JsonConvert.SerializeObject(m2));
            Assert.IsNull(p2.ParsePlayerId());
        }

        [TestMethod()]
        public void ParsePlayerNameTest() {
            //golden path "name" should not throw any exceptions
            Message m1 = DataMocker.newMessage("bjornkjellkvist", "");
            Parser p1 = new Parser(JsonConvert.SerializeObject(m1));
            Assert.IsNotNull(p1.ParsePlayerName());

            //golden path "ID" should not throw any exceptions
            Message m2 = DataMocker.newMessage("U339J4Y78", "");
            Parser p2 = new Parser(JsonConvert.SerializeObject(m2));
            Assert.IsNotNull(p2.ParsePlayerName());

            //empty username||id should be null
            Message m3 = DataMocker.newMessage("", "");
            Parser p3 = new Parser(JsonConvert.SerializeObject(m3));
            Assert.IsNull(p3.ParsePlayerName());

            //non-existing username||id should return null
            Message m4 = DataMocker.newMessage("aefwijfaweuofgaewiuiygweagyfaewufaegw", "");
            Parser p4 = new Parser(JsonConvert.SerializeObject(m4));
            Assert.IsNull(p4.ParsePlayerName());
        }

        [TestMethod()]
        public void ParseTargetPlayerTest() {
            //User id golden path
            Message m1 = DataMocker.newMessage("U339J4Y78", "!fistbump <@U339J4Y78>");
            Parser p1 = new Parser(JsonConvert.SerializeObject(m1));
            var user1 = p1.ParseTargetPlayer();
            Assert.IsNotNull(user1);
            Assert.AreEqual("bjornkjellkvist", user1.name);

            //User by name golden path
            Message m2 = DataMocker.newMessage("U339J4Y78", "!fistbump bjornkjellkvist");
            Parser p2 = new Parser(JsonConvert.SerializeObject(m2));
            var user2 = p2.ParseTargetPlayer();
            Assert.IsNotNull(user2);
            Assert.AreEqual("bjornkjellkvist", user2.name);

            //No user found should give null back
            Message m3 = DataMocker.newMessage("U339J4Y78", "!fistbump öäöåöäåö");
            Parser p3 = new Parser(JsonConvert.SerializeObject(m3));
            var user3 = p3.ParseTargetPlayer();
            Assert.IsNull(user3);

            //we should never get here but if we do we should return null
            Message m4 = DataMocker.newMessage("U339J4Y78", "!fistbump");
            Parser p4 = new Parser(JsonConvert.SerializeObject(m4));
            var user4 = p4.ParseTargetPlayer();
            Assert.IsNull(user4);
        }

        [TestMethod()]
        public void ParseChannelTest() {
            //golden path
            Message m1 = DataMocker.newMessage("", "", "rekt");
            Parser p1 = new Parser(JsonConvert.SerializeObject(m1));
            var channel1 = p1.ParseChannel();
            Assert.AreEqual(channel1, "rekt");

            //emtpy channel should give null
            Message m2 = DataMocker.newMessage("", "", "");
            Parser p2 = new Parser(JsonConvert.SerializeObject(m2));
            var channel2 = p2.ParseChannel();
            Assert.IsNull(channel2);
        }

        [TestMethod()]
        public void ParseGameTypeTest() {
            //golden path open game
            Message m1 = DataMocker.newMessage("", "awefasdfwaef");
            Parser p1 = new Parser(JsonConvert.SerializeObject(m1));
            var type1 = p1.ParseGameType();
            Assert.AreEqual(type1, GameType.Open);

            //golden path specific game
            Message m2 = DataMocker.newMessage("", "awefasdfwaef awefawef");
            Parser p2 = new Parser(JsonConvert.SerializeObject(m2));
            var type2 = p2.ParseGameType();
            Assert.AreEqual(type2, GameType.PlayerSpecific);

            //multiple params should return specifc game
            Message m3 = DataMocker.newMessage("", "awefasdfwaef awefawef awdfwaefawef");
            Parser p3 = new Parser(JsonConvert.SerializeObject(m3));
            var type3 = p3.ParseGameType();
            Assert.AreEqual(type3, GameType.PlayerSpecific);

            Message m4 = DataMocker.newMessage("", "");
            Parser p4 = new Parser(JsonConvert.SerializeObject(m4));
            var type4 = p4.ParseGameType();
            Assert.AreEqual(type4, GameType.Open);
        }
    }
}