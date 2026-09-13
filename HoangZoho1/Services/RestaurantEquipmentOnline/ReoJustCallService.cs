using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.RestaurantEquipmentOnline.JustCall;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Services.RestaurantEquipmentOnline
{
    public class ReoJustCallService : IReoJustCallService
    {

        public async Task<ApiResultDto<GetUserByIdResponse>> GetUserById(int agentId)
        {

            var apiResult = new ApiResultDto<GetUserByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string endpoint = $"{REOConstants.JustCall_GetUserById_Endpoint}";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", 
                    $"{REOConstants.JustCall_ApiKey}:{REOConstants.JustCall_ApiSecret}");

                var userRequest = new { id = agentId };
                string requestBody = JsonConvert.SerializeObject(userRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                { 
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetUserByIdResponse>(responseData);
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
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<GetCallByIdResponse>> GetCallById(int callId)
        {
            var apiResult = new ApiResultDto<GetCallByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string endpoint = $"{REOConstants.JustCall_GetCallById_Endpoint}";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",
                    $"{REOConstants.JustCall_ApiKey}:{REOConstants.JustCall_ApiSecret}");

                var userRequest = new { id = callId };
                string requestBody = JsonConvert.SerializeObject(userRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetCallByIdResponse>(responseData);
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
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<GetListOfSMSResponse>> GetListOfSMS(string page, string per_page = "100", string order = "ASC")
        {

            var apiResult = new ApiResultDto<GetListOfSMSResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string endpoint = $"{REOConstants.JustCall_GetListOfSMS_Endpoint}";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",
                    $"{REOConstants.JustCall_ApiKey}:{REOConstants.JustCall_ApiSecret}");

                var searchRequest = new SearchRequest()
                {
                    page = page,
                    per_page = per_page,
                    order = order
                };
                string requestBody = JsonConvert.SerializeObject(searchRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetListOfSMSResponse>(responseData);
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
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<GetListOfCallResponse>> GetListOfCalls(string page, string per_page = "100", string order = "ASC")
        {

            var apiResult = new ApiResultDto<GetListOfCallResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string endpoint = $"{REOConstants.JustCall_GetListOfCalls_Endpoint}";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",
                    $"{REOConstants.JustCall_ApiKey}:{REOConstants.JustCall_ApiSecret}");

                var searchRequest = new SearchRequest()
                {
                    page = page,
                    per_page = per_page,
                    order = order
                };
                string requestBody = JsonConvert.SerializeObject(searchRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetListOfCallResponse>(responseData);
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
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

    }
}
