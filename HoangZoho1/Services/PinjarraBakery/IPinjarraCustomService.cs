using HoangZoho1.Models.Common;
using HoangZoho1.Models.PinjarraBakery.ZohoForm;
using HoangZoho1.Models.PinjarraBakery.ZohoProjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.PinjarraBakery
{
    public interface IPinjarraCustomService
    {
        #region Recruitment Process

        Task<ApiResultDto<string>> Recruitment_InitializeTasks(string projectId);

        Task<ApiResultDto<string>> Recruitment_SendEmail(string projectId, string taskId);

        Task<ApiResultDto<string>> Recruitment_SendR04NotiEmail(R04NotiEmailRequest r04Request);

        #endregion

        #region Bakehouse Product Log

        Task<ApiResultDto<string>> HandleBakehouseProductLog(HandleProductLogRequest createProductLogRequest);

        #endregion

    }
}
