using HoangZoho1.Models.Common;
using HoangZoho1.Models.Getunik.ZohoCRM;
using HoangZoho1.Models.GetUnik.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GetUnik
{

    public interface IGetunikCrmService
    {

        #region Temporary Records

        Task<ApiResultDto<GetTemporaryRecordByIdResponse>> 
            GetTemporaryRecordById(string recordId);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> 
            UpdateTemporaryRecord(string recordId, string requestBody);

        #endregion

        #region Products

        Task<ApiResultDto<GetProductByIdResponse>>
            GetProductById(string productId);

        Task<ApiResultDto<GetProductDeliverablesResponse>>
            GetProductDeliverables(string productId);

        #endregion

        #region Quotes

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>>
            CreateQuote(string requestBody);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> 
            UpdateQuote(string quoteId, string requestBody);

        #endregion

    }

}
