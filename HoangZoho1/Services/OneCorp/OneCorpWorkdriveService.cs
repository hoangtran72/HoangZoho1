using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GoSunnySolar.ZohoCRM;
using HoangZoho1.Models.OneCorp.ZohoWorkdrive;
using HoangZoho1.Services.ZohoAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HoangZoho1.Services.OneCorp
{
    public class OneCorpWorkdriveService : IOneCorpWorkdriveService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public OneCorpWorkdriveService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<UploadFileResponse>> UploadFile(UploadFileRequest uploadRequest)
        {

            var apiResult = new ApiResultDto<UploadFileResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoWorkDrive);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoWorkdrive_EndpointV1}/upload";

                var fileContent = uploadRequest.Content;
                string fileName = uploadRequest.FileName;
                string folderId = uploadRequest.ParentId;
                string nameExist = uploadRequest.OverrideNameExist ? "true" : "false";

                using var form = new MultipartFormDataContent();
                form.Add(new ByteArrayContent(fileContent, 0, fileContent.Length), "content", fileName);
                form.Add(new StringContent(fileName), "filename");
                form.Add(new StringContent(folderId), "parent_id");
                form.Add(new StringContent(nameExist), "override-name-exist");

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.PostAsync(endpoint, form);
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<UploadFileResponse>(responseData);
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

        public async Task<ApiResultDto<ListFilesFoldersResponse>> ListFilesFolders(string parentId, string filterBy)
        {
            var apiResult = new ApiResultDto<ListFilesFoldersResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoWorkDrive);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoWorkdrive_EndpointV1}/files/{parentId}/files";

                if (!string.IsNullOrEmpty(filterBy))
                {
                    endpoint += $"?{filterBy}";
                }

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                request.Headers.TryAddWithoutValidation("Accept", "application/vnd.api+json");

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");

                using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<ListFilesFoldersResponse>(responseData);
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

        public async Task<ApiResultDto<SearchAcrossFolderResponse>> SearchAcrossFolder(SearchAcrossFolderRequest searchRequest)
        {
            var apiResult = new ApiResultDto<SearchAcrossFolderResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.SAF_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoWorkDrive);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string teamId = searchRequest.TeamId;
                string filterQuery = searchRequest.FilterQuery;

                string endpoint = $"{OneCorpConstants.ZohoWorkdrive_EndpointV1}/teams/{teamId}/records";

                if (!string.IsNullOrEmpty(filterQuery))
                {
                    endpoint += $"?{filterQuery}";
                }

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                request.Headers.TryAddWithoutValidation("Accept", "application/vnd.api+json");

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");

                using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<SearchAcrossFolderResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = OneCorpConstants.SAF_200;
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

        public async Task<ApiResultDto<CreateFolderResponse>> CreateFolder(CreateFolderRequest createFolderRequest)
        {
            var apiResult = new ApiResultDto<CreateFolderResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoWorkDrive);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string endpoint = $"{OneCorpConstants.ZohoWorkdrive_EndpointV1}/files";

                string folderName = createFolderRequest.FolderName;
                string parentId = createFolderRequest.ParentId;

                string requestBody = OneCorpConstants.CreateFolder_RequestBody
                    .Replace("$FolderName$", folderName)
                    .Replace("$ParentId$", parentId);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "text/plain")
                };
                request.Headers.TryAddWithoutValidation("Accept", "application/vnd.api+json");

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<CreateFolderResponse>(responseData);
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


        public async Task<ApiResultDto<MoveFolderResponse>> MoveFolder(MoveFolderRequest moveFolderRequest)
        {
            var apiResult = new ApiResultDto<MoveFolderResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.MF_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                        .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoWorkDrive);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }

                string sourceId = moveFolderRequest.SourceId;
                string destinationId = moveFolderRequest.DestinationId;

                string endpoint = $"{OneCorpConstants.ZohoWorkdrive_EndpointV1}/files/{sourceId}";

                string requestBody = OneCorpConstants.MoveFolder_RequestBody
                    .Replace("$DestinationId$", destinationId);

                var request = new HttpRequestMessage(
                           HttpMethod.Patch,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "text/plain")
                };
                request.Headers.TryAddWithoutValidation("Accept", "application/vnd.api+json");

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");
                using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK
                    || response.StatusCode == HttpStatusCode.Created)
                {
                    var responseObj = JsonConvert.DeserializeObject<MoveFolderResponse>(responseData);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = OneCorpConstants.MF_200;
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
