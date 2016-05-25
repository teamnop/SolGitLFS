using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SolGitLFS.Apis
{
    public class HTTPApi
    {
        public static async Task<Structs.HttpBatchResponse> DownloadQuery(string baseUrl, List<Entities.LFSObject> objects)
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

                Console.WriteLine(requestJson);
                var response = await client.SendAsync(request);

                var responseText = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseText);

                // TODO: 여기서 올바른 내용인지 검사해야함
                var responseObj = Jil.JSON.Deserialize<Structs.HttpBatchResponse>(responseText, Jil.Options.IncludeInherited);
                
                return responseObj;
            }
        }

        public void Download(string url)
        {

        }
    }
}
