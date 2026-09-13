using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GGInsurance;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using HoangZoho1.Services.ZohoAuth;

namespace HoangZoho1.Services.GGInsurance
{

    public class GGInsuranceCrmService : IGGInsuranceCrmService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public GGInsuranceCrmService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        #region Accounts

        public async Task<ApiResultDto<SearchAccountsResponse>> 
            SearchAccountsByPhone(string phone)
        {

            var apiResult = new ApiResultDto<SearchAccountsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = string.Empty;
                accessToken = await _zohoAuthService
                        .GetAccessToken(GGInsuranceConstants.GGInsurance, CommonConstants.ZohoCRM);
                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }
                string encodedPhone = Uri.EscapeDataString(phone);

                string endpoint = $"{GGInsuranceConstants.ZohoCRM_EndpointV8}/Accounts/search?" +
                    $"phone=" + encodedPhone;
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SearchAccountsResponse>
                        (responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        #endregion

        #region Contacts

        public async Task<ApiResultDto<SearchContactsResponse>> 
            SearchContactsByPhone(string phone)
        {

            var apiResult = new ApiResultDto<SearchContactsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = string.Empty;
                accessToken = await _zohoAuthService
                        .GetAccessToken(GGInsuranceConstants.GGInsurance, CommonConstants.ZohoCRM);
                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string encodedPhone = Uri.EscapeDataString(phone);
                string endpoint = $"{GGInsuranceConstants.ZohoCRM_EndpointV8}/Contacts/search?" +
                    $"phone=" + encodedPhone;
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SearchContactsResponse>
                        (responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        #endregion

    }

}
