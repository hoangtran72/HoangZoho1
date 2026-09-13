using Google.Ads.GoogleAds.V18.Services;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.DoAbility;
using static Google.Ads.GoogleAds.V18.Enums.ConsentStatusEnum.Types;

namespace HoangZoho1.Services.DoAbility
{

    public interface IGoogleAdsService
    {

        ApiResultDto<MutateUserListsResponse> CreateCustomerMatchUserList
            (CreateUserListRequest createUserListRequest);

        ApiResultDto<UploadUserDataResponse> AddUserToCustomerMatchUserList
            (AddUserToCustomerMatchUserListRequest userDataRequest);

        ApiResultDto<string> RemoveUserFromCustomerMatchUserList(
            RemoveUserFromCustomerMatchUserListRequest removeRequest);

    }

}
