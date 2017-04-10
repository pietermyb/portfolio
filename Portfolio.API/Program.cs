using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Portfolio.API
{
    /// <summary>
    /// Program class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Program entry Main method
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel(options =>
                {
                    options.NoDelay = true;
                    //Assign the cert to allow https
                    options.UseHttps("portfolio.pfx", "YourVerySecurePassword");
                    options.UseConnectionLogging();
                    options.AddServerHeader = false;
                })
                //Serve http and https
                .UseUrls("https://0.0.0.0:5000", "http://0.0.0.0:5001")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}