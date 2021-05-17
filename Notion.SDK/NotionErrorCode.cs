namespace Notion
{
    public enum NotionErrorCode
    {
        InvalidJson,
        InvalidRequestUrl,
        InvalidRequest,
        ValidationError,
        Unauthorized,
        RestrictedResource,
        ObjectNotFound,
        ConflictError,
        RateLimited,
        InternalServerError,
        ServiceUnavailable,
    }
}