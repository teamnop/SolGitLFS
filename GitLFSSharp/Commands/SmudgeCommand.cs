using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLFSSharp.Commands
{
    class SmudgeCommand : ICommand
    {
        public int Run(string[] args)
        {
            const int BOMLength = 3;
            byte[] versionByteArray = Encoding.UTF8.GetBytes("version ");
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
                    int readBytes = stdin.Read(inputBuf, cnt, versionWithBOM.Length - cnt);

                    if (readBytes == 0)
                    {
                        break;
                    }

                    cnt += readBytes;
                }

                if (inputBuf.SequenceEqual(versionWithBOM) == true ||
                    inputBuf.Take(versionByteArray.Length).SequenceEqual(versionByteArray) == true)
                {
                    Console.WriteLine("hello world");
                }
                else
                {
                    // redirect stdin to stdout
                    byte[] buffer = new byte[8196];
                    using (var stdout = Console.OpenStandardOutput())
                    {
                        stdout.Write(inputBuf, 0, cnt);

                        int readBytes = stdin.Read(buffer, 0, buffer.Length);
                        stdout.Write(buffer, 0, readBytes);
                    }
                }
            }

            return 0;
        }
    }
}
