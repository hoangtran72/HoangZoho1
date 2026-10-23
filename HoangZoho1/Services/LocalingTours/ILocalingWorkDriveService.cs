using HoangZoho1.Models.Common;
using HoangZoho1.Models.LocalingTours;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoangZoho1.Services.LocalingTours
{
    public interface ILocalingWorkDriveService
    {
        Task<ApiResultDto<List<WorkDriveFile>>> GetDirectChildTourPhotoImages(string folderId);
        Task<ApiResultDto<byte[]>> DownloadFile(string downloadUrl, string fileId, string fileName);
        Task<ApiResultDto<string>> UploadFile(string folderId, string fileName, byte[] fileBytes);
    }
}
