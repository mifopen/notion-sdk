using System;

namespace Notion
{
    public record NotionError
    {
        private NotionError(NotionErrorCode code, string message)
        {
            Code = code;
            Message = message;
        }

        public NotionErrorCode Code { get; }
        public string Message { get; }

        public static NotionError Create(string code, string message)
        {
            return new(
                code switch
                {
                    "invalid_json" => NotionErrorCode.InvalidJson,
                    "invalid_request_url" => NotionErrorCode.InvalidRequestUrl,
                    "invalid_request" => NotionErrorCode.InvalidRequest,
                    "validation_error" => NotionErrorCode.ValidationError,
                    "unauthorized" => NotionErrorCode.Unauthorized,
                    "restricted_resource" => NotionErrorCode.RestrictedResource,
                    "object_not_found" => NotionErrorCode.ObjectNotFound,
                    "conflict_error" => NotionErrorCode.ConflictError,
                    "rate_limited" => NotionErrorCode.RateLimited,
                    "internal_server_error" => NotionErrorCode.InternalServerError,
                    "service_unavailable" => NotionErrorCode.ServiceUnavailable,
                    _ => throw new ArgumentOutOfRangeException(nameof(code), code, null),
                },
                message
            );
        }
    }
}