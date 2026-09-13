using HoangZoho1.Models.Common;
using HoangZoho1.Models.Lumicare.ZohoCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Lumicare
{

    public interface ILumicareCrmService
    {

        #region Contacts

        Task<ApiResultDto<GetContactByIdResponse>> GetContactById(string email);

        Task<ApiResultDto<SearchContactsResponse>> SearchContactsByEmail(string email);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> CreateContact(UpsertRequest<CreateContactRequest> createContactRequest);

        Task<ApiResultDto<UpsertResponse<UpsertDetail>>> UpdateContact(string contactId,
            UpsertRequest<UpdateContactRequest> productLogRequest);

        #endregion

        #region Distributor

        Task<ApiResultDto<SearchDistributorsResponse>> SearchDistributorsByUsernameAndPassword(string username, string password);

        #endregion

        #region Latest News

        Task<ApiResultDto<QueryLastestNewsResponse>> QueryLatestNews(ZohoCoqlRequest coqlRequest);

        #endregion

    }

}
