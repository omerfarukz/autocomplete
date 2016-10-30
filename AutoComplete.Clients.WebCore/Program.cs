using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel;
using System.IO;

namespace AutoComplete.Clients.WebCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel(kestrelActions)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        private static void kestrelActions(KestrelServerOptions obj)
        {
            obj.AddServerHeader = false;
            obj.NoDelay = true;
            obj.MaxRequestBufferSize = 255;
            obj.ThreadCount = 4;
        }
    }
}
