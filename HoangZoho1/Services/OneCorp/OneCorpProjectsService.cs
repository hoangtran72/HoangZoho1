using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.ZohoProjects;
using HoangZoho1.Services.ZohoAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HoangZoho1.Services.OneCorp
{

    public class OneCorpProjectsService : IOneCorpProjectsService
    {

        private readonly IZohoAuthService _zohoAuthService;

        public OneCorpProjectsService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<string>> AssociateTag(AssociateTagRequest associateRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = OneCorpConstants.APT_400
            };

            try
            {
                string accessToken = await _zohoAuthService
                    .GetAccessToken(OneCorpConstants.OneCorp, CommonConstants.ZohoProjects);

                if (string.IsNullOrEmpty(accessToken))
                {
                    apiResult.Code = ResultCode.Unauthorize;
                    apiResult.Message = CommonConstants.MSG_401;
                    return apiResult;
                }
                string projectId = associateRequest.project_id;
                string entityId = associateRequest.entity_id;
                string tagId = associateRequest.tag_id;
                string entityType = associateRequest.entityType;

                string endpoint = $"{OneCorpConstants.ZohoProjects_EndpointV3}/projects/{projectId}/tags/associate?entity_id={entityId}&tag_id={tagId}&entityType={entityType}";
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

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = OneCorpConstants.APT_200;
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
