using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.RestaurantEquipmentOnline.TNZ;
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

    public class ReoTnzService : IReoTnzService
    {

        public async Task<ApiResultDto<SendMessageResponse>> SendSMS(string token, 
            SendSMSRequest sendSMSRequest)
        {
            var apiResult = new ApiResultDto<SendMessageResponse>()
            {
                Code = ResultCode.OK,
                Message = REOConstants.STS_400
            };

            try
            {

                string endpoint = $"{REOConstants.TNZ_EndpointV203}/send/sms";
                string requestBody = JsonConvert.SerializeObject(sendSMSRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {token}");
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
                    apiResult.Message = REOConstants.STS_200;
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

    }

}
