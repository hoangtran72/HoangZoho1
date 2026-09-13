using HoangZoho1.Models.Common;
using HoangZoho1.Models.ZoRaw.Stallion;
using System.Threading.Tasks;

namespace HoangZoho1.Services.ZoRaw
{

    public interface IZoRawStallionService
    {

        Task<ApiResultDto<GetStallionRatesResponse>> GetRates
            (GetStallionRatesRequest getRatesRequest);

        Task<ApiResultDto<CreateStallionShipmentResponse>> CreateShipment
            (CreateStallionShipmentRequest createShipmentRequest);

        Task<ApiResultDto<TrackShipmentResponse>> TrackShipment
                        (string trackingCode);

    }

}
