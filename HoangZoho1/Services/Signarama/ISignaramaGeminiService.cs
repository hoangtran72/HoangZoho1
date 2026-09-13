using HoangZoho1.Models.Common;
using HoangZoho1.Models.Signarama.Gemini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Signarama
{

    public interface ISignaramaGeminiService
    {

        Task<ApiResultDto<UploadFileResponse>> UploadFile(string filePath, string fileName);

        Task<ApiResultDto<GenerateContentResponse>>
            GenerateContent(GenerateContentRequest contentRequest);

    }

}
