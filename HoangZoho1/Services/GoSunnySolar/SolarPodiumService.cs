using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GoSunnySolar.Podium;
using HoangZoho1.Services.ZohoAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GoSunnySolar
{
    public class SolarPodiumService : ISolarPodiumService
    {

        private readonly IPodiumAuthService _podiumAuthService;

        public SolarPodiumService(IPodiumAuthService podiumAuthService)
        {
            _podiumAuthService = podiumAuthService;
        }

        public async Task<ApiResultDto<GetTemplatesResponse>> GetTemplates()
        {

            var apiResult = new ApiResultDto<GetTemplatesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _podiumAuthService
                    .GetAccessToken(GoSunnySolarConstants.GoSunnySolar);
                string endpoint = $"{GoSunnySolarConstants.PodiumApiEndpointV4}/templates?locationUid=b4347909-c339-5a07-a63e-0dd4689fdedf";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetTemplatesResponse>(responseData);
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

        public async Task<ApiResultDto<SendMessageResponse>> 
            SendMessage(SendMessageRequest sendMessageRequest)
        {
            var apiResult = new ApiResultDto<SendMessageResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _podiumAuthService
                    .GetAccessToken(GoSunnySolarConstants.GoSunnySolar);
                string endpoint = $"{GoSunnySolarConstants.PodiumApiEndpointV4}/messages";

                string requestBody = JsonConvert.SerializeObject(sendMessageRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SendMessageResponse>(responseData);
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
