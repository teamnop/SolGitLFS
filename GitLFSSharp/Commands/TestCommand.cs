using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLFSSharp.Commands
{
    class TestCommand : ICommand
    {
        public int Run(string[] args)
        {
            List<SolGitLFS.Entities.LFSObject> lfsObjects = new List<SolGitLFS.Entities.LFSObject>();
            lfsObjects.Add(new SolGitLFS.Entities.LFSObject()
            {
                oid = "c9032aeee0352e71587eb73095c92454c7a8710372481e07abf49f17579a3482",
                size = 16384
            });

            var task = SolGitLFS.Apis.HTTPApi.DownloadQueryAsync("https://github.com/teamnop/LFSTestRepo.git",
                lfsObjects);
            task.Wait();

            var downloadTask = SolGitLFS.Apis.HTTPApi.DownloadAsync(task.Result.objects[0].actions.download.href);
            downloadTask.Wait();

            Console.WriteLine(downloadTask.Result.Length);

            return 0;
        }
    }
}
