using HoangZoho1.Constants;
using HoangZoho1.Helpers;
using HoangZoho1.Models.Common;
using HoangZoho1.Models.Lumicare.TalentLMS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HoangZoho1.Services.Lumicare
{
    public class TalentLMSService : ITalentLMSService
    {

        #region Users

        public async Task<ApiResultDto<GetUserResponseModel>> GetUserByUsername(string username)
        {
            var apiResult = new ApiResultDto<GetUserResponseModel>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string endpoint = $"{LumicareConstants.TalentLMSEndpoint}/users/username:{HttpUtility.UrlEncode(username)}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {StringHelpers.Base64Encode(LumicareConstants.TalentLMSAPIKey + ":")}");
                using (var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    // response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    var reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<GetUserResponseModel>(responseData);
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = CommonConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        apiResult.Code = ResultCode.NotFound;
                        apiResult.Message = CommonConstants.MSG_404;
                    }
                }
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<GetUserResponseModel>> GetUserByEmail(string email)
        {
            var apiResult = new ApiResultDto<GetUserResponseModel>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string endpoint = $"{LumicareConstants.TalentLMSEndpoint}/users/email:{email}";

                var request = new HttpRequestMessage(
                           HttpMethod.Get,
                           endpoint);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {StringHelpers.Base64Encode(LumicareConstants.TalentLMSAPIKey + ":")}");
                using (var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    // response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    var reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<GetUserResponseModel>(responseData);
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = CommonConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        apiResult.Code = ResultCode.NotFound;
                        apiResult.Message = CommonConstants.MSG_404;
                    }
                }
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        public async Task<ApiResultDto<UserSignupResponse>> SignupUser(UserSignupModel signupModel)
        {
            var apiResult = new ApiResultDto<UserSignupResponse>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string endpoint = $"{LumicareConstants.TalentLMSEndpoint}/usersignup";
                var requestContent = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("first_name", signupModel.FirstName),
                    new KeyValuePair<string, string>("last_name", signupModel.LastName),
                    new KeyValuePair<string, string>("email", signupModel.Email),
                    new KeyValuePair<string, string>("login", signupModel.Login),
                    new KeyValuePair<string, string>("password", signupModel.Password),
                };
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new FormUrlEncodedContent(requestContent)
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {StringHelpers.Base64Encode(LumicareConstants.TalentLMSAPIKey + ":")}");
                using (var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    // response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    var reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseObj = JsonConvert.DeserializeObject<UserSignupResponse>(responseData);

                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = CommonConstants.MSG_200;
                        apiResult.Data = responseObj;
                    }
                }
                return apiResult;

            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        #endregion

        #region Course

        public async Task<ApiResultDto<string>> AddUserToCourse(AddUserToCourseModel addUserToCourseModel)
        {
            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.MSG_400
            };

            try
            {
                string endpoint = $"{LumicareConstants.TalentLMSEndpoint}/addusertocourse";
                var requestContent = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("user_id", addUserToCourseModel.UserId),
                    new KeyValuePair<string, string>("course_id", addUserToCourseModel.CourseId),
                    new KeyValuePair<string, string>("role", addUserToCourseModel.Role)
                };
                var request = new HttpRequestMessage(
                           HttpMethod.Post,
                           endpoint)
                {
                    Content = new FormUrlEncodedContent(requestContent)
                };
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {StringHelpers.Base64Encode(LumicareConstants.TalentLMSAPIKey + ":")}");
                using (var response = await httpClient.SendAsync(request,
                           HttpCompletionOption.ResponseHeadersRead))
                {
                    // response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    // Convert stream to string
                    var reader = new StreamReader(stream);
                    string responseData = reader.ReadToEnd();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        apiResult.Code = ResultCode.OK;
                        apiResult.Message = CommonConstants.MSG_200;
                    }
                }
                apiResult.Code = ResultCode.OK;
                apiResult.Message = CommonConstants.MSG_200;
                return apiResult;
            }
            catch (Exception ex)
            {
                apiResult.Message = $"{ex.Message} - {ex.StackTrace}";
                return apiResult;
            }
        }

        #endregion

    }
}
