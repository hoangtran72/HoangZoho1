using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.RestaurantEquipmentOnline.Shopify;
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
    public class ReoShopifyService : IReoShopifyService
    {

        public async Task<ApiResultDto<GetQuoteDetailsResponse>> GetQuoteDetails(GetQuoteDetailsRequest quoteRequest)
        {
            var apiResult = new ApiResultDto<GetQuoteDetailsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string endpoint = $"{REOConstants.ShopifyEndpoint}/get-quote-detail";

                string requestBody = JsonConvert.SerializeObject(quoteRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("token", REOConstants.ShopifyToken);
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetQuoteDetailsResponse>(responseData);
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
