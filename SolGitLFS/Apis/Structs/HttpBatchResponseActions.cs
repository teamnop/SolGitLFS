using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS.Apis.Structs
{
    public class HttpBatchResponseActions
    {
        public HttpBatchResponseAction download { get; set; }
        public HttpBatchResponseAction upload { get; set; }
        public HttpBatchResponseAction verify { get; set; }
    }
}
