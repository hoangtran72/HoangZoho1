using HoangZoho1.Models.Common;
using System.Net.Mail;
using System;
using System.Text;
using System.Security.Cryptography;

namespace HoangZoho1.Helpers
{

    public class XeroHelpers
    {

        public static bool IsXeroWebhookValid(string rawRequestBody, string receivedSignature, string webhookKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(webhookKey);
            var bodyBytes = Encoding.UTF8.GetBytes(rawRequestBody);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(bodyBytes);
                var computedSignature = Convert.ToBase64String(hashBytes);

                return computedSignature == receivedSignature;
            }
        }

    }

}
