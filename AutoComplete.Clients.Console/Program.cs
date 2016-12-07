using AutoComplete.Clients.IndexSearchers;
using AutoComplete.Core.Builders;
using AutoComplete.Core.Domain;
using AutoComplete.Core.Searchers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AutoComplete.Clients.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string headerPath = "header.json";
            string indexPath = "index.bin";
            
            IIndexSearcher searcher = new InMemoryIndexSearcher(headerPath, indexPath);

            var sw = new Stopwatch();

            while (true)
            {
                System.Console.WriteLine("Type any term");
                string term = System.Console.ReadLine();

                sw.Restart();

                SearchResult searchResult = searcher.Search(term, 5, false);
                sw.Stop();

                if (searchResult != null && searchResult.Items != null)
                {
                    foreach (var item in searchResult.Items)
                    {
                        System.Console.WriteLine(item);
                    }
                }

                System.Console.WriteLine("Elapsed ms: " + sw.Elapsed.TotalMilliseconds);
            }
        }
    }
}
