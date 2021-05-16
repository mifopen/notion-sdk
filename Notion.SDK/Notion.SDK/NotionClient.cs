using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

        public async Task<Page> GetPage(string pageId, string authToken, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.notion.com/v1/pages/{pageId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            request.Headers.Add("Notion-Version", ApiVersion);
            var response = await httpClient.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Page>(cancellationToken: cancellationToken) ??
                       throw new InvalidOperationException();
            }

            throw new NotImplementedException();
        }

        public async Task<ObjectList<Block>> GetBlockChildren(
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
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ObjectList<Block>>(
                           cancellationToken: cancellationToken) ??
                       throw new InvalidOperationException();
            }

            throw new NotImplementedException();
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