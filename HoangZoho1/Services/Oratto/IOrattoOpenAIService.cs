using HoangZoho1.Models.Common;
using HoangZoho1.Models.Oratto.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Oratto
{

    public interface IOrattoOpenAIService
    {

        Task<ApiResultDto<UploadFileResponse>> UploadFile2OpenAI(string filePath, string fileName, string purpose);

        Task<ApiResultDto<ChatCompletionResponse>> ChatCompletions(ChatCompletionRequest completionRequest);

    }

}
