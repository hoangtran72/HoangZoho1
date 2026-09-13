using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.Sakari;
using HoangZoho1.Services.SakariAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HoangZoho1.Services.OneCorp
{

    public class OneCorpSakariService : IOneCorpSakariService
    {

        private readonly ISakariAuthService _sakariAuthService;

        public OneCorpSakariService(ISakariAuthService sakariAuthService)
        {
            _sakariAuthService = sakariAuthService;
        }

        public async Task<ApiResultDto<SendSakariSMSResponse>> SendSakariSMS(SendSakariSMSRequest smsRequest)
        {

            var apiResult = new ApiResultDto<SendSakariSMSResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                // Get Access Token
                string clientId = OneCorpConstants.Sakari_ClientId;
                string clientSecret = OneCorpConstants.Sakari_ClientSecret;
                string token = await _sakariAuthService.GetSakariToken(clientId, clientSecret);

                if (string.IsNullOrEmpty(token))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    return apiResult;
                }

                if (string.IsNullOrEmpty(token))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.Sakari_EndpointV1}/accounts/{OneCorpConstants.Sakari_AccountId}/messages";

                string requestBody = JsonConvert.SerializeObject(smsRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SendSakariSMSResponse>(responseData);

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
            catch (Exception)
            {
                return apiResult;
            }

        }

        public async Task<ApiResultDto<GetPhoneGroupsResponse>> GetSakariPhoneGroups()
        {

            var apiResult = new ApiResultDto<GetPhoneGroupsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                // Get Access Token
                string clientId = OneCorpConstants.Sakari_ClientId;
                string clientSecret = OneCorpConstants.Sakari_ClientSecret;
                string token = await _sakariAuthService.GetSakariToken(clientId, clientSecret);

                if (string.IsNullOrEmpty(token))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.Sakari_EndpointV1}/accounts/{OneCorpConstants.Sakari_AccountId}/groups?admin=1&limit=500";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetPhoneGroupsResponse>(responseData);

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

        public async Task<ApiResultDto<FetchContactsResponse>> FetchContacts(FetchContactsParameters parameters)
        {
            var apiResult = new ApiResultDto<FetchContactsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                // Get Access Token
                string clientId = OneCorpConstants.Sakari_ClientId;
                string clientSecret = OneCorpConstants.Sakari_ClientSecret;
                string token = await _sakariAuthService.GetSakariToken(clientId, clientSecret);

                if (string.IsNullOrEmpty(token))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    return apiResult;
                }

                // Handle Endpoint
                string endpoint = $"{OneCorpConstants.Sakari_EndpointV1}/accounts/{OneCorpConstants.Sakari_AccountId}/contacts?";
                bool hasParameter = false;
                var offset = parameters.offset;
                if (offset.HasValue)
                {
                    if (hasParameter)
                    {
                        endpoint += "&";
                    }
                    endpoint += $"offset={offset.Value}";
                    hasParameter = true;
                }
                var limit = parameters.limit;
                if (limit.HasValue)
                {
                    if (hasParameter)
                    {
                        endpoint += "&";
                    }
                    endpoint += $"limit={limit.Value}";
                    hasParameter = true;
                }
                string firstName = parameters.firstName;
                if (!string.IsNullOrEmpty(firstName))
                {
                    if (hasParameter)
                    {
                        endpoint += "&";
                    }
                    endpoint += $"firstName={HttpUtility.UrlEncode(firstName)}";
                    hasParameter = true;
                }
                string lastName = parameters.lastName;
                if (!string.IsNullOrEmpty(lastName))
                {
                    if (hasParameter)
                    {
                        endpoint += "&";
                    }
                    endpoint += $"lastName={HttpUtility.UrlEncode(lastName)}";
                    hasParameter = true;
                }
                string mobile = parameters.mobile;
                if (!string.IsNullOrEmpty(mobile))
                {
                    if (hasParameter)
                    {
                        endpoint += "&";
                    }
                    endpoint += $"mobile={HttpUtility.UrlEncode(mobile)}";
                }
                string email = parameters.email;
                if (!string.IsNullOrEmpty(email))
                {
                    if (hasParameter)
                    {
                        endpoint += "&";
                    }
                    endpoint = $"email={HttpUtility.UrlEncode(email)}";
                }

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<FetchContactsResponse>(responseData);

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

        public async Task<ApiResultDto<UpsertContactResponse>> CreateContact(UpsertContactRequest createContactRequest)
        {
            var apiResult = new ApiResultDto<UpsertContactResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                // Get Access Token
                string clientId = OneCorpConstants.Sakari_ClientId;
                string clientSecret = OneCorpConstants.Sakari_ClientSecret;
                string token = await _sakariAuthService.GetSakariToken(clientId, clientSecret);

                if (string.IsNullOrEmpty(token))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    return apiResult;
                }

                if (string.IsNullOrEmpty(token))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.Sakari_EndpointV1}/accounts/{OneCorpConstants.Sakari_AccountId}/contacts";

                string requestBody = JsonConvert.SerializeObject(createContactRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK || 
                    response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpsertContactResponse>(responseData);

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
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<UpsertContactResponse>> UpdateContact(string contactId, UpsertContactRequest updateContactRequest)
        {
            var apiResult = new ApiResultDto<UpsertContactResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                // Get Access Token
                string clientId = OneCorpConstants.Sakari_ClientId;
                string clientSecret = OneCorpConstants.Sakari_ClientSecret;
                string token = await _sakariAuthService.GetSakariToken(clientId, clientSecret);

                if (string.IsNullOrEmpty(token))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    return apiResult;
                }

                if (string.IsNullOrEmpty(token))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.Sakari_EndpointV1}/accounts/{OneCorpConstants.Sakari_AccountId}/contacts/{contactId}";

                string requestBody = JsonConvert.SerializeObject(updateContactRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpsertContactResponse>(responseData);

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
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<GetSakariMessagesResponse>> GetSakariMessages(int offset = 0)
        {
            var apiResult = new ApiResultDto<GetSakariMessagesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                // Get Access Token
                string clientId = OneCorpConstants.Sakari_ClientId;
                string clientSecret = OneCorpConstants.Sakari_ClientSecret;
                string token = await _sakariAuthService.GetSakariToken(clientId, clientSecret);

                if (string.IsNullOrEmpty(token))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.Sakari_EndpointV1}/accounts/{OneCorpConstants.Sakari_AccountId}/messages?offset={offset}&limit=100";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetSakariMessagesResponse>(responseData);

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
