using HoangZoho1.Models.Common;
using HoangZoho1.Models.Twilio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace HoangZoho1.Services.Twilio
{
    public interface ITwilioService
    {
        ApiResultDto<MessageResource> SendTwilioSMS(SendSMSRequest smsRequest);
    }
}
