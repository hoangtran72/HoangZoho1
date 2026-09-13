using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Zipfox.WhatsApp;
using HoangZoho1.Models.Zipfox.ZohoCRM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Zipfox
{

    public class ZipfoxWhatsAppService : IZipfoxWhatsAppService
    {

        private readonly IZipfoxCrmService _zipfoxCrmService;

        public ZipfoxWhatsAppService(IZipfoxCrmService zipfoxCrmService)
        {
            _zipfoxCrmService = zipfoxCrmService;
        }

        public async Task<ApiResultDto<SendWhatsAppMessageResponse>> SendWhatsAppMessageByTemplate(
            string phoneNumberId, SendWhatsAppMessageByTemplateRequest whatsAppRequest)
        {

            var apiResult = new ApiResultDto<SendWhatsAppMessageResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string endpoint = $"{ZipfoxConstants.WhatsAppEndpoint}/{phoneNumberId}/messages";

                string requestBody = JsonConvert.SerializeObject(whatsAppRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ZipfoxConstants.PermanentToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SendWhatsAppMessageResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                string templateName = whatsAppRequest.template.name;
                if (templateName == "notificaciones_de_zipfox")
                {

                    string toNumber = whatsAppRequest.to;

                    // Get Contact Id
                    string phone = toNumber.Substring(3);
                    var searchContactsResponse = await _zipfoxCrmService.SearchContactsByPhone(phone);
                    string contactId = "";
                    if (searchContactsResponse.Code == ResultCode.OK)
                    {
                        var contactDetails = searchContactsResponse.Data.data[0];
                        contactId = contactDetails.id;
                    }

                    // Update Contact Notification Sent to true
                    if (!string.IsNullOrEmpty(contactId))
                    {
                        var upsertRequest = new UpsertRequest<ContactForUpdation>();
                        var contactForUpdation = new ContactForUpdation()
                        {
                            Notification_sent = true
                        };
                        upsertRequest.data.Add(contactForUpdation);
                        var upsertResponse = await _zipfoxCrmService.UpdateContact(contactId, upsertRequest);
                    }

                }

                return apiResult;

            }
            catch (Exception)
            {
                return apiResult;
            }

        }

        public async Task<ApiResultDto<SendWhatsAppMessageResponse>> SendWhatsAppMessageByText(string phoneNumberId, SendWhatsAppMessageByTextRequest whatsAppRequest)
        {

            var apiResult = new ApiResultDto<SendWhatsAppMessageResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string endpoint = $"{ZipfoxConstants.WhatsAppEndpoint}/{phoneNumberId}/messages";

                string requestBody = JsonConvert.SerializeObject(whatsAppRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ZipfoxConstants.PermanentToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SendWhatsAppMessageResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }

        }

        public async Task<ApiResultDto<GetWhatsAppTemplatesResponse>> GetWhatsAppTemplates()
        {
            var apiResult = new ApiResultDto<GetWhatsAppTemplatesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string endpoint = $"{ZipfoxConstants.WhatsAppEndpoint}/{ZipfoxConstants.WhatsAppBusinessId}/message_templates";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ZipfoxConstants.PermanentToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetWhatsAppTemplatesResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;

            }
            catch (Exception)
            {
                return apiResult;
            }
        }

    }

}
