using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.LocalingTours;
using HoangZoho1.Services.ZohoAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HoangZoho1.Services.LocalingTours
{
    public class LocalingWorkDriveService : ILocalingWorkDriveService
    {
        private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
            { "jpg", "jpeg", "png", "gif", "webp", "heic", "heif" };
        private readonly IZohoAuthService _zohoAuthService;

        public LocalingWorkDriveService(IZohoAuthService zohoAuthService) => _zohoAuthService = zohoAuthService;

        public async Task<ApiResultDto<List<WorkDriveFile>>> GetDirectChildTourPhotoImages(string folderId)
        {
            var result = new ApiResultDto<List<WorkDriveFile>> { Code = ResultCode.BadRequest, Message = CommonConstants.MSG_400 };
            try
            {
                var token = await _zohoAuthService.GetAccessToken("LocalingTours", CommonConstants.ZohoWorkDrive);
                if (string.IsNullOrWhiteSpace(token)) { result.Code = ResultCode.Unauthorize; result.Message = CommonConstants.MSG_401; return result; }
                var files = new List<WorkDriveFile>();
                var nextUrl = $"https://www.zohoapis.com/workdrive/api/v1/files/{Uri.EscapeDataString(folderId)}/files";
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {token}");
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.api+json");
                for (var page = 0; page < 75 && !string.IsNullOrWhiteSpace(nextUrl); page++)
                {
                    HttpResponseMessage response;
                    if (page == 0)
                    {
                        var query = "?page%5Blimit%5D=1000&page%5Bnext%5D=0&fields%5Bfiles%5D=name,is_folder,parent_id,type,extn,storage_info,download_url&sort=created_time";
                        response = await client.GetAsync(nextUrl + query);
                    }
                    else response = await client.GetAsync(nextUrl);
                    var body = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException(
                            $"WorkDrive file listing returned {(int)response.StatusCode} ({response.ReasonPhrase}): {body}");
                    var json = JObject.Parse(body);
                    var data = json["data"] as JArray;
                    if (data == null) throw new InvalidDataException("WorkDrive response did not contain data.");
                    foreach (var item in data)
                    {
                        var a = item["attributes"];
                        var name = (string)a?["name"];
                        var ext = (string)a?["extn"];
                        if ((bool?)a?["is_folder"] == false && (string)a?["parent_id"] == folderId &&
                            name?.StartsWith("TourPhoto", StringComparison.OrdinalIgnoreCase) == true &&
                            ImageExtensions.Contains(ext ?? string.Empty))
                            files.Add(new WorkDriveFile { Id = (string)item["id"], Name = name, DownloadUrl = (string)a["download_url"] });
                    }
                    var cursor = json["links"]?["cursor"];
                    nextUrl = (bool?)cursor?["has_next"] == true ? (string)cursor["next"] : null;
                }
                if (!string.IsNullOrWhiteSpace(nextUrl)) throw new InvalidOperationException("WorkDrive listing exceeded 75 pages.");
                result.Code = ResultCode.OK; result.Message = CommonConstants.MSG_200; result.Data = files;
            }
            catch (Exception ex) { result.Message = ex.Message; }
            return result;
        }

        public async Task<ApiResultDto<byte[]>> DownloadFile(string downloadUrl, string fileId, string fileName)
        {
            var result = new ApiResultDto<byte[]> { Code = ResultCode.BadRequest, Message = CommonConstants.MSG_400 };
            try
            {
                // WorkDrive returns the download URL with the folder listing; use that exact URL,
                // as the Deluge automation does, rather than constructing another route.
                var fileDownloadUrl = string.IsNullOrWhiteSpace(downloadUrl)
                    ? $"https://download.zoho.com/v1/workdrive/download/{Uri.EscapeDataString(fileId)}"
                    : downloadUrl;

                for (var attempt = 0; attempt < 2; attempt++)
                {
                    var token = await _zohoAuthService.GetAccessToken(
                        "LocalingTours", CommonConstants.ZohoWorkDrive, forceRefresh: attempt == 1);
                    if (string.IsNullOrWhiteSpace(token))
                    {
                        result.Code = ResultCode.Unauthorize;
                        result.Message = CommonConstants.MSG_401;
                        return result;
                    }

                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Zoho-oauthtoken {token}");
                    using var response = await client.GetAsync(fileDownloadUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        result.Code = ResultCode.OK;
                        result.Message = CommonConstants.MSG_200;
                        result.Data = await response.Content.ReadAsByteArrayAsync();
                        return result;
                    }

                    var errorBody = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && attempt == 0)
                        continue;

                    var tokenSource = string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(
                        "LOCALINGTOURS_ZOHO_WORKDRIVE_REFRESH_TOKEN"))
                        ? "bundled fallback refresh token"
                        : "LOCALINGTOURS_ZOHO_WORKDRIVE_REFRESH_TOKEN environment setting";
                    var requestUri = response.RequestMessage?.RequestUri;
                    var requestTarget = requestUri == null
                        ? "unknown"
                        : $"{requestUri.GetLeftPart(UriPartial.Path)}";
                    if (errorBody.Length > 1200)
                        errorBody = errorBody.Substring(0, 1200) + " [truncated]";
                    throw new HttpRequestException(
                        $"WorkDrive download returned {(int)response.StatusCode} ({response.ReasonPhrase}); " +
                        $"request={requestTarget}; tokenSource={tokenSource}. {errorBody}");
                }
            }
            catch (Exception ex) { result.Message = $"{fileName}: {ex.Message}"; }
            return result;
        }

        public async Task<ApiResultDto<string>> UploadFile(string folderId, string fileName, byte[] fileBytes)
        {
            var result = new ApiResultDto<string> { Code = ResultCode.BadRequest, Message = CommonConstants.MSG_400 };
            try
            {
                var token = await _zohoAuthService.GetAccessToken("LocalingTours", CommonConstants.ZohoWorkDrive);
                if (string.IsNullOrWhiteSpace(token)) { result.Code = ResultCode.Unauthorize; result.Message = CommonConstants.MSG_401; return result; }
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Zoho-oauthtoken {token}");
                using var form = new MultipartFormDataContent();
                var content = new ByteArrayContent(fileBytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                form.Add(content, "content", fileName);
                form.Add(new StringContent(fileName), "filename");
                form.Add(new StringContent(folderId), "parent_id");
                form.Add(new StringContent("true"), "override-name-exist");
                using var response = await client.PostAsync("https://www.zohoapis.com/workdrive/api/v1/upload", form);
                var body = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(
                        $"WorkDrive upload returned {(int)response.StatusCode} ({response.ReasonPhrase}): {body}");
                result.Code = ResultCode.OK; result.Message = CommonConstants.MSG_200; result.Data = body;
            }
            catch (Exception ex) { result.Message = ex.Message; }
            return result;
        }
    }
}
