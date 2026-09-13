using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GetUnik.ZohoCRM;
using HoangZoho1.Models.ZoRaw.Stallion;
using HoangZoho1.Models.ZoRaw.ZohoInventory;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Services.ZoRaw
{
    public class ZoRawStallionService : IZoRawStallionService
    {

        public async Task<ApiResultDto<GetStallionRatesResponse>> GetRates
            (GetStallionRatesRequest getRatesRequest)
        {

            var apiResult = new ApiResultDto<GetStallionRatesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.SEGR_400
            };

            try
            {

                string endpoint = $"{ZoRawConstants.Stallion_Endpoint_V4}/rates";

                string requestBody = JsonConvert.SerializeObject(getRatesRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ZoRawConstants.Stallion_Token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetStallionRatesResponse>(responseData);
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

        public async Task<ApiResultDto<CreateStallionShipmentResponse>> CreateShipment
            (CreateStallionShipmentRequest createShipmentRequest)
        {

            var apiResult = new ApiResultDto<CreateStallionShipmentResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.SECS_400
            };

            try
            {

                string endpoint = $"{ZoRawConstants.Stallion_Endpoint_V4}/shipments";

                string requestBody = JsonConvert.SerializeObject(createShipmentRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ZoRawConstants.Stallion_Token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<CreateStallionShipmentResponse>(responseData);
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

        public async Task<ApiResultDto<TrackShipmentResponse>> TrackShipment(string trackingCode)
        {

            var apiResult = new ApiResultDto<TrackShipmentResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.SECS_400
            };

            try
            {

                string endpoint = $"{ZoRawConstants.Stallion_Endpoint_V4}/track?tracking_code={trackingCode}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ZoRawConstants.Stallion_Token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<TrackShipmentResponse>(responseData);
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

    }
}
