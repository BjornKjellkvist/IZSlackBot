using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.WebAPI {
    class APICaller {
        public JObject CallAPI(FormUrlEncodedContent args, string target) {
            using (var client = new HttpClient()) {
                var Content = args;
                var Response = client.PostAsync(target, Content).Result;
                var JsonObj = JObject.Parse(Response.Content.ReadAsStringAsync().Result);
                return JsonObj;
            }
        }
    }
}
