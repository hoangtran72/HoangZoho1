using HoangZoho1.Models.Common;
using HoangZoho1.Models.Lumicare.TalentLMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Lumicare
{
    public interface ITalentLMSService
    {

        #region Users

        Task<ApiResultDto<GetUserResponseModel>> GetUserByEmail(string email);

        Task<ApiResultDto<GetUserResponseModel>> GetUserByUsername(string username);

        Task<ApiResultDto<UserSignupResponse>> SignupUser(UserSignupModel signupModel);

        #endregion

        #region Courses

        Task<ApiResultDto<string>> AddUserToCourse(AddUserToCourseModel addUserToCourseModel); 

        #endregion

    }
}
