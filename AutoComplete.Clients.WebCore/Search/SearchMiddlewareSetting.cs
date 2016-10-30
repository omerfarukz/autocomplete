namespace AutoComplete.Clients.WebCore.Search
{
    public class SearchMiddlewareSetting
    {
        public string Method { get; set; }

        public string KeywordName { get; set; }

        public string MaxItemCountName { get; set; }

        public string DbDirectory { get; set; }

        public bool UseMemorySearcher { get; set; }

        public string ResponseContentType { get; set; }
    }
}
