using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Getunik.ZohoProjects;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net;
using System;
using System.Threading.Tasks;
using HoangZoho1.Services.ZohoAuth;

namespace HoangZoho1.Services.Getunik
{

    public class GetunikProjectsService : IGetunikProjectsService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public GetunikProjectsService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<GetProjectByIdResponse>> GetProjectById(string projectId)
        {

            var apiResult = new ApiResultDto<GetProjectByIdResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            try
            {
                string accessToken = string.Empty;
                accessToken = await _zohoAuthService
                        .GetAccessToken(GetunikConstants.GetUnik, CommonConstants.ZohoProjects);
                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }
                string endpoint = $"{GetunikConstants.ZohoProjects_APIEndpoint_V3}/portal/{GetunikConstants.ZohoProjects_PortalId}/"
                    + $"projects/{projectId}";
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
                    var responseObj = JsonConvert.DeserializeObject<GetProjectByIdResponse>(responseData);
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

        public async Task<ApiResultDto<GetProjectTasklistsResponse>> GetProjectTasklists(string projectId)
        {
            var apiResult = new ApiResultDto<GetProjectTasklistsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            try
            {
                string accessToken = string.Empty;
                accessToken = await _zohoAuthService
                        .GetAccessToken(GetunikConstants.GetUnik, CommonConstants.ZohoProjects);
                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }
                string endpoint = $"{GetunikConstants.ZohoProjects_APIEndpoint_V3}/portal/{GetunikConstants.ZohoProjects_PortalId}/"
                    + $"projects/{projectId}/tasklists";
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
                    var responseObj = JsonConvert.DeserializeObject<GetProjectTasklistsResponse>(responseData);
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

        public async Task<ApiResultDto<GetProjectUsersResponse>> GetProjectUsers()
        {

            var apiResult = new ApiResultDto<GetProjectUsersResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = string.Empty;
                accessToken = await _zohoAuthService
                        .GetAccessToken(GetunikConstants.GetUnik, CommonConstants.ZohoProjects);
                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }
                string endpoint = $"{GetunikConstants.ZohoProjects_APIEndpoint_V3}/portal/" +
                    $"{GetunikConstants.ZohoProjects_PortalId}/users";

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
                    var responseObj = JsonConvert.DeserializeObject<GetProjectUsersResponse>(responseData);
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

        public async Task<ApiResultDto<CreateTaskResponse>> CreateTask(
            string projectId, CreateTaskRequest createTaskRequest)
        {

            var apiResult = new ApiResultDto<CreateTaskResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = string.Empty;
                accessToken = await _zohoAuthService
                        .GetAccessToken(GetunikConstants.GetUnik, CommonConstants.ZohoProjects);
                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{GetunikConstants.ZohoProjects_APIEndpoint_V3}/portal/" +
                    $"{GetunikConstants.ZohoProjects_PortalId}/projects/{projectId}/tasks";

                string requestBody = JsonConvert.SerializeObject(createTaskRequest);

                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    endpoint)
                {
                    Content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json")
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
                if (response.StatusCode == HttpStatusCode.OK 
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<CreateTaskResponse>(responseData);
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
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

    }

}
