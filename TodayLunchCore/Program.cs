using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TodayLunchCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => 
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseApplicationInsights()
            //.ConfigureAppConfiguration((context, config) =>
            //{
            //    var builtConfig = config.Build();
            //    var vaultUrl = "https://todaylunchkeyvault.vault.azure.net/";
            //    config.AddAzureKeyVault(vaultUrl);
            //})
            .UseStartup<Startup>();
    }
}
