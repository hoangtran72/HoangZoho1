using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.ZoRaw.Freightcom;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HoangZoho1.Services.ZoRaw
{
    public class ZoRawFreightcomService : IZoRawFreightcomService
    {

        public async Task<ApiResultDto<CalculateFreightClassResponse>> CalculateFreightClass
            (CalculateFreightClassRequest calculateFreightClassRequest)
        {

            var apiResult = new ApiResultDto<CalculateFreightClassResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.FCFC_400
            };

            try
            {

                string endpoint = $"{ZoRawConstants.Freightcom_Endpoint}/freight-class/calculate";

                string requestBody = JsonConvert.SerializeObject(calculateFreightClassRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"{ZoRawConstants.Freightcom_Token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<CalculateFreightClassResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.FCFC_200;
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

        public async Task<ApiResultDto<RequestRateEstimateResponse>> RequestRateEstimate
            (RequestRateEstimateRequest requestRateEstimateRequest)
        {

            var apiResult = new ApiResultDto<RequestRateEstimateResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.FFRE_400
            };

            try
            {

                string endpoint = $"{ZoRawConstants.Freightcom_Endpoint}/rate";

                string requestBody = JsonConvert.SerializeObject(requestRateEstimateRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"{ZoRawConstants.Freightcom_Token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                //response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK 
                    || response.StatusCode == HttpStatusCode.Accepted)
                {
                    var responseObj = JsonConvert.DeserializeObject<RequestRateEstimateResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.FRRE_200;
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

        public async Task<ApiResultDto<RetrieveARateResponse>> RetrieveARate(string rateId)
        {

            var apiResult = new ApiResultDto<RetrieveARateResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.FFRE_400
            };

            try
            {

                string endpoint = $"{ZoRawConstants.Freightcom_Endpoint}/rate/{rateId}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"{ZoRawConstants.Freightcom_Token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Accepted)
                {
                    var responseObj = JsonConvert.DeserializeObject<RetrieveARateResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.FRRE_200;
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

        public async Task<ApiResultDto<CreateFreightcomShipmentResponse>> 
            CreateFreightcomShipment(CreateFreightcomShipmentRequest createFreightcomShipmentRequest)
        {

            var apiResult = new ApiResultDto<CreateFreightcomShipmentResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.FRCS_400
            };

            try
            {

                string endpoint = $"{ZoRawConstants.Freightcom_Endpoint}/shipment";

                string requestBody = JsonConvert.SerializeObject(createFreightcomShipmentRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"{ZoRawConstants.Freightcom_Token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                //response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Accepted)
                {
                    var responseObj = JsonConvert.DeserializeObject<CreateFreightcomShipmentResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.FRCS_200;
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

        public async Task<ApiResultDto<RetrieveShipmentDetailsResponse>> 
            RetrieveShipmentDetails(string shipmentId)
        {
            var apiResult = new ApiResultDto<RetrieveShipmentDetailsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.FRSD_400
            };

            try
            {

                string endpoint = $"{ZoRawConstants.Freightcom_Endpoint}/shipment/{shipmentId}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"{ZoRawConstants.Freightcom_Token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Accepted)
                {
                    var responseObj = JsonConvert.DeserializeObject<RetrieveShipmentDetailsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.FRSD_200;
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

        public async Task<ApiResultDto<List<PaymentMethod>>> GetPaymentMethods()
        {

            var apiResult = new ApiResultDto<List<PaymentMethod>>()
            {
                Code = ResultCode.BadRequest,
                Message = ZoRawConstants.FRSD_400
            };

            try
            {

                string endpoint = $"{ZoRawConstants.Freightcom_Endpoint}/finance/payment-methods";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"{ZoRawConstants.Freightcom_Token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Accepted)
                {
                    var responseObj = JsonConvert.DeserializeObject<List<PaymentMethod>>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZoRawConstants.FRSD_200;
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
