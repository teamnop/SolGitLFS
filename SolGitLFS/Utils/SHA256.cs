﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS.Utils
{
    public class SHA256
    {
        /// <summary>
        /// sha256 hash를 계산
        /// </summary>
        /// <param name="data">hash를 구할 데이터</param>
        /// <returns>string으로 변환한 sha256 hash</returns>
        public static string CalcHash(byte[] data)
        {
            using (var sha256 = System.Security.Cryptography.SHA256Managed.Create())
            {
                var hash = sha256.ComputeHash(data);

                return BitConverter.ToString(hash).Replace("-", String.Empty).ToLower();
            }
        }

        /// <summary>
        /// sha256 hash를 계산
        /// </summary>
        /// <param name="stream">hash를 구할 stream</param>
        /// <returns>string으로 변환한 sha256 hash</returns>
        public static string CalcHash(Stream stream)
        {
            using (var sha256 = System.Security.Cryptography.SHA256Managed.Create())
            {
                var hash = sha256.ComputeHash(stream);

                return BitConverter.ToString(hash).Replace("-", String.Empty).ToLower();
            }
        }

        /// <summary>
        /// ByteArray를 Seralize하여 hash string으로 변경
        /// </summary>
        /// <param name="hash">변경할 ByteArray</param>
        /// <returns>hash string</returns>
        public static string ByteArrayToHash(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", String.Empty).ToLower();
        }
    }
}