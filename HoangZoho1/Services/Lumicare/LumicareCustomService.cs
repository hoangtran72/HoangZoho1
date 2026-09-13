using HoangZoho1.Constants;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Lumicare.TalentLMS;
using HoangZoho1.Models.Lumicare.ZohoCRM;
using HoangZoho1.Models.Lumicare.ZohoForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HoangZoho1.Services.Lumicare
{
    public class LumicareCustomService : ILumicareCustomService
    {

        private readonly ILumicareCrmService _lumicareCrmService;
        private readonly ITalentLMSService _talentLMSService;

        public LumicareCustomService(ILumicareCrmService lumicareCrmService, ITalentLMSService talentLMSService)
        {
            _lumicareCrmService = lumicareCrmService;
            _talentLMSService = talentLMSService;
        }

        public async Task<ApiResultDto<string>> HandleTrainingForm(TrainingFormRequest trainingFormRequest)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = LumicareConstants.CUSTOM_HTF_400
            };

            try
            {
                string salutation = trainingFormRequest.Salutation;
                string firstName = trainingFormRequest.FirstName;
                string lastName = trainingFormRequest.LastName;
                string email = trainingFormRequest.Email;
                string mobile = trainingFormRequest.Mobile;
                string title = trainingFormRequest.Title;
                string companyName = trainingFormRequest.CompanyName;
                string specialty = trainingFormRequest.Specialty;
                string country = trainingFormRequest.Country;

                // STEP 1: Search Contact by Email
                string contactId = string.Empty;
                var searchContactByEmailResponse = await _lumicareCrmService.SearchContactsByEmail(email);
                if (searchContactByEmailResponse.Code == ResultCode.OK)
                {
                    var contactDetails = searchContactByEmailResponse.Data.data[0];
                    contactId = contactDetails.id;
                }

                // STEP 2: Create Contact if not exist
                if (string.IsNullOrEmpty(contactId))
                {
                    var createContactRequest = new CreateContactRequest() {
                        Salutation = salutation,
                        First_Name = firstName,
                        Last_Name = lastName,
                        Email = email,
                        Mobile = mobile,
                        Title = title,
                        Company_Name = companyName,
                        Specialty = specialty,
                        Country_select = country
                    };
                    var upsertRequest = new UpsertRequest<CreateContactRequest>();
                    upsertRequest.data.Add(createContactRequest);

                    var createContactResponse = await _lumicareCrmService.CreateContact(upsertRequest);
                    if (createContactResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = LumicareConstants.CUSTOM_HTF_C01;
                        return apiResult;
                    }
                    var createContactDetails = createContactResponse.Data.data[0];
                    contactId = createContactDetails.details.id;
                }

                // STEP 3: Search Talent LMS user by email
                var searchLMSUserByEmailResponse = await _talentLMSService.GetUserByEmail(email);
                bool assignOnboardingCourse = false;
                string lmsUserId = string.Empty;
                if (searchLMSUserByEmailResponse.Code == ResultCode.OK)
                {
                    // CASE 1: LMS User already exists
                    var userDetails = searchLMSUserByEmailResponse.Data;
                    lmsUserId = userDetails.id;
                    string lmsUserName = userDetails.login;
                    var updateContactRequest = new UpdateContactRequest()
                    {
                        LMS_User_Id = lmsUserId,
                        LMS_Username = lmsUserName,
                        LMS_Email_Type = "Existing User",
                        Send_LMS_Email = true
                    };
                    var upsertRequest = new UpsertRequest<UpdateContactRequest>();
                    upsertRequest.data.Add(updateContactRequest);
                    upsertRequest.trigger.Add("workflow");
                    var updateContactResponse = await _lumicareCrmService.UpdateContact(contactId, upsertRequest);
                    if (updateContactResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = LumicareConstants.CUSTOM_HTF_C02;
                        return apiResult;
                    }

                    var relatedCourses = userDetails.courses;
                    foreach (var course in relatedCourses)
                    {
                        string courseId = course.id;
                        if (courseId == LumicareConstants.OnboardingCourse.ToString())
                        {
                            assignOnboardingCourse = true;
                            break;
                        }
                    }
                    if (!assignOnboardingCourse)
                    {
                        var addUserToCourseRequest = new AddUserToCourseModel()
                        {
                            CourseId = LumicareConstants.OnboardingCourse.ToString(),
                            UserId = lmsUserId,
                            Role = "learner"
                        };
                        var addUserToCourseResponse = await _talentLMSService.AddUserToCourse(addUserToCourseRequest);
                        if (addUserToCourseResponse.Code != ResultCode.OK)
                        {
                            apiResult.Message = LumicareConstants.CUSTOM_HTF_L02;
                            return apiResult;
                        }
                    }
                }
                else
                {
                    // CASE 2: LMS User doesn't exist yet
                    var updateContactRequest = new UpdateContactRequest()
                    {
                        LMS_Email_Type = "New User",
                        Send_LMS_Email = true
                    };
                    var upsertRequest = new UpsertRequest<UpdateContactRequest>();
                    upsertRequest.data.Add(updateContactRequest);
                    upsertRequest.trigger.Add("workflow");
                    var updateContactResponse = await _lumicareCrmService.UpdateContact(contactId, upsertRequest);
                    if (updateContactResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = LumicareConstants.CUSTOM_HTF_C02;
                        return apiResult;
                    }
                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = LumicareConstants.CUSTOM_HTF_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> ApproveClientToEnroll(string contactId)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = LumicareConstants.CUSTOM_ACE_400
            };

            try
            {
                // STEP 0: Initialize Constants and Variables
                var gmtTime = DateTimeOffset.UtcNow; //time at +0:00
                var usaEastTime = gmtTime.ToOffset(new TimeSpan(-4, 0, 0)); // time at -4:00
                var austTime = gmtTime.ToOffset(new TimeSpan(10, 0, 0)); // time at +10:00

                // STEP 1: Get Contact by Id
                var getContactByIdResponse = await _lumicareCrmService.GetContactById(contactId);
                if (getContactByIdResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = LumicareConstants.CUSTOM_ACE_C01;
                    return apiResult;
                }
                var contactDetails = getContactByIdResponse.Data.data[0];
                string email = contactDetails.Email;
                string firstName = contactDetails.First_Name;
                string lastName = contactDetails.Last_Name;
                if (string.IsNullOrEmpty(email))
                {
                    apiResult.Message = LumicareConstants.CUSTOM_ACE_C02;
                    return apiResult;
                }

                // STEP 3: Search Talent LMS user by email
                var searchLMSUserByEmailResponse = await _talentLMSService.GetUserByEmail(email);
                bool assignOnboardingCourse = false;
                string lmsUserId = string.Empty;
                if (searchLMSUserByEmailResponse.Code == ResultCode.OK)
                {
                    // CASE 1: LMS User already exists
                    var userDetails = searchLMSUserByEmailResponse.Data;
                    lmsUserId = userDetails.id;
                    string lmsUserName = userDetails.login;
                    var updateContactRequest = new UpdateContactRequest()
                    {
                        LMS_User_Id = lmsUserId,
                        LMS_Username = lmsUserName,
                        LMS_Email_Type = "Existing User",
                        Send_LMS_Email = true
                    };
                    var upsertRequest = new UpsertRequest<UpdateContactRequest>();
                    upsertRequest.data.Add(updateContactRequest);
                    upsertRequest.trigger.Add("workflow");
                    var updateContactResponse = await _lumicareCrmService.UpdateContact(contactId, upsertRequest);
                    if (updateContactResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = LumicareConstants.CUSTOM_HTF_C02;
                        return apiResult;
                    }

                    var relatedCourses = userDetails.courses;
                    foreach (var course in relatedCourses)
                    {
                        string courseId = course.id;
                        if (courseId == LumicareConstants.OnboardingCourse.ToString())
                        {
                            assignOnboardingCourse = true;
                            break;
                        }
                    }
                }
                else
                {
                    // CASE 2: LMS User doesn't exist yet
                    var emailSplits = email.Split("@");
                    string emailFirstPart = emailSplits[0];

                    // Search user by username
                    var currentTime = DateTime.UtcNow;
                    string currentUsername = emailFirstPart;

                    int userIndex = 1;
                    var searchLMSUserByUsernameResponse = await _talentLMSService.GetUserByUsername(currentUsername);
                    while (searchLMSUserByUsernameResponse.Code == ResultCode.OK)
                    {
                        currentUsername = emailFirstPart + userIndex;
                        searchLMSUserByUsernameResponse = await _talentLMSService.GetUserByUsername(currentUsername);
                        userIndex++;
                    }

                    var signupUserRequest = new UserSignupModel()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        Login = currentUsername,
                        Password = LumicareConstants.TalentLMSPassword
                    };
                    var createUserResponse = await _talentLMSService.SignupUser(signupUserRequest);
                    if (createUserResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = LumicareConstants.CUSTOM_HTF_L01;
                        return apiResult;
                    }
                    var createUserDetails = createUserResponse.Data;
                    lmsUserId = createUserDetails.id.ToString();
                    string lmsUserName = createUserDetails.login;
                    var updateContactRequest = new UpdateContactRequest()
                    {
                        LMS_User_Id = lmsUserId,
                        LMS_Username = lmsUserName,
                        LMS_Email_Type = "User Approved",
                        User_Creation_Date = austTime.ToString("yyyy-MM-dd"),
                        Send_LMS_Email = true
                    };
                    var upsertRequest = new UpsertRequest<UpdateContactRequest>();
                    upsertRequest.data.Add(updateContactRequest);
                    upsertRequest.trigger.Add("workflow");
                    var updateContactResponse = await _lumicareCrmService.UpdateContact(contactId, upsertRequest);
                    if (updateContactResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = LumicareConstants.CUSTOM_HTF_C02;
                        return apiResult;
                    }
                }
                if (!assignOnboardingCourse)
                {
                    var addUserToCourseRequest = new AddUserToCourseModel()
                    {
                        CourseId = LumicareConstants.OnboardingCourse.ToString(),
                        UserId = lmsUserId,
                        Role = "learner"
                    };
                    var addUserToCourseResponse = await _talentLMSService.AddUserToCourse(addUserToCourseRequest);
                    if (addUserToCourseResponse.Code != ResultCode.OK)
                    {
                        apiResult.Message = LumicareConstants.CUSTOM_ACE_C03;
                        return apiResult;
                    }
                }

                apiResult.Code = ResultCode.OK;
                apiResult.Message = LumicareConstants.CUSTOM_ACE_200;
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<string>> CheckDistributorCredential(CheckDistributorRequest checkDistributorRequest)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = LumicareConstants.CUSTOM_CDC_400
            };

            try
            {
                string username = checkDistributorRequest.Username;
                string password = checkDistributorRequest.Password;

                // STEP 1: Search Distributor by 
                var searchDistributorsByCredentialResponse = await _lumicareCrmService
                    .SearchDistributorsByUsernameAndPassword(username, password);
                if (searchDistributorsByCredentialResponse.Code != ResultCode.OK)
                {
                    apiResult.Message = LumicareConstants.CUSTOM_CDC_D01;
                    return apiResult;
                }

                var distributorDetails = searchDistributorsByCredentialResponse.Data.data[0];
                string distributorName = distributorDetails.Name;

                string returnedUrl = $"{LumicareConstants.DistributorFormUrl}?distributor={HttpUtility.UrlEncode(distributorName)}";

                apiResult.Code = ResultCode.OK;
                apiResult.Message = LumicareConstants.CUSTOM_CDC_200;
                apiResult.Data = returnedUrl;
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
