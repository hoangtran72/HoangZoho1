using Google.Rpc;
using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM;
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
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HoangZoho1.Services.RestaurantEquipmentOnline
{

    public class ReoCrmService : IReoCrmService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public ReoCrmService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        #region Leads

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateLead(UpsertRequest<LeadForCreation> leadRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);
                string endpoint = $"{REOConstants.ZohoCRM_EndpointV2}/Leads";
                string requestBody = JsonConvert.SerializeObject(leadRequest);
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

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpsertResponse<UpsertDetail>>(responseData);
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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateLead(string leadId, UpsertRequest<LeadForUpdation> leadRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);
                string endpoint = $"{REOConstants.ZohoCRM_EndpointV2}/Leads/{leadId}";
                string requestBody = JsonConvert.SerializeObject(leadRequest);
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

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<UpsertResponse<UpsertDetail>>(responseData);
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
        public async Task<ApiResultDto<SearchLeadsResponse>> SearchLeadsByEmail(string email)
        {
            var apiResult = new ApiResultDto<SearchLeadsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV2}/Leads/search?email={HttpUtility.UrlEncode(email)}";

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
                    var responseObj = JsonConvert.DeserializeObject<SearchLeadsResponse>(responseData);
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

        public async Task<ApiResultDto<QueryResponse<LeadQueryModel>>> QueryLeads(string query)
        {
            var apiResult = new ApiResultDto<QueryResponse<LeadQueryModel>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV2}/coql";
                string requestBody = JsonConvert.SerializeObject(new
                {
                    select_query = query
                });

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8)
                };
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
                    var responseObj = JsonConvert.DeserializeObject<QueryResponse<LeadQueryModel>>(responseData);
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

        #endregion

        #region Contacts

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
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV6}/Contacts/{contactId}";

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
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV2}/Contacts/search?email={HttpUtility.UrlEncode(email)}";

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

        #endregion

        #region Products

        public async Task<ApiResultDto<GetProductByIdResponse>> GetProductById(string productId)
        {

            var apiResult = new ApiResultDto<GetProductByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, 
                        CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV6}/Products/{productId}";

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
                    var responseObj = JsonConvert.DeserializeObject<GetProductByIdResponse>(responseData);
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

        public async Task<ApiResultDto<QueryResponse<ProductQueryModel>>> QueryProducts(string query)
        {
            var apiResult = new ApiResultDto<QueryResponse<ProductQueryModel>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV6}/coql";
                string requestBody = JsonConvert.SerializeObject(new
                {
                    select_query = query
                });

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8)
                };
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
                    var responseObj = JsonConvert.DeserializeObject<QueryResponse<ProductQueryModel>>(responseData);
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

        #endregion

        #region Quotes

        public async Task<ApiResultDto<GetQuoteByIdResponse>> GetQuoteById(string quoteId)
        {
            var apiResult = new ApiResultDto<GetQuoteByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, 
                        CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV6}/Quotes/{quoteId}";

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
                    var responseObj = JsonConvert.DeserializeObject<GetQuoteByIdResponse>(responseData);
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

        public async Task<ApiResultDto<QueryResponse<QuoteQueryModel>>> QueryQuotes(string query)
        {
            var apiResult = new ApiResultDto<QueryResponse<QuoteQueryModel>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV6}/coql";
                string requestBody = JsonConvert.SerializeObject(new
                {
                    select_query = query
                });

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8)
                };
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
                    var responseObj = JsonConvert.DeserializeObject<QueryResponse<QuoteQueryModel>>(responseData);
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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> 
            CreateQuote(UpsertRequest<QuoteForCreation> quoteRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);
                string endpoint = $"{REOConstants.ZohoCRM_EndpointV6}/Quotes";
                string requestBody = JsonConvert.SerializeObject(quoteRequest);
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

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> 
            UpdateQuote(string quoteId, UpsertRequest<UpdateQuoteRequest> quoteRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);
                string endpoint = $"{REOConstants.ZohoCRM_EndpointV6}/Quotes/{quoteId}";
                string requestBody = JsonConvert.SerializeObject(quoteRequest);
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

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
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

        #region Deals

        public async Task<ApiResultDto<GetDealByIdResponse>> GetDealById(string dealId)
        {

            var apiResult = new ApiResultDto<GetDealByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV6}/Deals/{dealId}";

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
                    var responseObj = JsonConvert.DeserializeObject<GetDealByIdResponse>(responseData);
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

        #endregion

        #region Users

        public async Task<ApiResultDto<GetUsersResponse>> GetUserByTypes(string userType)
        {
            var apiResult = new ApiResultDto<GetUsersResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV2}/users";
                if (!string.IsNullOrEmpty(userType))
                {
                    endpoint = $"{endpoint}?type={userType}";
                }

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
                    var responseObj = JsonConvert.DeserializeObject<GetUsersResponse>(responseData);
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

        public async Task<ApiResultDto<GetUsersResponse>> SearchUserByEmail(string email)
        {
            var apiResult = new ApiResultDto<GetUsersResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV2}/users/search?email={email}";

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
                    var responseObj = JsonConvert.DeserializeObject<GetUsersResponse>(responseData);
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


        #endregion

        #region JustCall Logs

        public async Task<ApiResultDto<SearchJustCallLogsResponse>> SearchJustCallLogs(string criteria)
        {
            var apiResult = new ApiResultDto<SearchJustCallLogsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV2}/JustCall_Logs/search?criteria=({criteria})";

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
                    var responseObj = JsonConvert.DeserializeObject<SearchJustCallLogsResponse>(responseData);
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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateJustCallLog(UpsertRequest<JustCallLogForCreation> justcallRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);
                string endpoint = $"{REOConstants.ZohoCRM_EndpointV2}/JustCall_Logs";
                string requestBody = JsonConvert.SerializeObject(justcallRequest);
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

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
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

        #region TNZ Logs

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateTNZLog(UpsertRequest<TNZLogForCreation> tnzRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);
                string endpoint = $"{REOConstants.ZohoCRM_EndpointV2}/TNZ_Logs";
                string requestBody = JsonConvert.SerializeObject(tnzRequest);
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

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
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

        #region Notes

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateNote(UpsertRequest<NoteForCreation> noteRequest)
        {

            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                var noteForCreation = noteRequest.data.FirstOrDefault();
                string parentModule = noteForCreation.se_module;
                string parentId = noteForCreation.Parent_Id;

                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);
                string endpoint = $"{REOConstants.ZohoCRM_EndpointV2}/{parentModule}/{parentId}/Notes";
                string requestBody = JsonConvert.SerializeObject(noteRequest);
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

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
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

        public async Task<ApiResultDto<GetNotesResponse>> GetNotes(string recordModule, string recordId)
        {
            var apiResult = new ApiResultDto<GetNotesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);
                string endpoint = $"{REOConstants.ZohoCRM_EndpointV2}/{recordModule}/{recordId}/Notes";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
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
                    var responseObj = JsonConvert.DeserializeObject<GetNotesResponse>(responseData);
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

        #region Timelines

        public async Task<ApiResultDto<GetTimelineResponse>> GetTimeline(string recordModule, string recordId)
        {
            var apiResult = new ApiResultDto<GetTimelineResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);
                string endpoint = $"{REOConstants.ZohoCRM_EndpointV6}/{recordModule}/{recordId}/__timeline";
                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
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
                    var responseObj = JsonConvert.DeserializeObject<GetTimelineResponse>(responseData);
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

        #region Blueprint

        public async Task<ApiResultDto<UpdateBlueprintResponse>> UpdateLeadBlueprint
            (string recordModule, string recordId, UpdateBlueprintRequest<LeadConnectedData> updateBlueprintRequest)
        {
            var apiResult = new ApiResultDto<UpdateBlueprintResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                    .GetAccessToken(REOConstants.RestaurantEquipmentOnline, CommonConstants.ZohoCRM);

                string endpoint = $"{REOConstants.ZohoCRM_EndpointV6}/{recordModule}/{recordId}/actions/blueprint";
                
                string requestBody = JsonConvert.SerializeObject(updateBlueprintRequest);

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
                    var responseObj = JsonConvert.DeserializeObject<UpdateBlueprintResponse>(responseData);
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

    }

}
