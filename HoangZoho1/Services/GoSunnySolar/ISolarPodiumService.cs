using HoangZoho1.Models.Common;
using HoangZoho1.Models.GoSunnySolar.Podium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GoSunnySolar
{

    public interface ISolarPodiumService
    {

        Task<ApiResultDto<GetTemplatesResponse>> GetTemplates();

        Task<ApiResultDto<SendMessageResponse>> SendMessage(SendMessageRequest sendMessageRequest);

    }

}
