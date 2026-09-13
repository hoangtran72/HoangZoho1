using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Twilio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace HoangZoho1.Services.Twilio
{
    public class TwilioService : ITwilioService
    {
        public ApiResultDto<MessageResource> SendTwilioSMS(SendSMSRequest smsRequest)
        {
            var apiResult = new ApiResultDto<MessageResource>()
            {
                Code = ResultCode.BadRequest,
                Message = TwilioConstants.SendSMS_400
            };

            try
            {
                string accountSid = PinjarraBakeryConstants.AccountSID;
                string authToken = PinjarraBakeryConstants.AuthToken;
                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: smsRequest.Body,
                    from: smsRequest.From,
                    to: smsRequest.To
                );

                apiResult.Code = ResultCode.OK;
                apiResult.Message = TwilioConstants.SendSMS_200;
                apiResult.Data = message;
                return apiResult;
            }
            catch (Exception ex)
            {
                return apiResult;
            }
        }
    }
}
