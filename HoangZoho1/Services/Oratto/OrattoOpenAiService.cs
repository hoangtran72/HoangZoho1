using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Oratto.OpenAI;
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

    public class OrattoOpenAiService : IOrattoOpenAIService
    {
        
        public async Task<ApiResultDto<UploadFileResponse>> UploadFile2OpenAI(string filePath
            , string fileName, string purpose)
        {

            var apiResult = new ApiResultDto<UploadFileResponse>()
            {
                Code = ResultCode.BadRequest
            };

            try
            {

                string endpoint = OrattoConstants.OpenAiV1Endpoint + "/files";
                var requestContent = new MultipartFormDataContent();
                var byteArray = File.ReadAllBytes(filePath);
                var fileContent = new ByteArrayContent(byteArray);
                requestContent.Add(fileContent, "file", fileName);
                requestContent.Add(new StringContent("assistants"), "purpose");

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
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {OrattoConstants.OpenAiToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiResult.Code = ResultCode.OK;
                    var responseObj = JsonConvert.DeserializeObject<UploadFileResponse>(responseData);
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

        public async Task<ApiResultDto<ChatCompletionResponse>> ChatCompletions(ChatCompletionRequest completionRequest)
        {
            var apiResult = new ApiResultDto<ChatCompletionResponse>()
            {
                Code = ResultCode.BadRequest,
            };

            try
            {
                string endpoint = OrattoConstants.OpenAiV1Endpoint + "/chat/completions";
                string requestBody = JsonConvert.SerializeObject(completionRequest);
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {OrattoConstants.OpenAiToken}");
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                var reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseObj = JsonConvert.DeserializeObject<ChatCompletionResponse>(responseData);

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

    }

}
