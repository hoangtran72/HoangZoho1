using HoangZoho1.Models.Common;
using HoangZoho1.Models.LocalingTours;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoangZoho1.Services.LocalingTours
{
    public interface ILocalingCrmService
    {
        Task<ApiResultDto<DrChipsRecord>> GetDrChipsById(string recordId);
        Task<ApiResultDto<string>> UpdateDrChipsPostTourImages(string recordId, List<string> imageUrls);
    }
}
