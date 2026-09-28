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
using System.Net;
using System.Threading.Tasks;

namespace HoangZoho1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var contentRootPath = Directory.GetCurrentDirectory();
            bool fallbackUsed;
            try
            {
                fallbackUsed = EnvironmentConstants.LoadFallbackAndValidate(contentRootPath);
            }
            catch (Exception ex)
            {
                await TrySendConfigurationFailureEmail(ex);
                throw;
            }

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

        private static async Task TrySendConfigurationFailureEmail(Exception configurationException)
        {
            try
            {
                var sender = Environment.GetEnvironmentVariable("EMAIL_MY_EMAIL_USERNAME");
                var password = Environment.GetEnvironmentVariable("EMAIL_MY_EMAIL_PASSWORD");
                var recipient = Environment.GetEnvironmentVariable("CONFIGURATION_NOTIFICATION_EMAIL");

                // If the dedicated recipient is the missing setting, notify the sender account.
                if (string.IsNullOrWhiteSpace(recipient))
                    recipient = sender;

                if (string.IsNullOrWhiteSpace(sender) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(recipient))
                {
                    Console.Error.WriteLine(
                        "Configuration loading failed, but the failure email could not be sent because the email settings were unavailable.");
                    return;
                }

                await EmailHelpers.SendEmail(new EmailContent
                {
                    Clients = recipient,
                    Subject = "HoangZoho1 configuration failed to load",
                    Body = $"Required environment variables could not be loaded. " +
                           $"Host: {WebUtility.HtmlEncode(Environment.MachineName)}. " +
                           $"UTC: {DateTime.UtcNow:O}.<br><br>" +
                           $"Error: {WebUtility.HtmlEncode(configurationException.Message)}",
                    SmtpServer = EmailConstants.Gmail_SmtpServer,
                    SmtpPort = EmailConstants.SmtpPort,
                    Email = sender,
                    Password = password
                });
            }
            catch (Exception emailException)
            {
                Console.Error.WriteLine(
                    $"Configuration loading failed, and the failure email could not be sent: {emailException.Message}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
