using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.PinjarraBakery.ZohoProjects;
using HoangZoho1.Services.ZohoAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace HoangZoho1.Services.PinjarraBakery
{
    public class PinjarraProjectService : IPinjarraProjectService
    {
        private readonly IZohoAuthService _zohoAuthService;

        public PinjarraProjectService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        #region Projects

        public async Task<ApiResultDto<GetProjectsResponse>> GetProjectDetails(string projectId)
        {
            var apiResult = new ApiResultDto<GetProjectsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(PinjarraBakeryConstants.PinjarraBakery, CommonConstants.ZohoProjects);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{PinjarraBakeryConstants.ProjectEndpoint}/projects/{projectId}/";

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
                    var responseObj = JsonConvert.DeserializeObject<GetProjectsResponse>(responseData);
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

        public async Task<ApiResultDto<GetTasksResponse>> GetAllTasksInProject(string projectId)
        {

            var apiResult = new ApiResultDto<GetTasksResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(PinjarraBakeryConstants.PinjarraBakery, CommonConstants.ZohoProjects);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{PinjarraBakeryConstants.ProjectEndpoint}/projects/{projectId}/tasks/";

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
                    var responseObj = JsonConvert.DeserializeObject<GetTasksResponse>(responseData);
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

        public async Task<ApiResultDto<GetTasksResponse>> GetTaskById(string projectId, string taskId)
        {
            var apiResult = new ApiResultDto<GetTasksResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(PinjarraBakeryConstants.PinjarraBakery, CommonConstants.ZohoProjects);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{PinjarraBakeryConstants.ProjectEndpoint}/projects/{projectId}/tasks/{taskId}/";

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
                    var responseObj = JsonConvert.DeserializeObject<GetTasksResponse>(responseData);
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

        public async Task<ApiResultDto<AddCommentResponse>> AddComment(string projectId, string taskId, AddCommentRequest addComment)
        {

            var apiResult = new ApiResultDto<AddCommentResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(PinjarraBakeryConstants.PinjarraBakery, CommonConstants.ZohoProjects);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{PinjarraBakeryConstants.ProjectEndpoint}/projects/{projectId}/tasks/{taskId}/comments/";

                var formContent = new List<KeyValuePair<string, string>>();
                formContent.Add(new KeyValuePair<string, string>("content", addComment.content));

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new FormUrlEncodedContent(formContent)
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
                    var responseObj = JsonConvert.DeserializeObject<AddCommentResponse>(responseData);

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

        public async Task<ApiResultDto<CreateTaskResponse>> UpdateTask(string projectId, string taskId, TaskForUpdation taskForUpdation)
        {
            var apiResult = new ApiResultDto<CreateTaskResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };
            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(PinjarraBakeryConstants.PinjarraBakery, CommonConstants.ZohoProjects);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{PinjarraBakeryConstants.ProjectEndpoint}/projects/{projectId}/tasks/{taskId}/";

                var formContent = new List<KeyValuePair<string, string>>();
                // Handle Endpoint for Task
                if (!string.IsNullOrEmpty(taskForUpdation.Description))
                {
                    formContent.Add(new KeyValuePair<string, string>("description", taskForUpdation.Description));
                }
                var customFields = new TaskCustomFields()
                {
                    EmailSubject = taskForUpdation.EmailSubject,
                    EmailContent = taskForUpdation.EmailContent
                };
                formContent.Add(new KeyValuePair<string, string>("custom_fields", customFields.CreateRequestString()));

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new FormUrlEncodedContent(formContent)
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
                    var responseObj = JsonConvert.DeserializeObject<CreateTaskResponse>(responseData);

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseObj;
                }
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = ex.Message + " - " + ex.StackTrace; 
                return apiResult;
            }
        }

        #endregion

    }
}
