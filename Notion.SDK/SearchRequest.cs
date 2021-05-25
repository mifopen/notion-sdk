namespace Notion
{
    internal class SearchRequest
    {
        public string? Query { get; set; }
        public SearchSort? Sort { get; set; }
        public SearchFilter? Filter { get; set; }

        internal class SearchSort
        {
            public string? Direction { get; set; }
            public string? Timestamp { get; set; }
        }

        internal class SearchFilter
        {
            public string Property { get; set; }
            public string Value { get; set; }
        }
    }
}