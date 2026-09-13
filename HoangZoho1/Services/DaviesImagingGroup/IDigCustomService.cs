using HoangZoho1.Models.Common;
using HoangZoho1.Models.DaviesImagingGroup;
using HoangZoho1.Models.DoAbility;
using System.Threading.Tasks;

namespace HoangZoho1.Services.DaviesImagingGroup
{

    public interface IDigCustomService
    {

        Task<ApiResultDto<string>> SendSpecPlusEmail
            (SendSpecPlusEmailRequest sendSpecPlusEmailRequest);

    }

}
