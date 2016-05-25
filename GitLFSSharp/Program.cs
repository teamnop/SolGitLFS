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
            if (args.Length == 0)
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
            }
            else
            {
                //System.Threading.Thread.Sleep(15000);
                Console.WriteLine(args[0]);

                if (args[0].Equals("smudge"))
                {
                    const int BOMLength = 3;
                    byte[] versionByteArray = Encoding.UTF8.GetBytes("version");
                    byte[] versionWithBOM = new byte[versionByteArray.Length + BOMLength]; // add BOM length

                    versionWithBOM[0] = 0xEF;
                    versionWithBOM[1] = 0xBB;
                    versionWithBOM[2] = 0xBF;

                    Array.Copy(versionByteArray, 0, versionWithBOM, 3, versionByteArray.Length);

                    byte[] inputBuf = new byte[versionWithBOM.Length];
                    int cnt = 0;
                    using (var stdin = Console.OpenStandardInput())
                    {
                        while (cnt < versionWithBOM.Length)
                        {
                            cnt += stdin.Read(inputBuf, cnt, versionWithBOM.Length - cnt);
                        }

                        if (inputBuf.SequenceEqual(versionWithBOM) == true ||
                            inputBuf.Skip(BOMLength).Take(versionByteArray.Length).SequenceEqual(versionByteArray) == true)
                        {
                            Console.WriteLine("true");
                        }
                        Console.WriteLine("false");
                    }
                }
            }
        }
    }
}
