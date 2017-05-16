using Newtonsoft.Json;
using IZSlack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using IZSlack.Core.Messengers;

namespace IZSlack.Core.Modules.LightChanger {
    class Parser {

        Message _Message;
        private RTMSender Sender = new RTMSender();

        public Parser(string payload) {
            _Message = JsonConvert.DeserializeObject<Message>(payload);
        }

        public int[] ParseColorToRGB() {
            List<string> Args = _Message.text.Trim().Split(' ').ToList();
            Args.RemoveAt(0);
            //Args should now only include arguments and not the calling command
            if (Args.Count <= 1) {
                try {
                    Color _Color = ColorTranslator.FromHtml(Args[0]);
                    int[] values = { _Color.R, _Color.G, _Color.B };
                    return values;
                } catch (Exception) {
                    Sender.SendMessage(new RTMMessageOut {
                        channel = _Message.channel,
                        text = "Invalid Color"
                    });
                }
            } else if (Args.Count >= 3) {
                int[] values = { Convert.ToInt32(Args[0]), Convert.ToInt32(Args[1]), Convert.ToInt32(Args[2]) };
                values = ValidateRGBValues(values);
                Color _Color = Color.FromArgb(values[0], values[1], values[2]);
                int[] RGBvalues = { _Color.R, _Color.G, _Color.B };
                return RGBvalues;
            }
            Sender.SendMessage(new RTMMessageOut {
                channel = _Message.channel,
                text = "Invalid Color"
            });
            return null;
        }

        //Making sure that the RGB values are correct
        private int[] ValidateRGBValues(int[] value) {
            for (int i = 0; i < value.Length; i++) {
                if (value[i] > 255) {
                    value[i] = 255;
                }
                if (value[i] < 0) {
                    value[i] = 0;
                }
            }
            return value;
        }
    }
}
