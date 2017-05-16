using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Model {
    public class User {

        public string id { get; set; }
        public string name { get; set; }
        public bool deleted { get; set; }
        public string color { get; set; }
        public Profile profile { get; set; }
        public bool is_admin { get; set; }
        public bool is_owner { get; set; }
        public bool is_primary_owner { get; set; }
        public bool is_restricted { get; set; }
        public bool is_ultra_restricted { get; set; }
        public bool has_2fa { get; set; }
        public string two_factor_type { get; set; }
        public bool has_files { get; set; }

        public class Profile {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string real_name { get; set; }
            public string email { get; set; }
            public string skype { get; set; }
            public string phone { get; set; }
            public string image_24 { get; set; }
            public string image_32 { get; set; }
            public string image_48 { get; set; }
            public string image_72 { get; set; }
            public string image_192 { get; set; }
            public string image_512 { get; set; }
        }

        public static User convertFromMember(RTMListOfUsers.Member memeber) {
            User user = new User {
                id = memeber.id,
                name = memeber.name,
                deleted = memeber.deleted,
                color = memeber.color,
                is_ultra_restricted = memeber.is_ultra_restricted,
                is_primary_owner = memeber.is_primary_owner,
                profile = new Profile {
                    email = memeber.profile.email,
                    first_name = memeber.profile.first_name,
                    image_192 = memeber.profile.image_192,
                    image_24 = memeber.profile.image_24,
                    image_32 = memeber.profile.image_32,
                    image_48 = memeber.profile.image_48,
                    image_512 = memeber.profile.image_512,
                    image_72 = memeber.profile.image_72,
                    last_name = memeber.profile.last_name,
                    real_name = memeber.profile.real_name
                }
            };
            return user;
        }
    }
}
