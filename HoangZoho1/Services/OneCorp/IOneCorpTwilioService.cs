using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.Twilio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.OneCorp
{

    public interface IOneCorpTwilioService
    {

        Task<ApiResultDto<ReadMultipleMessageResourcesResponse>> 
            ReadMultipleMessageResources(string queryParameter = "");

    }

}
