using HoangZoho1.Models.Common;
using HoangZoho1.Models.GoogleAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GoogleAPI
{
    public interface IGmailService
    {

        Task<ApiResultDto<SearchGmailsResponse>> SearchForEmails(string searchQuery);

        Task<ApiResultDto<GetEmailByIdResponse>> GetGmailById(string gmailId);

    }
}
