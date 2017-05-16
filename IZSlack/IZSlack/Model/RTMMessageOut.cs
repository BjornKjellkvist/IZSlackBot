using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Model {
    public class RTMMessageOut {
        static int _id;
        public RTMMessageOut() {
            _id++;
            id = _id;
            type = "message";
        }
        public int id { get; }
        public string type { get; }
        public string channel { get; set; }
        public string text { get; set; }
    }
}
