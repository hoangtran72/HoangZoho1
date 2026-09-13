using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Services.ZohoAuth;
using System;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GGInsurance
{

    public class GGInsuranceCustomService : IGGInsuranceCustomService
    {

        private readonly IGGInsuranceCrmService _crmService;

        public GGInsuranceCustomService(IGGInsuranceCrmService zohoAuthService)
        {
            _crmService = zohoAuthService;
        }

        public async Task<ApiResultDto<string>> GetZohoCrmRedirectUrl4Phone(string phone)
        {

            var apiResultDto = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = GGInsuranceConstants.GZCRU4P_400,
            };

            try
            {

                // STEP 1: Set Default URL
                string encodedPhone = Uri.EscapeDataString(phone);
                string searchUrl = GGInsuranceConstants.ZohoCRM_Search_URL
                    .Replace("$PhoneUrlEncoded$", encodedPhone);

                // STEP 2: Search Accounts by Phone
                string accountID = string.Empty;
                var searchAccountsByPhoneResponse = await _crmService
                    .SearchAccountsByPhone(phone);
                if (searchAccountsByPhoneResponse.Code == ResultCode.OK)
                {
                    var searchAccounts = searchAccountsByPhoneResponse.Data.data;
                    var firstAccount = searchAccounts[0];
                    accountID = firstAccount.id;
                }
                if (!string.IsNullOrEmpty(accountID))
                {
                    string accountUrl = GGInsuranceConstants.ZohoCRM_AccountPrefix_URL + "/" + accountID;
                    apiResultDto.Code = ResultCode.OK;
                    apiResultDto.Message = GGInsuranceConstants.GZCRU4P_200;
                    apiResultDto.Data = accountUrl;
                    return apiResultDto;
                }

                // STEP 3: Search Contacts by Phone
                string contactID = string.Empty;
                var searchContactsByPhoneResponse = await _crmService
                    .SearchContactsByPhone(phone);
                if (searchContactsByPhoneResponse.Code == ResultCode.OK)
                {
                    var searchContacts = searchContactsByPhoneResponse.Data.data;
                    var firstContact = searchContacts[0];
                    contactID = firstContact.id;
                }
                if (!string.IsNullOrEmpty(contactID))
                {
                    string contactUrl = GGInsuranceConstants.ZohoCRM_ContactPrefix_URL + "/" + contactID;
                    apiResultDto.Code = ResultCode.OK;
                    apiResultDto.Message = GGInsuranceConstants.GZCRU4P_200;
                    apiResultDto.Data = contactUrl;
                    return apiResultDto;
                }

                apiResultDto.Code = ResultCode.OK;
                apiResultDto.Message = GGInsuranceConstants.GZCRU4P_200;
                apiResultDto.Data = searchUrl;
                return apiResultDto;

            }
            catch (Exception ex)
            {
                apiResultDto.Message = $"{ex.Message} - {ex.Data}";
                return apiResultDto;
            }

        }
    }

}
