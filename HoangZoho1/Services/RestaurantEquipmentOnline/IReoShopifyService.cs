using HoangZoho1.Models.Common;
using HoangZoho1.Models.RestaurantEquipmentOnline.Shopify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.RestaurantEquipmentOnline
{

    public interface IReoShopifyService
    {

        Task<ApiResultDto<GetQuoteDetailsResponse>> GetQuoteDetails(GetQuoteDetailsRequest quoteRequest);

    }

}
