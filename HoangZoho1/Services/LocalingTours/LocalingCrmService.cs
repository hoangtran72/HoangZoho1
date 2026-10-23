using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Lumicare.ZohoCRM;
using HoangZoho1.Models.LocalingTours;
using HoangZoho1.Services.ZohoAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HoangZoho1.Services.LocalingTours
{

    public class LocalingCrmService : ILocalingCrmService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public LocalingCrmService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<DrChipsRecord>> GetDrChipsById(string recordId)
        {
            var result = new ApiResultDto<DrChipsRecord> { Code = ResultCode.BadRequest, Message = CommonConstants.MSG_400 };
            try
            {
                var token = await _zohoAuthService.GetAccessToken("LocalingTours", CommonConstants.ZohoCRM);
                if (string.IsNullOrWhiteSpace(token)) { result.Code = ResultCode.Unauthorize; result.Message = CommonConstants.MSG_401; return result; }
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {token}");
                using var response = await client.GetAsync($"https://www.zohoapis.com/crm/v7/DR_CHIPS/{Uri.EscapeDataString(recordId)}");
                var body = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(
                        $"Zoho CRM record lookup returned {(int)response.StatusCode} ({response.ReasonPhrase}): {body}");
                var wrapper = JsonConvert.DeserializeObject<DrChipsGetResponse>(body);
                result.Data = wrapper?.Data?.FirstOrDefault();
                result.Code = result.Data == null ? ResultCode.NoContent : ResultCode.OK;
                result.Message = result.Data == null ? "DR_CHIPS record was not found." : CommonConstants.MSG_200;
            }
            catch (Exception ex) { result.Message = ex.Message; }
            return result;
        }

        public async Task<ApiResultDto<string>> UpdateDrChipsPostTourImages(string recordId, List<string> imageUrls)
        {
            var result = new ApiResultDto<string> { Code = ResultCode.BadRequest, Message = CommonConstants.MSG_400 };
            try
            {
                var token = await _zohoAuthService.GetAccessToken("LocalingTours", CommonConstants.ZohoCRM);
                if (string.IsNullOrWhiteSpace(token)) { result.Code = ResultCode.Unauthorize; result.Message = CommonConstants.MSG_401; return result; }
                // Post_Tour_Image_URLs is a Zoho CRM textarea field. Store the
                // URL list as JSON text, matching the field's existing value format.
                var imageUrlsText = JsonConvert.SerializeObject(imageUrls);
                var payload = new { data = new[] { new { Post_Tour_Image_Uploaded = true, Post_Tour_Image_URLs = imageUrlsText } } };
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {token}");
                using var response = await client.PutAsync($"https://www.zohoapis.com/crm/v7/DR_CHIPS/{Uri.EscapeDataString(recordId)}",
                    new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));
                var body = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(
                        $"Zoho CRM record update returned {(int)response.StatusCode} ({response.ReasonPhrase}): {body}");
                result.Code = ResultCode.OK; result.Message = CommonConstants.MSG_200; result.Data = body;
            }
            catch (Exception ex) { result.Message = ex.Message; }
            return result;
        }

        private sealed class DrChipsGetResponse
        {
            [JsonProperty("data")] public List<DrChipsRecord> Data { get; set; }
        }

        public async Task<ApiResultDto<GetContactByIdResponse>> GetContactById(string contactId)
        {
            var apiResult = new ApiResultDto<GetContactByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(LumicareConstants.Lumicare, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{LumicareConstants.ZohoCRM_EndpointV3}/Contacts/{contactId}";

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
                    var responseObj = JsonConvert.DeserializeObject<GetContactByIdResponse>(responseData);

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

        public async Task<ApiResultDto<SearchContactsResponse>> SearchContactsByEmail(string email)
        {
            var apiResult = new ApiResultDto<SearchContactsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(LumicareConstants.Lumicare, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{LumicareConstants.ZohoCRM_EndpointV3}/Contacts/search?email={HttpUtility.UrlEncode(email)}";

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
                    var responseObj = JsonConvert.DeserializeObject<SearchContactsResponse>(responseData);

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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateContact(UpsertRequest<CreateContactRequest> productLogRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(LumicareConstants.Lumicare, CommonConstants.ZohoCRM);
                string endpoint = $"{LumicareConstants.ZohoCRM_EndpointV3}/Contacts";
                string requestBody = JsonConvert.SerializeObject(productLogRequest);
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

                if (response.StatusCode == HttpStatusCode.Created)
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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateContact(string contactId, UpsertRequest<UpdateContactRequest> productLogRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(LumicareConstants.Lumicare, CommonConstants.ZohoCRM);
                string endpoint = $"{LumicareConstants.ZohoCRM_EndpointV3}/Contacts/{contactId}";
                string requestBody = JsonConvert.SerializeObject(productLogRequest);
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

        public async Task<ApiResultDto<SearchDistributorsResponse>> SearchDistributorsByUsernameAndPassword(string username, string password)
        {
            var apiResult = new ApiResultDto<SearchDistributorsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(LumicareConstants.Lumicare, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string criteria = $"((Username:equals:{HttpUtility.UrlEncode(username)})and(Password:equals:{HttpUtility.UrlEncode(password)}))";
                string endpoint = $"{LumicareConstants.ZohoCRM_EndpointV3}/Distributors/search?criteria={criteria}";

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
                    var responseObj = JsonConvert.DeserializeObject<SearchDistributorsResponse>(responseData);

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = CommonConstants.MSG_204;
                }

                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<QueryLastestNewsResponse>> QueryLatestNews(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<QueryLastestNewsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(LumicareConstants.Lumicare, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{LumicareConstants.ZohoCRM_EndpointV3}/coql";
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
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<QueryLastestNewsResponse>(responseData);
                    if (responseObj.data != null)
                    {
                        var newsList = responseObj.data;
                        foreach (var news in newsList)
                        {
                            string newsDate = news.News_Date;
                            if (!string.IsNullOrEmpty(newsDate))
                            {
                                var newsDateTime = DateTime.ParseExact(newsDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                news.News_Date = newsDateTime.ToString("dd MMM yyyy");
                            }
                        }
                    }
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = CommonConstants.MSG_204;
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
