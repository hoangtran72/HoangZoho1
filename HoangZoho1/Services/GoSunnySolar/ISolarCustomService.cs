using HoangZoho1.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GoSunnySolar
{
    public interface ISolarCustomService
    {

        Task<ApiResultDto<string>> UpsertLeadsInCrmFromGhl(string contactId);

    }
}
