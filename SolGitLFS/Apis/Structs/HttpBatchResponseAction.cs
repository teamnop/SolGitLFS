using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS.Apis.Structs
{
    public class HttpBatchResponseAction
    {
        public string href { get; set; }
        public Dictionary<string, string> header { get; set; }
        public string expires_at { get; set; }
    }
}
