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
            resultDict.Add(new KeyValuePair<string, string>("version", SolGitLFS.Define.LFSVersion));

            byte[] buffer = new byte[65536];
            byte[] hashArray = null;

            var root = SolGitLFS.Utils.GitRepo.FindGitRoot();
            if (root == null)
            {
                throw new Exception("Git Repo를 찾지 못하였습니다");
            }

            var tmpPath = SolGitLFS.Define.LFSTmpObjectFolder;
            Directory.CreateDirectory(Path.Combine(root, tmpPath));

            var tmpName = DateTime.Now.Ticks;
            var tmpFile = Path.Combine(root, tmpPath, tmpName.ToString());

            using (var stdin = Console.OpenStandardInput())
            {
                int size = 0;
                using (var sha256 = SHA256Managed.Create())
                {
                    using (var fp = File.OpenWrite(tmpFile))
                    {
                        while (true)
                        {
                            int readBytes = stdin.Read(buffer, 0, buffer.Length);

                            size += readBytes;
                            if (readBytes > 0)
                            {
                                sha256.TransformBlock(buffer, 0, readBytes, null, 0);
                                fp.Write(buffer, 0, readBytes);
                            }
                            else
                            {
                                sha256.TransformFinalBlock(buffer, 0, readBytes);
                                hashArray = sha256.Hash;
                                break;
                            }
                        }

                        fp.Close();
                    }

                    var oid = SolGitLFS.Utils.SHA256.ByteArrayToHash(hashArray);

                    var relativeLfsObjectPath = SolGitLFS.Utils.GitRepo.CreateLFSObjectPath(oid);
                    Directory.CreateDirectory(Path.Combine(root, relativeLfsObjectPath));

                    var lfsObjectPath = Path.Combine(root, relativeLfsObjectPath, oid);
                    if (File.Exists(lfsObjectPath) == false)
                    {
                        File.Move(tmpFile, Path.Combine(root, relativeLfsObjectPath, oid));
                    }
                    else
                    {
                        File.Delete(tmpFile);
                    }
                    
                    resultDict.Add(new KeyValuePair<string, string>("oid", "sha256:" + oid));
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
