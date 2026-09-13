using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Signarama.ZohoBooks;
using HoangZoho1.Services.ZohoAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Signarama
{
    public class SignaramaBooksService : ISignaramaBooksService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public SignaramaBooksService(IZohoAuthService zohoAuthService)
        {
            
            _zohoAuthService = zohoAuthService;
        
        }

        public async Task<ApiResultDto<UploadAttachmentResponse>> UploadAttachment
            (string filePath, string moduleName, string moduleId)
        {

            var apiResult = new ApiResultDto<UploadAttachmentResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                    .GetAccessToken(SignaramaConstants.Signarama, CommonConstants.ZohoBooks);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                var requestContent = new MultipartFormDataContent();
                var byteArray = File.ReadAllBytes(filePath);
                var pdfContent = new ByteArrayContent(byteArray);
                pdfContent.Headers.ContentType =
                    MediaTypeHeaderValue.Parse("application/pdf");
                string fileName = Path.GetFileName(filePath);
                requestContent.Add(pdfContent, "attachment", fileName);

                string endpoint = $"https://www.zohoapis.com/books/v3/{moduleName}/{moduleId}/attachment?organization_id=" +
                    $"{SignaramaConstants.ZohoBooks_OrganizationId}";

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = requestContent
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject
                        <UploadAttachmentResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return apiResult;
            }

        }

    }
}
