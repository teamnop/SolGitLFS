using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
            byte[] hashArray = null;

            using (var stdin = Console.OpenStandardInput())
            {
                int size = 0;
                using (var sha256 = SHA256Managed.Create())
                {
                    while (true)
                    {
                        int readBytes = stdin.Read(buffer, 0, buffer.Length);

                        size += readBytes;
                        if (readBytes > 0)
                        {
                            sha256.TransformBlock(buffer, 0, readBytes, null, 0);
                        }
                        else
                        {
                            sha256.TransformFinalBlock(buffer, 0, readBytes);
                            hashArray = sha256.Hash;
                            break;
                        }
                    }

                    var hash = SolGitLFS.Utils.SHA256.ByteArrayToHash(hashArray);
                    resultDict.Add(new KeyValuePair<string, string>("oid", "sha256:" + hash));
                    resultDict.Add(new KeyValuePair<string, string>("size", size.ToString()));
                }
            }

            foreach (var element in resultDict)
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
