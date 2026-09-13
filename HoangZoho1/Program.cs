using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var contentRootPath = Directory.GetCurrentDirectory();
            var fallbackUsed = EnvironmentConstants.LoadFallbackAndValidate(contentRootPath);

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
                EnvironmentConstants.Get("SHARED_SYNCFUSION_LICENSE_KEY"));

            try
            {
                await EmailHelpers.SendEmail(new EmailContent
                {
                    Clients = EnvironmentConstants.Get("CONFIGURATION_NOTIFICATION_EMAIL"),
                    Subject = "HoangZoho1 configuration loaded successfully",
                    Body = $"All {RequiredEnvironmentVariables.Names.Count} required environment variables loaded successfully. " +
                           $"Local fallback used: {fallbackUsed}. UTC: {DateTime.UtcNow:O}",
                    SmtpServer = EmailConstants.Gmail_SmtpServer,
                    SmtpPort = EmailConstants.SmtpPort,
                    Email = EmailConstants.MyEmail_Username,
                    Password = EmailConstants.MyEmail_Password
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Configuration loaded, but the startup email failed: {ex.Message}");
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
