using HoangZoho1.Models.Common;
using HoangZoho1.Models.GGInsurance;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GGInsurance
{

    public interface IGGInsuranceCustomService
    {

        Task<ApiResultDto<string>>
            GetZohoCrmRedirectUrl4Phone(string phone);

    }

}
