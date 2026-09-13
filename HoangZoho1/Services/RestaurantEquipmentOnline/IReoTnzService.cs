using HoangZoho1.Models.Common;
using HoangZoho1.Models.RestaurantEquipmentOnline.TNZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.RestaurantEquipmentOnline
{

    public interface IReoTnzService
    {

        Task<ApiResultDto<SendMessageResponse>> SendSMS(string token, SendSMSRequest sendSMSRequest);

    }

}
