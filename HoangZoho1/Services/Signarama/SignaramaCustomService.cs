using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Signarama.Custom;
using HoangZoho1.Models.Signarama.Gemini;
using HoangZoho1.Models.Signarama.ZohoBooks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Signarama
{

    public class SignaramaCustomService : ISignaramaCustomService
    {

        private readonly ISignaramaGeminiService _geminiService;
        private readonly ISignaramaWorkDriveService _workDriveService;
        private readonly ISignaramaBooksService _booksService;

        public SignaramaCustomService(ISignaramaGeminiService geminiService,
            ISignaramaWorkDriveService workDriveService, 
            ISignaramaBooksService booksService)
        {
            _geminiService = geminiService;
            _workDriveService = workDriveService;
            _booksService = booksService;
        }

        public async Task<ApiResultDto<HandleEstimateResponse>> HandleUploadedEstimate(
            HandleEstimateRequest estimateRequest)
        {

            string filePath = string.Empty;
            var apiResult = new ApiResultDto<HandleEstimateResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = SignaramaConstants.HUE_400
            };

            try
            {

                // STEP 1: Download Estimate File
                string resourceId = estimateRequest.ResourceId;
                string fileName = estimateRequest.FileName;

                var downloadFileResponse = await _workDriveService
                    .DownloadFile(resourceId, fileName);
                
                if (downloadFileResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = SignaramaConstants.HUE_E01;
                    return apiResult;
                }
                filePath = downloadFileResponse.Data;

                // STEP 2: Upload the file to Gemini
                var uploadFileResponse = await _geminiService.UploadFile(filePath, fileName);
                if (uploadFileResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = SignaramaConstants.HUE_E02;
                }
                var upload = uploadFileResponse.Data.file;
                string fileUri = upload.uri;
                string fileType = upload.mimeType;

                // STEP 3: Generate File Content using Gemini
                var generateContentRequest = new GenerateContentRequest();
                var contents = new List<Content>();
                var content = new Content();
                var parts = new List<Part>();
                var textPart = new Part();
                textPart.text = SignaramaConstants.HUE_ExtractEstimateContent;
                parts.Add(textPart);

                var filePart = new Part();
                var fileData = new File_Data();
                fileData.mime_type = fileType;
                fileData.file_uri = fileUri;
                filePart.file_data = fileData;
                parts.Add(filePart);

                content.parts = parts;
                contents.Add(content);
                generateContentRequest.contents = contents;

                var generateContentResponse = await
                    _geminiService.GenerateContent(generateContentRequest);
                if (generateContentResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = OrattoConstants.GACBG_E02;
                    return apiResult;
                }
                var generateContentDetails = generateContentResponse.Data;
                var firstCandidate = generateContentDetails.candidates[0];
                var firstContent = firstCandidate.content;
                var firstPart = firstContent.parts[0];
                var estimateContent = firstPart.text;

                estimateContent = estimateContent.Replace("```json", "").Replace("```", "");

                var handleEstimateResponse = JsonConvert.DeserializeObject<HandleEstimateResponse>(estimateContent);
                apiResult.Code = ResultCode.OK;
                apiResult.Message = SignaramaConstants.HUE_200;
                apiResult.Data = handleEstimateResponse;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

        }

        public async Task<ApiResultDto<UploadAttachmentResponse>> UploadAttachment(
            UploadAttachmentRequest attachmentRequest)
        {
            string filePath = string.Empty;
            var apiResult = new ApiResultDto<UploadAttachmentResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = SignaramaConstants.UAB_400
            };

            try
            {

                // STEP 1: Download Estimate File
                string resourceId = attachmentRequest.ResourceId;
                string fileName = attachmentRequest.FileName;
                string moduleName = attachmentRequest.ModuleName;
                string moduleId = attachmentRequest.ModuleId;

                var downloadFileResponse = await _workDriveService
                    .DownloadFile(resourceId, fileName);

                if (downloadFileResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = SignaramaConstants.UAB_E01;
                    return apiResult;
                }
                filePath = downloadFileResponse.Data;

                // STEP 2: Upload File to Estimate
                var uploadFileResponse = await _booksService
                    .UploadAttachment(filePath, moduleName, moduleId);
                if (uploadFileResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = SignaramaConstants.UAB_E02;
                    return apiResult;
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = SignaramaConstants.UAB_200;
                apiResult.Data = uploadFileResponse.Data; 
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

        }

    }

}
