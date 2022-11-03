namespace AutoComplete.Domain
{
    public class SearchOptions
    {
        public string Term { get; set; }

        public int MaxItemCount { get; set; }

        public bool SuggestWhenFoundStartsWith { get; set; }
    }
}