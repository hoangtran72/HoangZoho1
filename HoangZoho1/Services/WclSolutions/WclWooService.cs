using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.WclSolutions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Services.WclSolutions
{
    public class WclWooService : IWclWooService
    {

        public async Task<ApiResultDto<GetWooOrderResponse>> GetWooOrderByNumber(string wooOrderNumber)
        {

            var apiResult = new ApiResultDto<GetWooOrderResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = "Get Woo Order FAILED"
            };

            try
            {

                if (string.IsNullOrEmpty(wooOrderNumber))
                {
                    return apiResult;
                }

                string wooEndpoint = string.Empty;
                string clientId = string.Empty;
                string clientSecret = string.Empty;

                if (wooOrderNumber.StartsWith("EU",
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    wooEndpoint = WclConstants.EU_Woo_Endpoint_V3;
                    clientId = WclConstants.EU_Client_Id;
                    clientSecret = WclConstants.EU_Client_Secret;
                }
                else if (wooOrderNumber.StartsWith("UK",
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    wooEndpoint = WclConstants.UK_Woo_Endpoint_V3;
                    clientId = WclConstants.UK_Client_Id;
                    clientSecret = WclConstants.UK_Client_Secret;
                }
                else if (wooOrderNumber.StartsWith("RW",
                    StringComparison.InvariantCultureIgnoreCase) || 
                    wooOrderNumber.StartsWith("KSA", 
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    wooEndpoint = WclConstants.RW_Woo_Endpoint_V3;
                    clientId = WclConstants.RW_Client_Id;
                    clientSecret = WclConstants.RW_Client_Secret;
                }
                else
                {
                    apiResult.Code = ResultCode.BadRequest;
                    apiResult.Message = "Invalid Woo Order Number";
                    return apiResult;
                }

                string getOrderByNumberEndpoint = $"{wooEndpoint}/orders/{wooOrderNumber}";

                using (var client = new System.Net.Http.HttpClient())
                {

                    var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(getOrderByNumberEndpoint);
                    string responseStr = JsonConvert.SerializeObject(response);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var wooOrderResponse = Newtonsoft.Json.JsonConvert
                            .DeserializeObject<GetWooOrderResponse>(content);
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = "Get Woo Order SUCCESS";
                        apiResult.Data = wooOrderResponse;
                    }
                    else
                    {
                        apiResult.Code = ResultCode.BadRequest;
                        apiResult.Message = "Get Woo Order FAILED";
                    }
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
