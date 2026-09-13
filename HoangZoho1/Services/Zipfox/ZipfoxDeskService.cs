using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Zipfox.ZohoDesk;
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

namespace HoangZoho1.Services.Zipfox
{

    public class ZipfoxDeskService : IZipfoxDeskService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public ZipfoxDeskService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<CreateTicketResponse>> CreateTicket(CreateTicketRequest createTicketRequest)
        {
            var apiResult = new ApiResultDto<CreateTicketResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.CDT_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZipfoxConstants.Zipfox, CommonConstants.ZohoDesk);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZipfoxConstants.ZohoDesk_EndpointV1}/tickets";
                string requestBody = JsonConvert.SerializeObject(createTicketRequest);
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
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<CreateTicketResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZipfoxConstants.CCC_200;
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
