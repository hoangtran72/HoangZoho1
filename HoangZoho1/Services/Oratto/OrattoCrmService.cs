using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Oratto.ZohoCRM;
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

namespace HoangZoho1.Services.Oratto
{
    public class OrattoCrmService : IOrattoCrmService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public OrattoCrmService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        #region Leads 

        public async Task<ApiResultDto<GetLeadRelatedSolicitorsResponse>> GetLeadRelatedSolicitors(string leadId)
        {
            var apiResult = new ApiResultDto<GetLeadRelatedSolicitorsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(OrattoConstants.Oratto, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OrattoConstants.ZohoCRM_EndpointV2}/Leads/{leadId}/Solicitors1";

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
                    var responseObj = JsonConvert.DeserializeObject<GetLeadRelatedSolicitorsResponse>(responseData);

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

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateLeadSolicitorLinking
            (UpsertRequest<LeadSolicitorLinkingForCreation> linkingRequest)
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
                        .GetAccessToken(OrattoConstants.Oratto, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OrattoConstants.ZohoCRM_EndpointV3}/Matters";
                string requestBody = JsonConvert.SerializeObject(linkingRequest);
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

        #region Timeline

        public async Task<ApiResultDto<GetModuleTimelineResponse>> GetModuleTimeline(string moduleName, string moduleId)
        {
            var apiResult = new ApiResultDto<GetModuleTimelineResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(OrattoConstants.Oratto, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OrattoConstants.ZohoCRM_EndpointV5}/{moduleName}/{moduleId}/__timeline";

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
                    var responseObj = JsonConvert.DeserializeObject<GetModuleTimelineResponse>(responseData);

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
                return apiResult;
            }
        }

        #endregion

        #region Solicitor

        public async Task<ApiResultDto<QuerySolicitorsResponse>> QuerySolicitors(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<QuerySolicitorsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OrattoConstants.Oratto, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OrattoConstants.ZohoCRM_EndpointV3}/coql";

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
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<QuerySolicitorsResponse>(responseData);
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

        public async Task<ApiResultDto<GetSolicitorByIdResponse>> GetSolicitorById(string solicitorId)
        {
            var apiResult = new ApiResultDto<GetSolicitorByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(OrattoConstants.Oratto, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OrattoConstants.ZohoCRM_EndpointV3}/Contacts/{solicitorId}";

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
                    var responseObj = JsonConvert.DeserializeObject<GetSolicitorByIdResponse>(responseData);

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

        #region Matter

        public async Task<ApiResultDto<GetMatterByIdResponse>> GetMatterById(string matterId)
        {
            var apiResult = new ApiResultDto<GetMatterByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(OrattoConstants.Oratto, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OrattoConstants.ZohoCRM_EndpointV3}/Matters1/{matterId}";

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
                    var responseObj = JsonConvert.DeserializeObject<GetMatterByIdResponse>(responseData);

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
                return apiResult;
            }
        }

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateMatter(string matterId, UpsertRequest<MatterForUpdation> updateMatterRequest)
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
                        .GetAccessToken(OrattoConstants.Oratto, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OrattoConstants.ZohoCRM_EndpointV3}/Matters1/{matterId}";
                string requestBody = JsonConvert.SerializeObject(updateMatterRequest);
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

        public async Task<ApiResultDto<ZohoDeleteResponse>> DeleteLeadSolicitorLinking(string linkingId)
        {
            var apiResult = new ApiResultDto<ZohoDeleteResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = string.Empty;

                accessToken = await _zohoAuthService
                        .GetAccessToken(OrattoConstants.Oratto, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OrattoConstants.ZohoCRM_EndpointV3}/Matters/{linkingId}";
                var request = new HttpRequestMessage(
                           HttpMethod.Delete,
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
                    var responseObj = JsonConvert.DeserializeObject<ZohoDeleteResponse>(responseData);
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

        #region Queued Attachments

        public async Task<ApiResultDto<UpsertResponse<UpsertDetail>>> 
            UpdateQueuedAttachments(string attachmentId, 
            UpsertRequest<AttachmentForUpdation> upsertRequest)
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
                        .GetAccessToken(OrattoConstants.Oratto, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OrattoConstants.ZohoCRM_EndpointV3}/Queued_Attachments/{attachmentId}";
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

        public async Task<ApiResultDto<QueryAttachmentsResponse>> 
            QueryAttachments(ZohoCoqlRequest coqlRequest)
        {
            var apiResult = new ApiResultDto<QueryAttachmentsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OrattoConstants.Oratto, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OrattoConstants.ZohoCRM_EndpointV3}/coql";

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
                    var responseObj = JsonConvert.DeserializeObject<QueryAttachmentsResponse>(responseData);
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
            UpdatedQueuedEmails(string emailId, 
                UpsertRequest<EmailForUpdation> upsertRequest)
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
                        .GetAccessToken(OrattoConstants.Oratto, CommonConstants.ZohoCRM);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OrattoConstants.ZohoCRM_EndpointV3}/Queued_Emails/{emailId}";
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

    }
}
