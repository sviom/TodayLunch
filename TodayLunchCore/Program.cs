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
            .ConfigureAppConfiguration((context, config) =>
            {
                // Build the current set of configuration to load values from
                // JSON files and environment variables, including VaultName.
                var builtConfig = config.Build();

                // Use VaultName from the configuration to create the full vault URL.
                //var vaultUrl = $"https://{builtConfig["VaultName"]}.vault.azure.net/";
                var vaultUrl = "https://todaylunchkeyvault.vault.azure.net/";
                //https://todaylunchkeyvault.vault.azure.net/

                // Load all secrets from the vault into configuration. This will automatically
                // authenticate to the vault using a managed identity. If a managed identity
                // is not available, it will check if Visual Studio and/or the Azure CLI are
                // installed locally and see if they are configured with credentials that can
                // access the vault.
                config.AddAzureKeyVault(vaultUrl);
            })
            .UseStartup<Startup>();
    }
}
