using HoangZoho1.Models.Common;
using HoangZoho1.Models.GGInsurance;
using System.Threading.Tasks;

namespace HoangZoho1.Services.GGInsurance
{

    public interface IGGInsuranceCrmService
    {

        #region Accounts

        Task<ApiResultDto<SearchAccountsResponse>>
            SearchAccountsByPhone(string phone);

        #endregion

        #region Contacts

        Task<ApiResultDto<SearchContactsResponse>>
            SearchContactsByPhone(string phone);

        #endregion
    }

}
