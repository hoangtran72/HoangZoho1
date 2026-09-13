using HoangZoho1.Models.Common;
using HoangZoho1.Models.Zipfox.ZohoDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Zipfox
{

    public interface IZipfoxDeskService
    {

        Task<ApiResultDto<CreateTicketResponse>> CreateTicket(CreateTicketRequest createTicketRequest);

    }

}
