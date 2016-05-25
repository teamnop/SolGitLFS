using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS.Apis.Structs
{
    public class HttpBatchResponseObject : Entities.LFSObject
    {
        public HttpBatchResponseError error { get; set; }
        public HttpBatchResponseActions actions { get; set; }
    }
}
