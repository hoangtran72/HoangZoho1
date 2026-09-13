using HoangZoho1.Models.Common;
using HoangZoho1.Models.ZoRaw.Freightcom;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoangZoho1.Services.ZoRaw
{

    public interface IZoRawFreightcomService
    {

        public Task<ApiResultDto<CalculateFreightClassResponse>> CalculateFreightClass
            (CalculateFreightClassRequest calculateFreightClassRequest);

        public Task<ApiResultDto<RequestRateEstimateResponse>> RequestRateEstimate
            (RequestRateEstimateRequest requestRateEstimateRequest);

        public Task<ApiResultDto<RetrieveARateResponse>> RetrieveARate
            (string rateId);

        public Task<ApiResultDto<CreateFreightcomShipmentResponse>> CreateFreightcomShipment
            (CreateFreightcomShipmentRequest createFreightcomShipmentRequest);

        public Task<ApiResultDto<RetrieveShipmentDetailsResponse>> RetrieveShipmentDetails
            (string shipmentId);

        public Task<ApiResultDto<List<PaymentMethod>>> GetPaymentMethods();

    }

}
