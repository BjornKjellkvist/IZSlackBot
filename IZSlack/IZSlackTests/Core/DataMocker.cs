using IZSlack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Module.FistBump.Tests {
    class DataMocker {
        private static int id;
        public static User newStandardUser() {
            id++;
            User user = new User {
                color = "9f69e7",
                deleted = false,
                has_2fa = false,
                has_files = false,
                id = id.ToString(),
                is_admin = false,
                is_owner = false,
                is_primary_owner = false,
                is_restricted = false,
                is_ultra_restricted = false,
                name = "user" + id,
                profile = null,
                two_factor_type = "no"
            };
            return user;
        }

        public static Message newMessage(string user, string text, string channel = "waeawef") {
            Message message = new Message {
                channel = channel,
                user = user,
                text = text,
                ts = DateTime.Now.Ticks.ToString(),
                type = "message"
            };
            return message;
        }

    }
}
