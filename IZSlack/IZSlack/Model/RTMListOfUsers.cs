using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Model {
    public class RTMListOfUsers {

        public bool ok { get; set; }
        public Member[] members { get; set; }
        public int cache_ts { get; set; }

        public class Member {
            public string id { get; set; }
            public string team_id { get; set; }
            public string name { get; set; }
            public bool deleted { get; set; }
            public object status { get; set; }
            public string color { get; set; }
            public string real_name { get; set; }
            public string tz { get; set; }
            public string tz_label { get; set; }
            public int tz_offset { get; set; }
            public Profile profile { get; set; }
            public bool is_admin { get; set; }
            public bool is_owner { get; set; }
            public bool is_primary_owner { get; set; }
            public bool is_restricted { get; set; }
            public bool is_ultra_restricted { get; set; }
            public bool is_bot { get; set; }
        }

        public class Profile {
            public string avatar_hash { get; set; }
            public string real_name { get; set; }
            public string real_name_normalized { get; set; }
            public string email { get; set; }
            public string image_24 { get; set; }
            public string image_32 { get; set; }
            public string image_48 { get; set; }
            public string image_72 { get; set; }
            public string image_192 { get; set; }
            public string image_512 { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string title { get; set; }
            public string skype { get; set; }
            public string phone { get; set; }
            public string image_1024 { get; set; }
            public string image_original { get; set; }
            public string bot_id { get; set; }
            public string api_app_id { get; set; }
            public bool always_active { get; set; }
            public object fields { get; set; }
        }

    }
}
