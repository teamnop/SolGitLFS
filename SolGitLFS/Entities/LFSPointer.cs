using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS.Entities
{
    /// <summary>
    /// LFS의 객체 위치를 담고 있는 Pointer
    /// 
    /// oid, size로 이루어진 정보를 담고 있다.
    /// </summary>
    public class LFSPointer
    {
        public string oid { get; set; }
        public long size { get; set; }
    }
}
