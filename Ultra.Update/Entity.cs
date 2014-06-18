using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultra.Update {

    [Serializable]
    public class ClientConfig {
        public string ServerUrl { get; set; }
        public string ClientFileName { get; set; }
        public string BackupDirectory { get; set; }
        public string ProcessName { get; set; }
        public string Version { get; set; }
    }

    [Serializable]
    public class ServerConfig {
        public string FileUrl { get; set; }
        public string Version { get; set; }
    }
}
