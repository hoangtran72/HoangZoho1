using Google.Ads.GoogleAds.Config;
using Google.Ads.GoogleAds.Lib;
using Google.Ads.GoogleAds.V18.Common;
using Google.Ads.GoogleAds.V18.Errors;
using Google.Ads.GoogleAds.V18.Resources;
using Google.Ads.GoogleAds.V18.Services;
using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.DoAbility;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using static Google.Ads.GoogleAds.V18.Enums.ConsentStatusEnum.Types;
using static Google.Ads.GoogleAds.V18.Enums.CustomerMatchUploadKeyTypeEnum.Types;
using static Google.Ads.GoogleAds.V18.Enums.OfflineUserDataJobTypeEnum.Types;

namespace HoangZoho1.Services.DoAbility
{

    public class GoogleAdsService : IGoogleAdsService
    {

        private readonly GoogleAdsClient client;

        public GoogleAdsService()
        {
            
            var googleAdsConfig = new GoogleAdsConfig()
            {
                DeveloperToken = DoAbilityConstants.GoogleAds_DeveloperToken,
                OAuth2ClientId = DoAbilityConstants.GoogleAds_ClientId,
                OAuth2ClientSecret = DoAbilityConstants.GoogleAds_ClientSecret,
                OAuth2RefreshToken = DoAbilityConstants.GoogleAds_RefreshToken,
                LoginCustomerId = DoAbilityConstants.GoogleAds_Login_CustomerId
            };
            client = new GoogleAdsClient(googleAdsConfig);
        }

        public ApiResultDto<MutateUserListsResponse> CreateCustomerMatchUserList(
            CreateUserListRequest createUserListRequest)
        {

            var apiResult = new ApiResultDto<MutateUserListsResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {

                string userListName = createUserListRequest.UserListName;
                string userListDescription = createUserListRequest.UserListDescription;

                UserListServiceClient userListService = client
                    .GetService(Google.Ads.GoogleAds.Services.V18.UserListService);

                // Create a user list
                var userListOperation = new UserListOperation
                {
                    Create = new UserList
                    {
                        Name = userListName,
                        Description = userListDescription,
                        MembershipLifeSpan = 10000,
                        CrmBasedUserList = new CrmBasedUserListInfo
                        {
                            UploadKeyType = CustomerMatchUploadKeyType.ContactInfo
                        }
                    }
                };

                // Add the user list.
                MutateUserListsResponse response =
                    userListService.MutateUserLists(DoAbilityConstants.GoogleAds_Login_CustomerId, 
                    new[] { userListOperation });

                string userListResourceName = response.Results[0].ResourceName;

                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                apiResult.Data = response;

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return apiResult;

            }
        
        }

        public ApiResultDto<UploadUserDataResponse> AddUserToCustomerMatchUserList(
            AddUserToCustomerMatchUserListRequest userDataRequest)
        {

            var apiResult = new ApiResultDto<UploadUserDataResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = DoAbilityConstants.AU2CMUL_400
            };
           
