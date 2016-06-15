using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS.Utils
{
    public class GitRepo
    {
        /// <summary>
        /// Git 저장소의 루트 폴더를 찾는다
        /// 경로를 타고 올라가면서 .git이 있는 최초의 폴더를 찾는다
        /// </summary>
        /// <param name="path">루트를 찾을 경로, null을 입력할경우 GetCurrentDirectory()의 경로를 사용</param>
        /// <returns>.git 저장소의 경로. 못 찾은 경우 null<returns>
        public static string FindGitRoot(string path = null)
        {
            if (path == null)
            {
                path = Directory.GetCurrentDirectory();
            }

            string gitPath;
            while(true)
            {
                gitPath = Path.Combine(path, ".git");

                if (Directory.Exists(gitPath) == true )
                {
                    break;
                }

                var parent = Directory.GetParent(path);
                if ( parent == null )
                {
                    return null;
                }
                path = parent.FullName;
            }

            return gitPath;
        }

        /// <summary>
        /// .git/config의 경로를 반환 한다.
        /// </summary>
        /// <param name="gitPath">.git 폴더의 경로. 없는 경우 FindGitRoot를 통해 자동으로 찾는다</param>
        /// <returns>.git/config 의 경로. 못 찾은 경우 null<returns>
        public static string GetGitConfigPath(string gitPath = null)
        {
            if ( gitPath == null )
            {
                gitPath = FindGitRoot();

                // 경로를 얻어온 다음에도 null 이면 null 반환
                if ( gitPath == null )
                {
                    return null;
                }
            }

            return Path.Combine(gitPath, "config");
        }

        /// <summary>
        /// .git/config에서 remote 경로를 가져 온다.
        /// </summary>
        /// <param name="remoteName">remote의 이름. 기본값은 origin</param>
        /// <param name="gitConfigPath">.git/config의 경로</param>
        /// <returns>.git/config내의 remote 경로. config를 못찾거나 항목이 없는 경우 null<returns>
        public static string GetRemotePath(string remoteName = "origin", string gitConfigPath = null)
        {
            if (gitConfigPath == null)
            {
                gitConfigPath = GetGitConfigPath();

                // 경로를 얻어온 다음에도 null 이면 null 반환
                if (gitConfigPath == null )
                {
                    return null;
                }
            }

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(gitConfigPath);

            var sectionName = string.Format("remote \"{0}\"", remoteName);

            var remoteSection = data["remote \"origin\""];
            if (remoteSection.ContainsKey("url") == false)
            {
                return null;
            }

            return remoteSection["url"];
        }

        /// <summary>
        /// .git 폴더에서 LFS Object의 경로를 만들어서 반환해준다.
        /// </summary>
        /// <param name="oid">LFS Object의 oid</param>
        /// <returns>LFS Object가 위치할 경로</returns>
        public static string CreateLFSObjectPath(string oid, bool withOid = false)
        {
            if (withOid == false)
            {
                return string.Format("lfs/objects/{0}/{1}", oid.Substring(0, 2), oid.Substring(2, 2));
            }
            else
            {
                return string.Format("lfs/objects/{0}/{1}/{2}",oid.Substring(0, 2), oid.Substring(2, 2), oid);
            }
        }
    }
}
