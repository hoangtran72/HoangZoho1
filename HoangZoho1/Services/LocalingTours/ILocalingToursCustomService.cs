using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HoangZoho1.Services.LocalingTours
{

    public interface ILocalingToursCustomService
    {

        public Task<ApiResultDto<string>> XeroSync2ZohoStandalone(string invoiceId);

        public Task<string> UploadS3Async(IFormFile file);

        public Task<ApiResultDto<string>> UploadPostTourImagesToS3(string drChipsId);

        public Task<ApiResultDto<string>> UploadImageToWorkDrive(string folderId, IFormFile file);

    }

}
