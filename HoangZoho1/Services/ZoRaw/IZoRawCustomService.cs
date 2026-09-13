using HoangZoho1.Models.Common;
using HoangZoho1.Models.ZoRaw.Custom;
using HoangZoho1.Models.ZoRaw.SalesData;
using HoangZoho1.Models.ZoRaw.Stallion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoangZoho1.Services.ZoRaw
{

    public interface IZoRawCustomService
    {

        #region Handle Sales in Zoho CRM

        List<TransformedRecord> ReadExcelToTransformedRecords
            (string inputFile, ExcelMapping mapping, 
            bool exportExcel = false, string outputPath = null);

        #endregion

        #region APIs for Zoho Inventory Widget

        Task<ApiResultDto<List<CommonShipmentRate>>> GetRatesFromWidget
            (GetRatesFromWidgetRequest request);

        Task<ApiResultDto<string>> BookShipmentFromWidget
            (BookShipmentFromWidgetRequest request);

        Task<ApiResultDto<string>> CreatePackageFromWidget
            (CreatePackageFromWidgetRequest request);

        #endregion

    }

}
