using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.ZohoCRM;
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

namespace HoangZoho1.Services.OneCorp
{
    public class OneCorpCrmService : IOneCorpCrmService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public OneCorpCrmService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

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
                    .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/Contacts/{contactId}";

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

        public async Task<ApiResultDto<SearchContactsByPhoneResponse>> SearchContactsByPhone(string phoneNumber)
        {
            var apiResult = new ApiResultDto<SearchContactsByPhoneResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }
                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/Contacts/search?phone={HttpUtility.UrlEncode(phoneNumber)}";

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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateContact(string contactId, UpsertRequest<ContactForUpdation> upsertRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/Contacts/{contactId}";
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
            catch (Exception)
            {
                return apiResult;
            }
        }

        #endregion

        #region Leads

        public async Task<ApiResultDto<GetLeadByIdResponse>> GetLeadById(string leadId)
        {
            var apiResult = new ApiResultDto<GetLeadByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/Leads/{leadId}";

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
                    var responseObj = JsonConvert.DeserializeObject<GetLeadByIdResponse>(responseData);

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

        public async Task<ApiResultDto<GetLeadStatusHistoriesResponse>> GetRelatedStatusHistories(string leadId)
        {
            var apiResult = new ApiResultDto<GetLeadStatusHistoriesResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                    .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV2}/Leads/{leadId}/Lead_Status_History";

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
                    var responseObj = JsonConvert.DeserializeObject<GetLeadStatusHistoriesResponse>(responseData);

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
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

        public async Task<ApiResultDto<SearchLeadsByPhoneResponse>> SearchLeadsByPhone(string phoneNumber)
        {
            var apiResult = new ApiResultDto<SearchLeadsByPhoneResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }
                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/Leads/search?phone={HttpUtility.UrlEncode(phoneNumber)}";

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
                    var responseObj = JsonConvert.DeserializeObject<SearchLeadsByPhoneResponse>(responseData);

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
            UpdateLead(string leadId, UpsertRequest<LeadForUpdation> upsertRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/Leads/{leadId}";
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
            catch (Exception)
            {
                return apiResult;
            }
        }

        #endregion

        #region Deals

        public async Task<ApiResultDto<QueryDealsResponse>> QueryDeals(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<QueryDealsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/coql";

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
                    var responseObj = JsonConvert.DeserializeObject<QueryDealsResponse>(responseData);
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

        #endregion

        #region ScheduleOnce Bookings

        public async Task<ApiResultDto<SearchBookingResponse>> 
            SearchScheduleOnceBookings(string criteria)
        {

            var apiResult = new ApiResultDto<SearchBookingResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                    .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV2}/ScheduleOnce_Bookings/search?criteria={criteria}";

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
                    var responseObj = JsonConvert.DeserializeObject<SearchBookingResponse>(responseData);

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

        public async Task<ApiResultDto<QueryBookingsResponse>> QueryBookings(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<QueryBookingsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/coql";

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
                    var responseObj = JsonConvert.DeserializeObject<QueryBookingsResponse>(responseData);
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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> 
            CreateBooking(UpsertRequest<BookingForCreation> createBookingRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = string.Empty;
                
                accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV2}/ScheduleOnce_Bookings";
                string requestBody = JsonConvert.SerializeObject(createBookingRequest);
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
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateBooking
            (string bookingId, UpsertRequest<BookingForUpdation> updateBookingRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = string.Empty;
                
                accessToken = await _zohoAuthService
                    .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV2}/ScheduleOnce_Bookings/{bookingId}";

                string requestBody = JsonConvert.SerializeObject(updateBookingRequest);
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
            catch (Exception)
            {
                return apiResult;
            }
        }

        #endregion

        #region Calls

        public async Task<ApiResultDto<GetCallByIdResponse>> GetCallById(string callId)
        {

            var apiResult = new ApiResultDto<GetCallByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV2}/Calls/{callId}";

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
                    var responseObj = JsonConvert.DeserializeObject<GetCallByIdResponse>(responseData);

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

        #region Tasks

        public async Task<ApiResultDto<GetTaskByIdResponse>> GetTaskById(string taskId)
        {
            var apiResult = new ApiResultDto<GetTaskByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV2}/Tasks/{taskId}";

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
                    var responseObj = JsonConvert.DeserializeObject<GetTaskByIdResponse>(responseData);

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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateTask(string taskId, UpsertRequest<TaskForUpdation> upsertRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/Tasks/{taskId}";
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
            catch (Exception)
            {
                return apiResult;
            }
        }

        #endregion

        #region Sakari SMS Log

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateSakariSMSLog(UpsertRequest<SMSLogForUpsert> createSmsLogRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/Sakari_SMS_Logs";

                string requestBody = JsonConvert.SerializeObject(createSmsLogRequest);
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

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateSakariSMSLog(string logId, UpsertRequest<SMSLogForUpsert> createSmsLogRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/Sakari_SMS_Logs/{logId}";

                string requestBody = JsonConvert.SerializeObject(createSmsLogRequest);
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

                response.EnsureSuccessStatusCode();
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
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<QuerySakariSMSLogsResponse>> QuerySakariSMSLogs(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<QuerySakariSMSLogsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/coql";

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
                    var responseObj = JsonConvert.DeserializeObject<QuerySakariSMSLogsResponse>(responseData);
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

        #endregion

        #region Notes

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateNote(UpsertRequest<NoteForCreation> upsertRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                var noteForCreation = upsertRequest.data[0];
                string module = noteForCreation.se_module;
                string moduleId = noteForCreation.Parent_Id;

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/{module}/{moduleId}/Notes";

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

                response.EnsureSuccessStatusCode();
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
            catch (Exception)
            {
                return apiResult;
            }
        }

        #endregion

        #region Zoho Projects Comments

        public async Task<ApiResultDto<QueryZPCommentsResponse>> QueryZPComments(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<QueryZPCommentsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV5}/coql";

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
                    var responseObj = JsonConvert.DeserializeObject<QueryZPCommentsResponse>(responseData);
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

        public async Task<ApiResultDto<QueryZPTasksResponse>> QueryZPTasks(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<QueryZPTasksResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV5}/coql";

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
                    var responseObj = JsonConvert.DeserializeObject<QueryZPTasksResponse>(responseData);
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

        public async Task<ApiResultDto<GetZPCommentByIdResponse>> GetZPCommentById(string commentId)
        {
            var apiResult = new ApiResultDto<GetZPCommentByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/ZP_Comments/{commentId}";

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
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetZPCommentByIdResponse>(responseData);
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

        #endregion

        #region Sakari Users

        public async Task<ApiResultDto<GetSakariUserByIdResponse>> GetSakariUserById(string sakariUserId)
        {
            var apiResult = new ApiResultDto<GetSakariUserByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/Sakari_Users/{sakariUserId}";

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
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetSakariUserByIdResponse>(responseData);
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

        public async Task<ApiResultDto<QuerySakariUsersResponse>> QuerySakariUsers(ZohoCoqlRequest coqlRequest)
        {

            var apiResult = new ApiResultDto<QuerySakariUsersResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/coql";

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
                    var responseObj = JsonConvert.DeserializeObject<QuerySakariUsersResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.NoContent;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = null;
                }

                return apiResult;
            }
            catch (Exception)
            {
                return apiResult;
            }

        }

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateSakariUser(UpsertRequest<SakariUserForUpsert> createUserRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/Sakari_Users";
                string requestBody = JsonConvert.SerializeObject(createUserRequest);
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
            catch (Exception)
            {
                return apiResult;
            }
        }

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateSakariUser
            (string sakariUserId, UpsertRequest<SakariUserForUpsert> updateUserRequest)
        {
            var apiResult = new ApiResultDto<UpsertResponse<UpsertDetail>>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/Sakari_Users/{sakariUserId}";
                string requestBody = JsonConvert.SerializeObject(updateUserRequest);
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
            catch (Exception)
            {
                return apiResult;
            }
        }

        #endregion

        #region Users

        public async Task<ApiResultDto<GetUserByIdResponse>> GetUserById(string userId)
        {
            var apiResult = new ApiResultDto<GetUserByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/users/{userId}";

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
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<GetUserByIdResponse>(responseData);
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

        public async Task<ApiResultDto<QueryUsersResponse>> QueryUsers(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<QueryUsersResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV3}/coql";

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
                    var responseObj = JsonConvert.DeserializeObject<QueryUsersResponse>(responseData);
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

        #endregion

        #region Blueprint

        public async Task<ApiResultDto<UpdateBlueprintResponse>> UpdateBlueprint
            (string recordModule, string recordId, UpdateBlueprintRequest blueprintRequest)
        {

            var apiResult = new ApiResultDto<UpdateBlueprintResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoCRM_EndpointV2}/" +
                    $"{recordModule}/{recordId}/actions/blueprint";

                string requestBody = JsonConvert.SerializeObject(blueprintRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Put,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", 
                    $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();
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
