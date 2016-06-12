using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLFSSharp.Commands
{
    class CleanCommand : ICommand
    {
        public async Task<int> Run(string[] args)
        {
            var resultDict = new List<KeyValuePair<string, string>>();
            resultDict.Add(new KeyValuePair<string, string>("version", "https://git-lfs.github.com/spec/v1"));

            byte[] buffer = new byte[65536];
            using (var stdin = Console.OpenStandardInput())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;

                    while ((read = stdin.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }

                    var data = ms.ToArray();
                    var hash = SolGitLFS.Utils.SHA256.CalcHash(data);

                    resultDict.Add(new KeyValuePair<string, string>("oid", "sha256:" + hash));
                    resultDict.Add(new KeyValuePair<string, string>("size", data.Length.ToString()));
                }
            }

            foreach(var element in resultDict)
            {
                Console.Write(element.Key);
                Console.Write(" ");
                Console.Write(element.Value);
                Console.Write("\n");
            }

            return 0;
        }
    }
}
