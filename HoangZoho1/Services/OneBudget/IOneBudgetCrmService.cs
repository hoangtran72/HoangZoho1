using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneBudget.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.OneBudget
{

    public interface IOneBudgetCrmService
    {

        #region ScheduleOnce Bookings

        Task<ApiResultDto<SearchBookingResponse>> SearchScheduleOnceBookings(string criteria);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateBooking
            (UpsertRequest<BookingForCreation> createBookingRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateBooking
            (string bookingId, UpsertRequest<BookingForUpdation> updateBookingRequest);

        #endregion

        #region Users

        Task<ApiResultDto<GetUserByIdResponse>> GetUserById(string userId);

        Task<ApiResultDto<QueryUsersResponse>> QueryUsers(ZohoCoqlRequest coqlRequest);

        #endregion

        #region Blueprint

        Task<ApiResultDto<UpdateBlueprintResponse>> UpdateBlueprint
            (string recordModule, string recordId, UpdateBlueprintRequest blueprintRequest);

        #endregion

    }

}
