using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS.Apis
{
    /// <summary>
    /// LFS HTTP Client
    /// </summary>
    public class HTTPApi
    {
        /// <summary>
        /// LFS Batch(Download) 명령으로 LFS Pointer로 실제 객체 정보를 가져 온다
        /// 
        /// 추가로 DownloadAsync 함수를 실행하여 실제 객체를 받아 올 수 있다.
        /// </summary>
        /// <param name="baseUrl">저장소 URL</param>
        /// <param name="objects">요청할 LFS Object 정보</param>
        /// <returns>서버에서 받은 HttpBatchResponse 응답</returns>
        public static async Task<Structs.HttpBatchResponse> BatchDownloadQueryAsync(string baseUrl, List<Entities.LFSPointer> objects)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,
                     baseUrl + "/info/lfs/objects/batch");

                var batchRequest = new Structs.HttpBatchRequest();
                batchRequest.operation = Structs.HttpBatchRequest.OperationType.download;
                batchRequest.objects = new List<Structs.HttpBatchRequestObject>();

                foreach (var obj in objects)
                {
                    batchRequest.objects.Add(new Structs.HttpBatchRequestObject()
                    {
                        oid = obj.oid,
                        size = obj.size
                    });
                }

                var requestJson = Jil.JSON.Serialize<Structs.HttpBatchRequest>(batchRequest, Jil.Options.IncludeInherited);
                request.Content = new StringContent(requestJson, Encoding.UTF8, "application/vnd.git-lfs+json");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.git-lfs+json"));

                System.Diagnostics.Trace.WriteLine(requestJson);
                var response = await client.SendAsync(request);

                var responseText = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Trace.WriteLine(responseText);

                // TODO: 여기서 올바른 내용인지 검사해야함
                var responseObj = Jil.JSON.Deserialize<Structs.HttpBatchResponse>(responseText, Jil.Options.IncludeInherited);
                
                return responseObj;
            }
        }

        /// <summary>
        /// LFS 객체를 실제 다운로드 하는 함수
        /// Batch 명령으로 객체 주소를 알아낸 후에 받아 올 수 있다.
        /// </summary>
        /// <param name="url">객체 위치 ( Batch 명령으로 가지고 온 URL )</param>
        /// <param name="headers">요청 헤더</param>
        /// <returns>byte[]로 이루어진 LFS 파일의 데이터</returns>
        public static async Task<byte[]> DownloadAsync(string url, Dictionary<string, string> headers = null)
        {
            using (HttpClient client = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        if (header.Key.ToLower().Equals("authorization"))
                        {
                            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(header.Value);
                        }
                        else
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                var response = await client.GetAsync(url);

                return await response.Content.ReadAsByteArrayAsync();
            }
        }
    }
}
