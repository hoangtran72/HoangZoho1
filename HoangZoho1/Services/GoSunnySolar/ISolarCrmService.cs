using HoangZoho1.Models.Common;
using HoangZoho1.Models.GoSunnySolar.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GoSunnySolar
{
    public interface ISolarCrmService
    {
        Task<ApiResultDto<SearchLeadsResponse>> SearchLeadsByEmail(string email);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateLead(UpsertRequest<LeadForCreation> upsertRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateLead(string leadId, UpsertRequest<LeadForUpdation> upsertRequest);
    }
}
