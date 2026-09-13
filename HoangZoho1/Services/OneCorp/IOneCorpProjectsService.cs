using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.ZohoProjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.OneCorp
{

    public interface IOneCorpProjectsService
    {

        Task<ApiResultDto<string>> AssociateTag(AssociateTagRequest associateRequest);

    }

}
