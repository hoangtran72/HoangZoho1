using HoangZoho1.Models.Common;
using HoangZoho1.Models.WclSolutions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.WclSolutions
{

    public interface IWclInventoryService
    {

        Task<ApiResultDto<UpdateSalesOrderResponse>> UpdateSalesOrder(string salesOrderId, 
            UpdateBatchNumberRequest updateBatchNumberRequest);

    }

}
