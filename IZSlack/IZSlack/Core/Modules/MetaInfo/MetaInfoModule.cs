using IZSlack.Core.Events.EventHandlers;
using IZSlack.Core.Messengers;
using IZSlack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Modules.MetaInfo {
    class MetaInfoModule : IModule {

        public Dictionary<string, Action<string>> Commands { get; set; }
        RTMSender Sender = new RTMSender();
        private int Padding = 2;
        private string PaddingString;

        public MetaInfoModule() {
            var PaddingStringBuilder = new StringBuilder();
            for (int i = 0; i > Padding; i++) {
                PaddingStringBuilder.Append(" ");
            }
            PaddingString = PaddingStringBuilder.ToString();
            Commands = new Dictionary<string, Action<string>> {
                { "!commands", InitCommands }
            };
        }

        public void InitCommands(string payload) {
            Parser _Parser = new Parser(payload);
            string Channel = _Parser.GetChannel();
            Sender.SendMessage(new RTMMessageOut {
                channel = Channel,
                text = GetFormatCommandList()
            });
        }

        //Buckle up kid, you are going for a ride...
        public string GetFormatCommandList() {
            //Let's get all the commands
            var CommandList = new List<string>();
            foreach (var m in Events.EventHandlers.Message.RegisteredModules) {
                foreach (var c in m.Commands) {
                    CommandList.Add(c.Key);
                }
            }

            //we need two different lists, first list is the left column and the second one is the right
            var FirstArray = CommandList.Take((CommandList.Count + 1) / 2).ToList();
            var SecondArray = CommandList.Skip((CommandList.Count + 1) / 2).ToList();

            //cool now we have the length of the longest string + the padding
            int MaxLength = FirstArray.Max(x => x.Length) + Padding;
            var DefaultStringBuilder = new StringBuilder();
            //lets turn that lenght into a bunch of whitespaces
            for (int i = 0; i < MaxLength; i++) {
                DefaultStringBuilder.Append(" ");
            }
            string DefaultString = DefaultStringBuilder.ToString();

            var sb = new StringBuilder();
			//This makes the message Monospace
            sb.Append("```");
            sb.AppendLine("Currently Available Commands:");
            //if there is a difference of one (first list being longer) then we append
            //an empty line to second array, the arrays shouldhave the same length now
            if (SecondArray.Count < FirstArray.Count) {
                SecondArray.Add("");
            }
			//this is where the magic happens, we now create a stylized list.
			for (int i = 0; i < FirstArray.Count; i++) {
                sb.Append(FirstArray[i] + DefaultString.Remove(0, FirstArray[i].Count())).Append("|").Append(PaddingString).AppendLine(SecondArray[i]);
            }
            sb.Append("```");
            return sb.ToString();
        }
    }
}
