using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLFSSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<SolGitLFS.Entities.LFSObject> lfsObjects = new List<SolGitLFS.Entities.LFSObject>();
            lfsObjects.Add(new SolGitLFS.Entities.LFSObject()
            {
                oid = "c9032aeee0352e71587eb73095c92454c7a8710372481e07abf49f17579a3482",
                size = 16384
            });

            SolGitLFS.Apis.HTTPApi.DownloadQuery("https://github.com/teamnop/LFSTestRepo.git", lfsObjects).Wait();
        }
    }
}
