using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
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
            var result = await GetResponse<JsonElement>(response, cancellationToken);
            if (result.IsFailure)
            {
                return new NotionResponse<Page>(result.GetError());
            }

            var value = result.GetValue();
            return new NotionResponse<Page>(PageJsonConverter.Convert(value));
        }

        public async Task<NotionResponse<ObjectList<Page>>> Search(string authToken, string? query = null,
            SearchFilterObjectType? filterObjectType = null,
            CancellationToken cancellationToken = default)
        {
            var requestBody = new SearchRequest
            {
                Query = query,
                Filter = filterObjectType == null
                    ? null
                    : new SearchRequest.SearchFilter
                    {
                        Property = "object",
                        Value = filterObjectType.Value.ToString().ToLower(),
                    },
            };
            var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(
                stream,
                requestBody,
                new JsonSerializerOptions
                {
                    IgnoreNullValues = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                },
                cancellationToken);
            stream.Position = 0;

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.notion.com/v1/search")
            {
                Content = new StreamContent(stream)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json"),
                    },
                },
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            request.Headers.Add("Notion-Version", ApiVersion);
            var response = await httpClient.SendAsync(request, cancellationToken);
            var result = await GetResponse<ObjectList<JsonElement>>(response, cancellationToken);
            if (result.IsFailure)
            {
                return new NotionResponse<ObjectList<Page>>(result.GetError());
            }

            var value = result.GetValue();
            return new NotionResponse<ObjectList<Page>>(new ObjectList<Page>
            {
                Object = value.Object,
                Results = value.Results.Select(PageJsonConverter.Convert).ToArray(),
                HasMore = value.HasMore,
                NextCursor = value.NextCursor,
            });
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
            var result = await GetResponse<ObjectList<JsonDocument>>(response, cancellationToken);
            if (result.IsFailure)
            {
                return new NotionResponse<ObjectList<Block>>(result.GetError());
            }

            var value = result.GetValue();
            return new NotionResponse<ObjectList<Block>>(new ObjectList<Block>
            {
                Object = value.Object,
                Results = value.Results.Select(BlockJsonConverter.Convert).ToArray(),
                HasMore = value.HasMore,
                NextCursor = value.NextCursor,
            });
        }

        public async Task<NotionResponse<TokenResponse>> ExchangeCodeForOAuthToken(
            string clientId,
            string clientSecret,
            string code,
            string redirectUri,
            CancellationToken cancellationToken = default
        )
        {
            var byteArray = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
            var requestBody = new
            {
                grant_type = "authorization_code",
                code,
                redirect_uri = redirectUri,
            };
            var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, requestBody, cancellationToken: cancellationToken);
            stream.Position = 0;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.notion.com/v1/oauth/token"),
                Headers =
                {
                    Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray)),
                },
                Content = new StreamContent(stream)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json"),
                    },
                },
            };
            var response = await httpClient.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return new NotionResponse<TokenResponse>(
                    (await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken))!);
            }

            var errorJson = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync(cancellationToken),
                cancellationToken: cancellationToken);
            var error = errorJson.RootElement.GetProperty("error").GetString();
            var notionError = NotionError.Create("exchange_for_token_failed", error!);
            return new NotionResponse<TokenResponse>(notionError);
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