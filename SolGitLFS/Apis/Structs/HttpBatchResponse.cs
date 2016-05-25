using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS.Apis.Structs
{
    /// <summary>
    /// https://github.com/github/git-lfs/blob/master/docs/api/http-v1-batch.md
    /// https://github.com/github/git-lfs/blob/master/docs/api/http-v1-batch-response-schema.json
    /// </summary>
    public class HttpBatchResponse
    {
        public List<HttpBatchResponseObject> objects { get; set; }
    }
}
