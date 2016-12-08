namespace AutoComplete.Clients.Web.Search
{
    public class SearchMiddlewareSetting
    {
        public string Method { get; set; }

        public string KeywordName { get; set; }

        public string MaxItemCountName { get; set; }

        public string DbDirectory { get; set; }

        public bool UseMemorySearcher { get; set; }

        public bool UseTailFile { get; set; }

        public string ResponseContentType { get; set; }

        public bool SuggestWhenFoundStartsWith { get; set; }

        public int DefaultMaxItemCount { get; set; }
    }
}
