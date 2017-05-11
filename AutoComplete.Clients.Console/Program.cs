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
            bool useTailFile = true;

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
                        IndexBuilder builder = null;
                        if (useTailFile)
                            builder = new IndexBuilder(header, index, tail);
                        else
                            builder = new IndexBuilder(header, index);


                        builder.Add("ade");
                        builder.Add("car");
                        builder.Add("folk");
                        builder.Add("xyz");

                        builder.Build();
                    }
                }
            }


            IIndexSearcher searcher = null;
            if (useTailFile)
                searcher = new InMemoryIndexSearcher(headerPath, indexPath, tailPath);
            else
                searcher = new InMemoryIndexSearcher(headerPath, indexPath);

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
