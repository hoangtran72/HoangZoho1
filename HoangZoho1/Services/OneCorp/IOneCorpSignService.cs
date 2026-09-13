using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.ZohoCRM;
using HoangZoho1.Models.OneCorp.ZohoSign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.OneCorp
{

    public interface IOneCorpSignService
    {

        #region Document Management

        Task<ApiResultDto<GetDocumentDetailsByIdResponse>> GetDocumentDetailsById(string requestId);

        Task<ApiResultDto<string>> DownloadDocumentById(string requestId, string fileName);

        Task<ApiResultDto<DeleteDocumentResponse>> DeleteDocument(string requestId);

        #endregion

    }

}
