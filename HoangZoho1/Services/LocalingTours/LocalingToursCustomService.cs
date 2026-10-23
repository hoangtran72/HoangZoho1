using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.GetUnik.ZohoCRM;
using HoangZoho1.Models.LocalingTours;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HoangZoho1.Services.LocalingTours
{

    public class LocalingToursCustomService : ILocalingToursCustomService
    {

        private const string BucketName = "localing-tour-photos";
        private const string Folder = "post-tour-reports";
        private readonly ILocalingCrmService _crmService;
        private readonly ILocalingWorkDriveService _workDriveService;

        public LocalingToursCustomService(ILocalingCrmService crmService, ILocalingWorkDriveService workDriveService)
        {
            _crmService = crmService;
            _workDriveService = workDriveService;
        }

        public async Task<ApiResultDto<string>> XeroSync2ZohoStandalone(string invoiceId)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = "Xero Sync to Zoho FAILED"
            };

            try
            {

                string endpoint = LocalingToursConstants.Xero_Sync2Zoho_StandaloneFunction_Url;

                var syncRequest = new XeroSyncToZohoRequest
                {
                    InvoiceId = invoiceId
                };
                string syncRequestStr = JsonConvert.SerializeObject(syncRequest);

                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new StringContent(syncRequestStr, Encoding.UTF8, "application/json")
                };

                using var httpClient = new HttpClient();
                using var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead);

                var stream = await response.Content.ReadAsStreamAsync();

                // Convert stream to string
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.MSG_200;
                    apiResult.Data = responseData;
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

        public async Task<string> UploadS3Async(IFormFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            if (file.Length == 0)
                throw new Exception("Empty file.");


            // Create filename
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName)
                .Replace(" ", "_");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            // Detect image files
            bool isImage =
                (file.ContentType != null &&
                 file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                || extension == ".jpg"
                || extension == ".jpeg"
                || extension == ".png"
                || extension == ".webp"
                || extension == ".gif"
                || extension == ".bmp"
                || extension == ".tif"
                || extension == ".tiff"
                || extension == ".heic"
                || extension == ".heif";

            // Tour images are always stored as JPG
            var fileName = isImage
                ? $"{timestamp}_{originalFileName}.jpg"
                : $"{timestamp}_{originalFileName}{extension}";

            var key = $"{Folder}/{fileName}";

            // Credentials are resolved by the standard AWS credential provider chain
            // (environment variables, shared AWS profile, or the deployment IAM role).
            using var client = new AmazonS3Client(
                RegionEndpoint.GetBySystemName("ap-southeast-2"));

            PutObjectRequest request;

            if (isImage)
            {
                using var inputStream = file.OpenReadStream();
                using var outputStream = CompressImage(inputStream);
                outputStream.Position = 0;

                request = new PutObjectRequest
                {
                    BucketName = BucketName,
                    Key = key,
                    InputStream = outputStream,
                    ContentType = "image/jpeg",
                    AutoCloseStream = false
                };

                await client.PutObjectAsync(request);
            }
            else
            {
                // Non-image files are uploaded unchanged
                using var stream = file.OpenReadStream();

                request = new PutObjectRequest
                {
                    BucketName = BucketName,
                    Key = key,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    AutoCloseStream = false
                };

                await client.PutObjectAsync(request);
            }

            return $"https://{BucketName}.s3.{client.Config.RegionEndpoint.SystemName}.amazonaws.com/{key}";
        }

        public async Task<ApiResultDto<string>> UploadPostTourImagesToS3(string drChipsId)
        {
            var result = new ApiResultDto<string> { Code = ResultCode.BadRequest, Message = "Post tour image upload failed." };
            if (string.IsNullOrWhiteSpace(drChipsId)) { result.Message = "DR_CHIPS ID is required."; return result; }

            var recordResult = await _crmService.GetDrChipsById(drChipsId);
            var record = recordResult?.Data;
            if (record == null) { result.Message = recordResult?.Message ?? "DR_CHIPS record was not found."; return result; }
            if (string.IsNullOrWhiteSpace(record.WorkDrive_Folder_ID)) { result.Message = "WorkDrive folder ID is required."; return result; }

            var imagesResult = await _workDriveService.GetDirectChildTourPhotoImages(record.WorkDrive_Folder_ID);
            if (imagesResult?.Data == null || imagesResult.Data.Count == 0) { result.Message = imagesResult?.Message ?? "No direct-child TourPhoto images found."; return result; }

            var urls = new List<string>();
            foreach (var item in imagesResult.Data)
            {
                var fileResult = await _workDriveService.DownloadFile(item.DownloadUrl, item.Id, item.Name);
                if (fileResult?.Data == null || fileResult.Data.Length == 0)
                {
                    result.Message = $"Could not download WorkDrive image '{item.Name}'. " +
                        (string.IsNullOrWhiteSpace(fileResult?.Message) ? "WorkDrive returned no file data." : fileResult.Message);
                    return result;
                }
                using var source = new MemoryStream(fileResult.Data);
                using var compressed = CompressImage(source);
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                var originalFileName = Path.GetFileNameWithoutExtension(item.Name).Replace(" ", "_");
                var key = $"{Folder}/{timestamp}_{originalFileName}.jpg";
                using var client = new AmazonS3Client(RegionEndpoint.GetBySystemName("ap-southeast-2"));
                await client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = BucketName, Key = key, InputStream = compressed,
                    ContentType = "image/jpeg", AutoCloseStream = false
                });
                urls.Add($"https://{BucketName}.s3.{client.Config.RegionEndpoint.SystemName}.amazonaws.com/{key}");
            }

            var updateResult = await _crmService.UpdateDrChipsPostTourImages(drChipsId, urls);
            if (updateResult?.Code != ResultCode.OK) { result.Message = updateResult?.Message ?? "Could not update DR_CHIPS record."; return result; }
            result.Code = ResultCode.OK;
            result.Message = CommonConstants.MSG_200;
            result.Data = JsonConvert.SerializeObject(urls);
            return result;
        }

        public static MemoryStream CompressImage(Stream input)
        {
            const int maxDimension = 2000;
            using var image = new MagickImage(input);
            image.AutoOrient();
            if (image.Width > maxDimension || image.Height > maxDimension)
                image.Resize(new MagickGeometry(maxDimension, maxDimension) { IgnoreAspectRatio = false });
            image.Strip();
            image.Format = MagickFormat.Jpeg;
            image.Quality = 90;
            var output = new MemoryStream();
            image.Write(output);
            output.Position = 0;
            return output;
        }

        public async Task<ApiResultDto<string>> UploadImageToWorkDrive(string folderId, IFormFile file)
        {
            var result = new ApiResultDto<string> { Code = ResultCode.BadRequest, Message = "WorkDrive image upload failed." };
            if (file == null || file.Length == 0) { result.Message = "A non-empty image file is required."; return result; }
            try
            {
                using var source = file.OpenReadStream();
                using var compressed = CompressImage(source);
                var baseName = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "_");
                var upload = await _workDriveService.UploadFile(folderId, $"{baseName}.jpg", compressed.ToArray());
                result.Code = upload.Code;
                result.Message = upload.Message;
                result.Data = upload.Data;
            }
            catch (Exception ex) { result.Message = ex.Message; }
            return result;
        }

    }
}
