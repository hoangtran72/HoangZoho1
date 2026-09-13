using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.WclSolutions;
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

namespace HoangZoho1.Services.WclSolutions
{

    public class WclInventoryService : IWclInventoryService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public WclInventoryService(IZohoAuthService zohoAuthService)
        {

            _zohoAuthService = zohoAuthService;
        
        }

        public async Task<ApiResultDto<UpdateSalesOrderResponse>> UpdateSalesOrder(string salesOrderId, UpdateBatchNumberRequest updateBatchNumberRequest)
        {

            var apiResult = new ApiResultDto<UpdateSalesOrderResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(WclConstants.WCLSolutions, CommonConstants.ZohoInventory);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{WclConstants.ZohoInventory_EndpointV1}/salesorders/{salesOrderId}" +
                    $"?organization_id={WclConstants.ZohoInventory_OrganizationId}";

                string requestBody = JsonConvert.SerializeObject(updateBatchNumberRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
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
                    var responseObj = JsonConvert.DeserializeObject<UpdateSalesOrderResponse>(responseData);

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
