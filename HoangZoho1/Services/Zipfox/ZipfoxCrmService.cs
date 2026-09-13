using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Zipfox.ZohoCRM;
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

    public class ZipfoxCrmService : IZipfoxCrmService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public ZipfoxCrmService(IZohoAuthService zohoAuthService)
        {

            _zohoAuthService = zohoAuthService;

        }

        public async Task<ApiResultDto<GetAccountRelatedContactsResponse>> GetAccountRelatedContacts(string accountId)
        {

            var apiResult = new ApiResultDto<GetAccountRelatedContactsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.GARC_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(ZipfoxConstants.Zipfox, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }
                string endpoint = $"{ZipfoxConstants.ZohoCRM_EndpointV2}/Accounts/{accountId}/Contacts";

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
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetAccountRelatedContactsResponse>(responseData);

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZipfoxConstants.GARC_200;
                    apiResult.Data = responseObj;
                }
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZipfoxConstants.GARC_400;
                }

                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }

        }

        #region Contacts

        public async Task<ApiResultDto<SearchContactsByPhoneResponse>> SearchContactsByPhone(string phone)
        {

            var apiResult = new ApiResultDto<SearchContactsByPhoneResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.GARC_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(ZipfoxConstants.Zipfox, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZipfoxConstants.ZohoCRM_EndpointV3}/Contacts/search?phone={phone}";

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
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SearchContactsByPhoneResponse>(responseData);

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZipfoxConstants.SCP_200;
                    apiResult.Data = responseObj;
                }
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = ZipfoxConstants.SCP_204;
                }

                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<QueryContactsResponse>> QueryContacts(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<QueryContactsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.QC_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(ZipfoxConstants.Zipfox, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZipfoxConstants.ZohoCRM_EndpointV3}/coql";

                string requestBody = JsonConvert.SerializeObject(coqlRequest);
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

               //  response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<QueryContactsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZipfoxConstants.QC_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateContact(UpsertRequest<ContactForCreation> upsertRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.OK,
                Message = ZipfoxConstants.CCC_400
            };

            try
            {
                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZipfoxConstants.Zipfox, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZipfoxConstants.ZohoCRM_EndpointV3}/Contacts";
                string requestBody = JsonConvert.SerializeObject(upsertRequest);
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

                if (response.StatusCode == HttpStatusCode.OK 
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpsertResponse<UpsertDetail>>(responseData);
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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateContact(string contactId, UpsertRequest<ContactForUpdation> upsertRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.OK,
                Message = ZipfoxConstants.UCC_400
            };

            try
            {
                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZipfoxConstants.Zipfox, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZipfoxConstants.ZohoCRM_EndpointV3}/Contacts/{contactId}";
                string requestBody = JsonConvert.SerializeObject(upsertRequest);
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
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpsertResponse<UpsertDetail>>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZipfoxConstants.UCC_200;
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

        #region WhatsApp Log

        public async Task<ApiResultDto<QueryWhatsAppLogsResponse>> QueryWhatsAppLogs(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<QueryWhatsAppLogsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.QWL_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(ZipfoxConstants.Zipfox, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZipfoxConstants.ZohoCRM_EndpointV3}/coql";

                string requestBody = JsonConvert.SerializeObject(coqlRequest);
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

                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<QueryWhatsAppLogsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZipfoxConstants.QWL_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateWhatsAppLog(UpsertRequest<WhatsAppLogForCreation> upsertRequest)
        {

            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>() {
                Code = ResultCode.OK,
                Message = ZipfoxConstants.CWL_400
            };

            try
            {
                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZipfoxConstants.Zipfox, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZipfoxConstants.ZohoCRM_EndpointV3}/WhatsApp_Logs";
                string requestBody = JsonConvert.SerializeObject(upsertRequest);
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

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpsertResponse<UpsertDetail>>(responseData);
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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateWhatsAppLog(string logId, UpsertRequest<WhatsAppLogForUpdation> upsertRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.OK,
                Message = ZipfoxConstants.CWL_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZipfoxConstants.Zipfox, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZipfoxConstants.ZohoCRM_EndpointV3}/WhatsApp_Logs/{logId}";
                string requestBody = JsonConvert.SerializeObject(upsertRequest);
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
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpsertResponse<UpsertDetail>>(responseData);
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

        #region RFQ

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateRFQ
            (string rfqId, UpsertRequest<RFQForUpdation> upsertRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.OK,
                Message = ZipfoxConstants.CWL_400
            };

            try
            {

                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(ZipfoxConstants.Zipfox, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZipfoxConstants.ZohoCRM_EndpointV3}/RFQs/{rfqId}";
                string requestBody = JsonConvert.SerializeObject(upsertRequest);
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
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpsertResponse<UpsertDetail>>(responseData);
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

        public async Task<ApiResultDto<QueryRFQsResponse>> QueryRFQs(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<QueryRFQsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = ZipfoxConstants.QRFQs_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(ZipfoxConstants.Zipfox, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{ZipfoxConstants.ZohoCRM_EndpointV3}/coql";

                string requestBody = JsonConvert.SerializeObject(coqlRequest);
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

                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<QueryRFQsResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = ZipfoxConstants.QRFQs_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }
        }

        #endregion

    }

}
