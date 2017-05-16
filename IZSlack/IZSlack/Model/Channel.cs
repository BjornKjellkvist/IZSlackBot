using IZSlack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Model {
    class Channel {

        public string id { get; set; }
        public string name { get; set; }
        public string is_channel { get; set; }
        public int created { get; set; }
        public string creator { get; set; }
        public bool is_archived { get; set; }
        public bool is_general { get; set; }
        public string[] members { get; set; }
        public Topic topic { get; set; }
        public Purpose purpose { get; set; }
        public bool is_member { get; set; }
        public string last_read { get; set; }
        public Message latest { get; set; }
        public int unread_count { get; set; }
        public int unread_count_display { get; set; }

        public class Topic {
            public string value { get; set; }
            public string creator { get; set; }
            public int last_set { get; set; }
        }

        public class Purpose {
            public string value { get; set; }
            public string creator { get; set; }
            public int last_set { get; set; }
        }

    }
}
