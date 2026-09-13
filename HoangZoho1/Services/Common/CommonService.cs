using System.IO;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using HoangZoho1.Models.Common;
using HoangZoho1.Constants;

namespace HoangZoho1.Services.Common
{
    public class CommonService : ICommonService
    {

        public async Task<ApiResultDto<string>> DownloadFileFromUrl
            (string url, string filePath)
        {

            var apiResult = new ApiResultDto<string>()
            {
                Code = ResultCode.BadRequest,
                Message = CommonConstants.DFFU_400
            };

            try
            {
                using HttpClient client = new HttpClient();

                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await response.Content.CopyToAsync(fs);
                    }

                    apiResult.Code = ResultCode.OK;
                    apiResult.Message = CommonConstants.DFFU_200;

                }

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
