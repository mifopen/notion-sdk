using System;

namespace Notion
{
    public record NotionResponse<T>
    {
        public NotionResponse(T value)
        {
            Value = value;
        }

        public NotionResponse(NotionError error)
        {
            Error = error;
        }

        public T? Value { get; }
        public NotionError? Error { get; }
        public bool IsSuccess => Error == null;
        public bool IsFailure => Error != null;

        public T GetValue()
        {
            if (Value == null)
            {
                throw new InvalidOperationException($"There's no value in the response {this}");
            }

            return Value;
        }

        public NotionError GetError()
        {
            if (Error == null)
            {
                throw new InvalidOperationException($"There's no error in the response {this}");
            }

            return Error;
        }
    }
}