using AutoComplete.Core.DataSource;
using System.Collections.Generic;
using System.IO;

namespace AutoComplete.UnitTests.Source
{
    public class FakeKeywordDataSource : IKeywordDataSource
    {
        public string Name
        {
            get { return this.GetType().Name; }
        }

        public IEnumerable<string> GetKeywords()
        {
            //https://github.com/first20hours/google-10000-english/blob/master/20k.txt
            return File.ReadAllLines("Resource\\20k.txt");
        }
    }
}