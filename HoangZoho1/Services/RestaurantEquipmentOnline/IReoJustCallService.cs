using HoangZoho1.Models.Common;
using HoangZoho1.Models.RestaurantEquipmentOnline.JustCall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.RestaurantEquipmentOnline
{
    public interface IReoJustCallService
    {

        Task<ApiResultDto<GetUserByIdResponse>> GetUserById(int agentId);

        Task<ApiResultDto<GetCallByIdResponse>> GetCallById(int callId);

        Task<ApiResultDto<GetListOfSMSResponse>> 
            GetListOfSMS(string page, string per_page = "100", string order = "ASC");

        Task<ApiResultDto<GetListOfCallResponse>> 
            GetListOfCalls(string page, string per_page = "100", string order = "ASC");

    }
}
