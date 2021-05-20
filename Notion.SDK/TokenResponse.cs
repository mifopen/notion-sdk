using System.Text.Json.Serialization;

namespace Notion
{
    public class TokenResponse
    {
        [JsonPropertyName("access_token")] public string AccessToken { get; set; }
        [JsonPropertyName("workspace_name")] public string WorkspaceName { get; set; }
        [JsonPropertyName("workspace_icon")] public string WorkspaceIcon { get; set; }
        [JsonPropertyName("bot_id")] public string BotId { get; set; }
    }
}