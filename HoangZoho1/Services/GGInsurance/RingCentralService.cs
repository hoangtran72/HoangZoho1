using Google.Apis.Util;
using HoangZoho1.Models.GGInsurance;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GGInsurance
{
    public class RingCentralService : IRingCentralService
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public RingCentralService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> SendMmsAsync(RingCentralMmsRequest requestData)
        {
            if (requestData == null)
                throw new ArgumentNullException(nameof(requestData));

            if (string.IsNullOrWhiteSpace(requestData.AccessToken))
                throw new ArgumentException("AccessToken is required.");

            if (string.IsNullOrWhiteSpace(requestData.FromNumber))
                throw new ArgumentException("FromNumber is required.");

            if (string.IsNullOrWhiteSpace(requestData.ToNumber))
                throw new ArgumentException("ToNumber is required.");

            var attachments = requestData.Attachments?
                .Where(file =>
                    file != null &&
                    !string.IsNullOrWhiteSpace(file.Base64))
                .ToList();

            var hasAttachments = attachments != null && attachments.Count > 0;

            using (var client = _httpClientFactory.CreateClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.Headers.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    requestData.AccessToken);

                if (!hasAttachments)
                {
                    // Standard SMS
                    request.RequestUri = new Uri(
                        "https://platform.ringcentral.com/restapi/v1.0/account/~/extension/~/sms");

                    var smsPayload = new
                    {
                        from = new
                        {
                            phoneNumber = requestData.FromNumber
                        },
                        to = new[]
                        {
                    new
                    {
                        phoneNumber = requestData.ToNumber
                    }
                },
                        text = requestData.Text ?? string.Empty
                    };

                    var json = JsonConvert.SerializeObject(smsPayload);

                    request.Content = new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json");
                }
                else
                {
                    // MMS with one or more attachments
                    request.RequestUri = new Uri(
                        "https://platform.ringcentral.com/restapi/v1.0/account/~/extension/~/mms");

                    var multipartContent = new MultipartFormDataContent();

                    try
                    {
                        multipartContent.Add(
                            new StringContent(requestData.ToNumber),
                            "to");

                        multipartContent.Add(
                            new StringContent(requestData.FromNumber),
                            "from");

                        multipartContent.Add(
                            new StringContent(requestData.Text ?? string.Empty),
                            "text");

                        var contentTypeProvider =
                            new FileExtensionContentTypeProvider();

                        foreach (var file in attachments)
                        {
                            byte[] fileBytes;

                            try
                            {
                                fileBytes = Convert.FromBase64String(file.Base64);
                            }
                            catch (FormatException exception)
                            {
                                throw new ArgumentException(
                                    string.Format(
                                        "Attachment '{0}' contains invalid Base64 data.",
                                        file.Name),
                                    exception);
                            }

                            var fileName = string.IsNullOrWhiteSpace(file.Name)
                                ? "attachment"
                                : file.Name;

                            string contentType;

                            if (!contentTypeProvider.TryGetContentType(
                                    fileName,
                                    out contentType))
                            {
                                contentType = "application/octet-stream";
                            }

                            var fileContent = new ByteArrayContent(fileBytes);

                            fileContent.Headers.ContentType =
                                MediaTypeHeaderValue.Parse(contentType);

                            multipartContent.Add(
                                fileContent,
                                "attachment",
                                fileName);
                        }

                        // HttpRequestMessage will dispose this content.
                        request.Content = multipartContent;
                        multipartContent = null;
                    }
                    finally
                    {
                        // Dispose only when ownership wasn't transferred.
                        if (multipartContent != null)
                            multipartContent.Dispose();
                    }
                }

                using (var response = await client.SendAsync(request))
                {
                    // Return RingCentral's exact success or error response.
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

    }
}
