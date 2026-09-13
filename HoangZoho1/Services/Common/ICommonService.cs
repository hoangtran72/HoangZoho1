using HoangZoho1.Models.Common;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Common
{

    public interface ICommonService
    {

        Task<ApiResultDto<string>> DownloadFileFromUrl(string url, string filePath);

    }

}
