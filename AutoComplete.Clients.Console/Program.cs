using AutoComplete.Clients.IndexSearchers;
using AutoComplete.Core.Builders;
using AutoComplete.Core.Domain;
using AutoComplete.Core.Searchers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace AutoComplete.Clients.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string headerPath = "header.json";
            string indexPath = "index.bin";
            string tailPath = "tail.txt";

            if (File.Exists(headerPath))
                File.Delete(headerPath);

            if (File.Exists(indexPath))
                File.Delete(indexPath);

            if (File.Exists(tailPath))
                File.Delete(tailPath);

            using (var header = new FileStream(headerPath, FileMode.Create))
            {
                using (var index = new FileStream(indexPath, FileMode.Create))
                {
                    using (var tail = new FileStream(tailPath, FileMode.Create))
                    {
                        var builder = new IndexBuilder(header, index, tail);

                        builder.Add("airplane");
                        builder.Add("bus");
                        builder.Add("car");

                        builder.Build();
                    }
                }
            }


            IIndexSearcher searcher = new InMemoryIndexSearcher(headerPath, indexPath, tailPath);

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
