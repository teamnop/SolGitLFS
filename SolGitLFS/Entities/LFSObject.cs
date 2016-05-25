using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS.Entities
{
    public class LFSObject
    {
        public string oid { get; set; }
        public long size { get; set; }
    }
}
