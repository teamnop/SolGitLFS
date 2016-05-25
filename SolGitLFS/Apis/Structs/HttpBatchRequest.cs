using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS.Apis.Structs
{
    /// <summary>
    /// https://github.com/github/git-lfs/blob/master/docs/api/http-v1-batch.md
    /// https://github.com/github/git-lfs/blob/master/docs/api/http-v1-batch-request-schema.json
    /// </summary>
    class HttpBatchRequest
    {
        public enum OperationType
        {
            download   
        }

        public OperationType operation { get; set; }
        public List<HttpBatchRequestObject> objects { get; set; }
    }
}
