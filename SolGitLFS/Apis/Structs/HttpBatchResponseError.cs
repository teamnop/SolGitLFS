using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS.Apis.Structs
{
    public class HttpBatchResponseError
    {
        public int code { get; set; }
        public string message { get; set; }
    }
}
