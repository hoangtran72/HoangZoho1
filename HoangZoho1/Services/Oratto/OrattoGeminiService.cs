using DotnetGeminiSDK.Client.Interfaces;
using DotnetGeminiSDK.Model;
using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Oratto.Gemini;
using Microsoft.AspNetCore.StaticFiles;
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

namespace HoangZoho1.Services.Oratto
{

    public class OrattoGeminiService : IOrattoGeminiService
    {

        public async Task<ApiResultDto<UploadFileResponse>> UploadFile(string filePath, string fileName)
        {

            var apiResult = new ApiResultDto<UploadFileResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = OrattoConstants.UMA2G_400
            };

            try
            {

                string endpoint = $"{OrattoConstants.GeminiEndpoint}/upload/v1beta/files?" +
                    $"key={OrattoConstants.GeminiApiKey}";

                var client = new WebClient();

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(fileName, out string contentType))
                {
                    contentType = "application/octet-stream";
                }

                client.Headers.Add("Content-Type", contentType);

                byte[] responseArray = client.UploadFile(endpoint, "POST", filePath);
                client.Dispose();

                string responseData = System.Text.Encoding.ASCII.GetString(responseArray);

                apiResult.Code = ResultCode.OK;
                apiResult.Message = OrattoConstants.UMA2G_200;
                var responseObj = JsonConvert.DeserializeObject<UploadFileResponse>(responseData);
                apiResult.Data = responseObj;

                /*
                var requestContent = new MultipartFormDataContent();
                var byteArray = File.ReadAllBytes(filePath);

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(fileName, out string contentType))
                {
                    contentType = "application/octet-stream";
                }

                if (contentType.Contains("image", StringComparison.InvariantCultureIgnoreCase)
                    || contentType.Contains("video", StringComparison.InvariantCultureIgnoreCase)
                    || contentType.Contains("audio", StringComparison.InvariantCultureIgnoreCase))
                {
                    // await UploadImageAsync(filePath);
                    endpoint += "&uploadType=media";
                }   

                var fileContent = new ByteArrayContent(byteArray, 0, byteArray.Length);

                requestContent.Headers.Remove("Content-Type");
                requestContent.Headers.Add("Content-Type", contentType);

                fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                requestContent.Add(fileContent, "file", fileName);

                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    endpoint)
                {
                    Content = requestContent
                };
                using var httpClient = new HttpClient(
                    new HttpClientHandler()
                    {
                        AutomaticDecompression = DecompressionMethods.GZip
                    });
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                // response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = OrattoConstants.UMA2G_200;
                    var responseObj = JsonConvert.DeserializeObject<UploadFileResponse>(responseData);
                    apiResult.Data = responseObj;
                }
                */
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

        }

        public async Task<ApiResultDto<GenerateContentResponse>> GenerateContent(GenerateContentRequest contentRequest)
        {
            var apiResult = new ApiResultDto<GenerateContentResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string endpoint = $"{OrattoConstants.GeminiEndpoint}/v1beta/models/" +
                    $"gemini-1.5-flash-001:generateContent?key={OrattoConstants.GeminiApiKey}";

                string requestBody = JsonConvert.SerializeObject(contentRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMinutes(10);
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject
                        <GenerateContentResponse>(responseData);

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

    }

}
