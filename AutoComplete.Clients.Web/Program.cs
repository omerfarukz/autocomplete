using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel;
using System.IO;

namespace AutoComplete.Clients.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
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
            obj.Limits.MaxRequestBufferSize = 255;
            obj.ThreadCount = 4;
        }
    }
}
