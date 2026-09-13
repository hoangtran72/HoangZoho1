using HoangZoho1.Models.Common;
using HoangZoho1.Models.Zipfox;
using HoangZoho1.Models.Zipfox.WhatsApp;
using HoangZoho1.Models.Zipfox.ZohoDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Zipfox
{

    public interface IZipfoxCustomService
    {

        #region WhatsApp Functions

        Task<ApiResultDto<string>> HandleMessagePayload(MessagePayload payload);

        Task<ApiResultDto<string>> HandleStatusPayload(StatusPayload payload);

        #endregion

        #region Zoho Functions

        Task<ApiResultDto<string>> CreateContactForNotFoundSearch(ProductNotFoundContact[] contacts);

        Task<ApiResultDto<string>> CreateTicketForNotFoundSearch(ProductNotFoundContact[] contacts);

        #endregion

    }

}
