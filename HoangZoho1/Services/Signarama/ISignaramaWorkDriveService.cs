using HoangZoho1.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Signarama
{

    public interface ISignaramaWorkDriveService
    {

        Task<ApiResultDto<string>> DownloadFile(string resourceId, string fileName);

    }

}
