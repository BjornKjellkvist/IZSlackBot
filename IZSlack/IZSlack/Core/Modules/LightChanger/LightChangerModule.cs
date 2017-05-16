using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IZSlack.Core.WebAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Modules.LightChanger {
    class LightChangerModule : IModule {
        APICaller AC = new APICaller();
        public Dictionary<string, Action<string>> Commands { get; set; }
        string Path = ConfigurationManager.AppSettings.Get("Raspberry-Socket");

        public LightChangerModule() {
            Commands = new Dictionary<string, Action<string>> {
                { "!lightcolor", InitChangeColor },
                { "!lightsoff", InitTurnOff },
                { "!lightrandom", InitRandom},
                { "!lightrandomblink", InitRandomBlink}
            };
        }

        public void InitChangeColor(string payload) {
            Parser _Parser = new Parser(payload);
            int[] color = _Parser.ParseColorToRGB();
            SendRGBColor(color);
        }

        public void InitTurnOff(string payload) {
            SendRGBColor(new[]{ 0, 0, 0 });
        }

        private void InitRandom(string payload) {
            SendRandomColor(false);
        }

        private void InitRandomBlink(string payload) {
            SendRandomColor(true);
        }

        private void SendRGBColor(int[] color) {
            string Key = "{\"rgb\"" + ":";
            string Value = $"[{color[0]},{color[1]},{color[2]}]" + "}";
            string Content = Key + Value;
            string _Path = Path + "/changecolor";
            RestCall(Content, _Path);
        }

        private void SendRandomColor(bool continuous) {
            string _Path = Path + "/random";
            if (continuous) {
                _Path += "blink";
            } else {
                _Path += "color";
            }
            RestCall("", _Path);
        }

        public string RestCall(string json, string target) {
            using (var client = new HttpClient()) {
                var Content = new StringContent(json, Encoding.UTF8, "application/json");
                var Response = client.PostAsync(target, Content).Result;
                var JsonObj = Response.Content.ReadAsStringAsync().Result;
                return JsonObj;
            }
        }
    }
}
