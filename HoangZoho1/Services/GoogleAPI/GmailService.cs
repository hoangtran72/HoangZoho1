using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GoogleAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace HoangZoho1.Services.GoogleAPI
{
    public class GmailService : IGmailService
    {
        private readonly IGoogleAuthService _googleAuthService;

        public GmailService(IGoogleAuthService googleAuthService)
        {
            _googleAuthService = googleAuthService;
        }

        public async Task<ApiResultDto<GetEmailByIdResponse>> GetGmailById(string gmailId)
        {
            var apiResult = new ApiResultDto<GetEmailByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = GoogleAPIConstants.SearchEmailMessages_400
            };

            try
            {
                string accessToken = await _googleAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{GoogleAPIConstants.GmailEndpoint}/users/{HttpUtility.UrlEncode(REOConstants.REO_SalesEmail)}" +
                    $"/messages/{gmailId}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetEmailByIdResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = GoogleAPIConstants.SearchEmailMessages_200;
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

        public async Task<ApiResultDto<SearchGmailsResponse>> SearchForEmails(string searchQuery)
        {
            var apiResult = new ApiResultDto<SearchGmailsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = GoogleAPIConstants.SearchEmailMessages_400
            };

            try
            {
                string accessToken = await _googleAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{GoogleAPIConstants.GmailEndpoint}/users/{HttpUtility.UrlEncode(REOConstants.REO_SalesEmail)}" +
                    $"/messages?q={HttpUtility.UrlEncode(searchQuery)}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SearchGmailsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = GoogleAPIConstants.SearchEmailMessages_200;
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
