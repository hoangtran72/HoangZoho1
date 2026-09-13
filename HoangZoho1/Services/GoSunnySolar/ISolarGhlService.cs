using HoangZoho1.Models.Common;
using HoangZoho1.Models.GoSunnySolar.GHL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GoSunnySolar
{
    public interface ISolarGhlService
    {
        Task<ApiResultDto<GetContactByIdResponse>> GetContactById(string contactId);
    }
}
