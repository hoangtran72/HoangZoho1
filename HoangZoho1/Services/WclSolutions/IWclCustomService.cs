using HoangZoho1.Models.Common;
using HoangZoho1.Models.WclSolutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.WclSolutions
{
    public interface IWclCustomService
    {

        Task<GetMasterItemsResponse> GetMasterItems();

        Task<string> SyncWooOrder2ZohoResponse(SyncWooOrdersFromWidgetRequest request);

    }

}
