using HoangZoho1.Models.Common;
using HoangZoho1.Models.GetUnik.ZohoBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GetUnik
{

    public interface IGetunikBooksService
    {

        Task<ApiResultDto<CreateInvoiceResponse>> CreateInvoice(string createInvoiceRequest);

        Task<ApiResultDto<SubmitInvoiceResponse>> SubmitInvoice(string invoiceId);

    }

}
