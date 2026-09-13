using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GetUnik.ZohoBooks;
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

namespace HoangZoho1.Services.GetUnik
{

    public class GetunikBooksService : IGetunikBooksService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public GetunikBooksService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<CreateInvoiceResponse>> 
            CreateInvoice(string createInvoiceRequest)
        {
            var apiResult = new ApiResultDto<CreateInvoiceResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(GetunikConstants.GetUnik, CommonConstants.ZohoBooks);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{GetunikConstants.ZohoBooks_EndpointV3}/invoices?" +
                    $"organization_id={GetunikConstants.ZohoBooks_LiveOrganizationId}";
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(createInvoiceRequest, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<CreateInvoiceResponse>(responseData);
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
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<SubmitInvoiceResponse>> SubmitInvoice(string invoiceId)
        {
            var apiResult = new ApiResultDto<SubmitInvoiceResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = string.Empty;
                accessToken = await _zohoAuthService
                        .GetAccessToken(GetunikConstants.GetUnik, CommonConstants.ZohoBooks);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{GetunikConstants.ZohoBooks_EndpointV3}/invoices/" +
                    $"{invoiceId}/submit?" +
                    $"organization_id={GetunikConstants.ZohoBooks_LiveOrganizationId}";
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<SubmitInvoiceResponse>(responseData);
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
            catch (Exception)
            {
                return apiResult;
            }
        }

    }

}