            try
            {

                #region Handle User Data

                string firstName = userDataRequest.FirstName;
                string lastName = userDataRequest.LastName;
                string email = userDataRequest.Email;
                string phone = userDataRequest.Phone;

                string address = userDataRequest.Address;
                string street2 = userDataRequest.Street2;
                string city = userDataRequest.City;
                string state = userDataRequest.State;
                string countryCode = userDataRequest.CountryCode;
                string postalCode = userDataRequest.PostalCode;

                var userData = new UserData();

                // Step 1: Handle Email and Phone
                if (!string.IsNullOrEmpty(email))
                {
                    userData.UserIdentifiers.Add(new UserIdentifier
                    {
                        HashedEmail = StringHelpers.NormalizeAndHash(email)
                    });
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    userData.UserIdentifiers.Add(new UserIdentifier
                    {
                        HashedPhoneNumber = StringHelpers.NormalizeAndHash(phone)
                    });
                }

                // Step 2: Handle Address
                var addressInfo = new OfflineUserAddressInfo();
                if (!string.IsNullOrEmpty(firstName))
                {
                    addressInfo.HashedFirstName = StringHelpers.NormalizeAndHash(firstName);
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    addressInfo.HashedLastName = StringHelpers.NormalizeAndHash(lastName);
                }

                string streetAddress = string.Empty;
                if (!string.IsNullOrEmpty(address))
                {
                    streetAddress += address;
                }
                if (!string.IsNullOrEmpty(street2))
                {
                    if (string.IsNullOrEmpty(streetAddress))
                    {
                        streetAddress = street2;
                    }
                    else
                    {
                        streetAddress += ", " + street2;
                    }
                }

                if (!string.IsNullOrEmpty(streetAddress))
                {
                    addressInfo.HashedStreetAddress = StringHelpers.NormalizeAndHash(streetAddress);
                }

                if (!string.IsNullOrEmpty(city))
                {
                    addressInfo.City = city;
                }

                if (!string.IsNullOrEmpty(state))
                {
                    addressInfo.State = state;
                }

                if (!string.IsNullOrEmpty(postalCode))
                {
                    addressInfo.PostalCode = postalCode;
                }

                if (!string.IsNullOrEmpty(countryCode))
                {
                    addressInfo.CountryCode = countryCode;
                }

                UserIdentifier addressIdentifier = new UserIdentifier()
                {
                    AddressInfo = addressInfo
                };

                userData.UserIdentifiers.Add(addressIdentifier);

                #endregion

                #region Add User Data to Customer Match List

                UserDataOperation operation = new UserDataOperation
                {
                    Create = userData
                };

                string userListResourceName = userDataRequest.UserListResourceName;

                var customerMatchUserListMetadata = new CustomerMatchUserListMetadata();
                customerMatchUserListMetadata.UserList = userListResourceName;
                customerMatchUserListMetadata.Consent = new Consent
                {
                    AdPersonalization = ConsentStatus.Granted,
                    AdUserData = ConsentStatus.Granted
                };

                UploadUserDataRequest request = new UploadUserDataRequest
                {
                    CustomerId = DoAbilityConstants.GoogleAds_Login_CustomerId,
                    Operations = { operation },
                    CustomerMatchUserListMetadata = customerMatchUserListMetadata
                };

                UserDataServiceClient userDataServiceClient = client.GetService(Google.Ads.GoogleAds.Services.V18.UserDataService);
                
                UploadUserDataResponse response = userDataServiceClient.UploadUserData(request);

                #endregion

                apiResult.Code = ResultCode.OK;
                apiResult.Message = DoAbilityConstants.AU2CMUL_200;
                apiResult.Data = response;

                return apiResult;

            }
            catch (Exception ex)
            {

                apiResult.Message = ex.Message + " - " + ex.StackTrace;
                return apiResult;
            
            }

        }

        public ApiResultDto<string> RemoveUserFromCustomerMatchUserList(
            RemoveUserFromCustomerMatchUserListRequest removeRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = DoAbilityConstants.RUFCMUL_400
            };

            try
            {

                string email = removeRequest.Email;

                var userDataJobService = client.GetService(Google.Ads.GoogleAds.Services.V18.OfflineUserDataJobService);

                var job = new OfflineUserDataJob
                {
                    CustomerMatchUserListMetadata = new CustomerMatchUserListMetadata
                    {
                        UserList = removeRequest.UserListResourceName,
                    },
                    Type = OfflineUserDataJobType.CustomerMatchUserList
                };

                var createJobResponse = userDataJobService.CreateOfflineUserDataJob(
                    new CreateOfflineUserDataJobRequest
                    {
                        CustomerId = DoAbilityConstants.GoogleAds_Login_CustomerId,
                        Job = job
                    });

                var jobResourceName = createJobResponse.ResourceName;

                var operation = new OfflineUserDataJobOperation
                {
                    Remove = new UserData
                    {
                        UserIdentifiers =
                {
                    new UserIdentifier
                    {
                        HashedEmail = StringHelpers.NormalizeAndHash(email)
                    }
                }
                    }
                };

                userDataJobService.AddOfflineUserDataJobOperations(new AddOfflineUserDataJobOperationsRequest
                {
                    ResourceName = jobResourceName,
                    Operations = { operation },
                    EnablePartialFailure = true
                });

                userDataJobService.RunOfflineUserDataJob(new RunOfflineUserDataJobRequest
                {
                    ResourceName = jobResourceName
                });

                apiResult.Code = ResultCode.OK;
                apiResult.Message = DoAbilityConstants.RUFCMUL_200;

                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }

            

        }

    }
}
