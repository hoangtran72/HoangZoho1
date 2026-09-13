using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.DaviesImagingGroup;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Services.DaviesImagingGroup
{

    public class DigCustomService : IDigCustomService
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public DigCustomService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResultDto<string>> SendSpecPlusEmail(SendSpecPlusEmailRequest sendSpecPlusEmailRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                 Code = ResultCode.BadRequest,
                 Message = DaviesImagingGroupConstants.SendSpecPlusEmail_400
            };

            try
            {
                // 1.Create the client
                var client = _httpClientFactory.CreateClient();

                // 2. Send the POST request
                // This automatically serializes 'data' to JSON and sets 'Content-Type: application/json'
                var jsonPayload = JsonConvert.SerializeObject(sendSpecPlusEmailRequest);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(DaviesImagingGroupConstants
                    .FrameFlowSendSpecPlusEmailStandalone_Endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = DaviesImagingGroupConstants.SendSpecPlusEmail_200;
                    return apiResult;
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        
        }

    }

}
