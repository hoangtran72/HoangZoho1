using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.ScheduleOnce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.OneCorp
{
    public interface IOneCorpOnceHubService
    {

        Task<ApiResultDto<GetBookingByIdResponse>> GetBookingById(string bookingId);

        Task<ApiResultDto<GetUserByIdResponse>> GetUserById(string userId);

    }
}
