using HoangZoho1.Models.Common;
using HoangZoho1.Models.OneCorp.Sakari;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.OneCorp
{

    public interface IOneCorpSakariService
    {

        Task<ApiResultDto<SendSakariSMSResponse>> SendSakariSMS(SendSakariSMSRequest smsRequest);

        Task<ApiResultDto<GetSakariMessagesResponse>> GetSakariMessages(int offset = 0);

        Task<ApiResultDto<GetPhoneGroupsResponse>> GetSakariPhoneGroups();

        Task<ApiResultDto<FetchContactsResponse>> FetchContacts(FetchContactsParameters parameters);

        Task<ApiResultDto<UpsertContactResponse>> CreateContact(UpsertContactRequest createContactRequest);

        Task<ApiResultDto<UpsertContactResponse>> UpdateContact(string contactId, UpsertContactRequest updateContactRequest);

    }

}
