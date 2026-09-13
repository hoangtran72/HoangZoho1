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

namespace HoangZoho1.Services.LocalingTours
{

    public class LocalingToursCustomService : ILocalingToursCustomService
    {

        private const string BucketName = "localing-tour-photos";
        private const string Folder = "post-tour-reports";

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

            const int MaxDimension = 2000;
            const int JpegQuality = 90;

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

                // Magick.NET detects JPG, PNG, HEIC, HEIF, etc.
                using var image = new MagickImage(inputStream);

                // Fix phone/iPhone orientation
                image.AutoOrient();

                // Resize only if necessary
                if (image.Width > MaxDimension || image.Height > MaxDimension)
                {
                    image.Resize(new MagickGeometry(MaxDimension, MaxDimension)
                    {
                        IgnoreAspectRatio = false
                    });
                }

                // Remove EXIF/GPS metadata
                image.Strip();

                // Convert image to JPEG
                image.Format = MagickFormat.Jpeg;
                image.Quality = JpegQuality;

                using var outputStream = new MemoryStream();

                image.Write(outputStream);
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

    }
}
