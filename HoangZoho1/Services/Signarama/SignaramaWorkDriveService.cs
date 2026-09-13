using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Services.ZohoAuth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Signarama
{
    public class SignaramaWorkDriveService : ISignaramaWorkDriveService
    {

        private readonly IZohoAuthService _zohoAuthService;
        private string DocumentPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Documents";

        public SignaramaWorkDriveService(IZohoAuthService zohoAuthService)
        {
            _zohoAuthService = zohoAuthService;
        }

        public async Task<ApiResultDto<string>> DownloadFile(
            string resourceId, string fileName)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest
            };

            try
            {

                // STEP 1: Get Zoho WorkDrive Access Token
                string accessToken = await _zohoAuthService.GetAccessToken("Signarama", "ZohoWorkDrive");
                if (string.IsNullOrEmpty(accessToken))
                {
                    return apiResult;
                }

                string filePath = $"{DocumentPath}/{fileName}";

                string endpoint = $"https://download.zoho.com/v1/workdrive/download/{resourceId}";

                var request = new HttpRequestMessage(
                          HttpMethod.Get,
                          endpoint);
                using var httpClient = new HttpClient(
                             new HttpClientHandler()
                             {
                                 AutomaticDecompression = DecompressionMethods.GZip
                             });
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {accessToken}");

                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var byteArray = await response.Content.ReadAsByteArrayAsync();
                    File.WriteAllBytes(filePath, byteArray);
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = filePath;
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
