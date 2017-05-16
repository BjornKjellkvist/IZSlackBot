using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IZSlack.Core.Modules {
    public interface IModule {
        Dictionary<string, Action<string>> Commands { get; set; }
    }
}
