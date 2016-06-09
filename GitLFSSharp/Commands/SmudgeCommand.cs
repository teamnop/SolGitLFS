using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLFSSharp.Commands
{
    class SmudgeCommand : ICommand
    {
        public async Task<int> Run(string[] args)
        {
            // 우선 "version " 으로 시작 되는지 체크한다
            // 위의 문자열로 시작 하지 않는다면 그냥 무시하고 그대로 stdout으로 출력한다

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
                    Dictionary<string, string> inputParams = new Dictionary<string, string>();

                    // 처리하기 쉽게 하기 위해 모든 내용을 memory로 읽어드리고 파싱한다
                    byte[] buffer = new byte[8196];

                    using (var ms = new MemoryStream())
                    {
                        ms.Write(inputBuf, 0, cnt);

                        while (true)
                        {
                            int readBytes = stdin.Read(buffer, 0, buffer.Length);
                            if (readBytes <= 0)
                            {
                                break;
                            }

                            ms.Write(buffer, 0, readBytes);
                        }

                        var inputString = Encoding.UTF8.GetString(ms.ToArray());
                        using (var reader = new StringReader(inputString))
                        {
                            while (true)
                            {
                                var row = reader.ReadLine();
                                if (row == null)
                                {
                                    break;
                                }

                                var splitIndex = row.IndexOf(' ');
                                if (splitIndex < 0)
                                {
                                    throw new Exception("올바른 LFS 파일이 아닙니다");
                                }

                                var key = row.Substring(0, splitIndex);
                                var value = row.Substring(splitIndex + 1);

                                inputParams.Add(key, value);
                            }
                        }
                    }

                    // 파일 파싱이 끝났으면 실제파일을 받아와서 stdout으로 보내준다
                    var lfsObject = new SolGitLFS.Entities.LFSPointer()
                    {
                        oid = inputParams["oid"].Replace("sha256:", ""),
                        size = long.Parse(inputParams["size"])
                    };

                    var batchResult =
                        await
                            SolGitLFS.Apis.HTTPApi.BatchDownloadQueryAsync("https://github.com/teamnop/LFSTestRepo.git",
                                new List<SolGitLFS.Entities.LFSPointer>() {lfsObject});

                    var lfsBatchResult = batchResult.objects.FirstOrDefault();
                    if (lfsBatchResult == null)
                    {
                        throw new Exception("LFS Batch 응답이 올바르지 않습니다.");
                    }

                    var LfsDownloadResult =
                        await SolGitLFS.Apis.HTTPApi.DownloadAsync(lfsBatchResult.actions.download.href,
                            lfsBatchResult.actions.download.header);

                    if (LfsDownloadResult == null)
                    {
                        throw new Exception("LFS Download 결과가 올바르지 않습니다");
                    }

                    using (var stdout = Console.OpenStandardOutput())
                    {
                        stdout.Write(LfsDownloadResult, 0, LfsDownloadResult.Length);
                    }
                }
                else
                {
                    // redirect stdin to stdout
                    byte[] buffer = new byte[8196];

                    using (var stdout = Console.OpenStandardOutput())
                    {
                        stdout.Write(inputBuf, 0, cnt);

                        while (true)
                        {
                            int readBytes = stdin.Read(buffer, 0, buffer.Length);
                            if (readBytes <= 0)
                            {
                                break;
                            }

                            stdout.Write(buffer, 0, readBytes);
                        }
                    }
                }
            }

            return 0;
        }
    }
}
