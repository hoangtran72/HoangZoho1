using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.Twilio;
using HoangZoho1.Services.ZohoAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace HoangZoho1.Services.OneCorp
{

    public class OneCorpTwilioService : IOneCorpTwilioService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public OneCorpTwilioService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<ReadMultipleMessageResourcesResponse>> 
            ReadMultipleMessageResources(string queryParameter = "")
        {

            var apiResult = new ApiResultDto<ReadMultipleMessageResourcesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.RMMRR_400
            };

            try
            {
                string basicToken = StringHelpers.Base64Encode($"{OneCorpConstants.Twilio_AccountSID}:{OneCorpConstants.Twilio_AuthToken}");

                string endpoint = $"{OneCorpConstants.Twilio_Endpoint}/{OneCorpConstants.Twilio_ApiVersion}/Accounts/{OneCorpConstants.Twilio_AccountSID}/Messages.json";

                if (!string.IsNullOrEmpty(queryParameter))
                {
                    endpoint += queryParameter;
                }

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {basicToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<ReadMultipleMessageResourcesResponse>(responseData);

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = CommonConstants.MSG_200;
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
