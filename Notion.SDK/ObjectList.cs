namespace Notion
{
    public class ObjectList<T>
    {
        public string Object { get; set; }
        public T[] Results { get; set; }
        public string? NextCursor { get; set; }
        public bool HasMore { get; set; }
    }
}