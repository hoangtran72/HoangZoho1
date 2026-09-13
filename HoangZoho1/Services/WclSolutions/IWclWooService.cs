using HoangZoho1.Models.Common;
using HoangZoho1.Models.WclSolutions;
using System.Threading.Tasks;

namespace HoangZoho1.Services.WclSolutions
{
    public interface IWclWooService
    {

        Task<ApiResultDto<GetWooOrderResponse>> GetWooOrderByNumber(
            string wooOrderNumber);

    }
}
