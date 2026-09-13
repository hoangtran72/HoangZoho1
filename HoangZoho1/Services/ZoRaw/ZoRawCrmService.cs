using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.ZoRaw.ZohoCRM;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using HoangZoho1.Services.ZohoAuth;
using System.Text;

namespace HoangZoho1.Services.ZoRaw
{
    public class ZoRawCrmService : IZoRawCrmService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public ZoRawCrmService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<QueryBoxWeightsResponse>> QueryBoxWeights(ZohoCoqlRequest coqlRequest)
        {

            var apiResult = new ApiResultDto<QueryBoxWeightsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_200
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string requestBody = JsonConvert.SerializeObject(coqlRequest);

                string endpoint = $"{ZoRawConstants.ZohoCRM_EndpointV8}/coql";
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<QueryBoxWeightsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }
                else
                {
                    apiResult.Message = responseData;
                }

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<string>> SyncOrderFulfillmentFromSalesOrder(
            SyncOrderFulfillmentFromSalesOrderRequest syncRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_200
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZoRawConstants.ZoRawChocolates, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string requestBody = JsonConvert.SerializeObject(syncRequest);

                string endpoint = $"{ZoRawConstants.ZohoCRM_SyncOrderFulfillmentFromSalesOrder_Endpoint}";
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                // httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseData;
                }
                else
                {
                    apiResult.Message = responseData;
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
