using HoangZoho1.Models.Common;
using HoangZoho1.Models.Getunik.Custom;
using HoangZoho1.Models.Getunik.ZohoProjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GetUnik
{

    public interface IGetunikCustomService
    {

        Task<ApiResultDto<string>> HandleCreateAndSubmitInvoice(string recordId);

        Task<ApiResultDto<GetProjectByIdResponse>> GetProjectDetailsFromUrl(string projectUrl);

        Task<ApiResultDto<string>> CreateBacklogTasks(CreateBacklogTasksRequest request);

    }

}
