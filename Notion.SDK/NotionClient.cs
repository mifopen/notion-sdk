using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Notion
{
    public class NotionClient
    {
        private readonly HttpClient httpClient;
        public const string ApiVersion = "2021-05-13";

        public NotionClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<NotionResponse<Page>> GetPage(string pageId, string authToken,
            CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.notion.com/v1/pages/{pageId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            request.Headers.Add("Notion-Version", ApiVersion);
            var response = await httpClient.SendAsync(request, cancellationToken);
            return await GetResponse<Page>(response, cancellationToken);
        }


        public async Task<NotionResponse<ObjectList<Block>>> GetBlockChildren(
            string blockId,
            string authToken,
            string? startCursor = null,
            int? pageSize = null,
            CancellationToken cancellationToken = default
        )
        {
            var uri = new UriBuilder($"https://api.notion.com/v1/blocks/{blockId}/children")
            {
                Query = BuildQuery(
                    ("start_cursor", startCursor),
                    ("page_size", pageSize?.ToString())
                ),
            };
            var request = new HttpRequestMessage(HttpMethod.Get, uri.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            request.Headers.Add("Notion-Version", ApiVersion);
            var response = await httpClient.SendAsync(request, cancellationToken);
            return await GetResponse<ObjectList<Block>>(response, cancellationToken);
        }

        private static async Task<NotionResponse<T>> GetResponse<T>(HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            if (response.IsSuccessStatusCode)
            {
                return new NotionResponse<T>(
                    await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken) ??
                    throw new InvalidOperationException()
                );
            }

            return await GetError<T>(response, cancellationToken);
        }

        private static async Task<NotionResponse<T>> GetError<T>(HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            var errorJson = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync(cancellationToken),
                cancellationToken: cancellationToken);
            var code = errorJson.RootElement.GetProperty("code").GetString();
            var message = errorJson.RootElement.GetProperty("message").GetString();
            var notionError = NotionError.Create(code!, message!);
            return new NotionResponse<T>(notionError);
        }

        private static string? BuildQuery(params (string key, string? value)[] pairs)
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            var pairsWithValues = pairs.Where(x => !string.IsNullOrEmpty(x.value)).ToArray();
            if (pairsWithValues.Length == 0)
            {
                return null;
            }

            foreach (var (key, value) in pairsWithValues)
            {
                queryString.Add(key, value);
            }

            return queryString.ToString();
        }
    }
}