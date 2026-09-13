using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.ScheduleOnce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.OneBudget
{

    public interface IOneBudgetCustomService
    {

        Task<ApiResultDto<string>> ScheduleOnce_SyncBooking(BookingPayload bookingPayload);

        Task<ApiResultDto<string>> SyncBookingById(string bookingId);

    }


}
