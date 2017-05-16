using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Modules.FistBump {
    public class GameRound {

        public GameRound() {
            PlayerSpecific = false;
        }

        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }
        public string Channel { get; set; }
        public bool PlayerSpecific { get; set; }
        public int TimePassed { get; set; }
    }
}
