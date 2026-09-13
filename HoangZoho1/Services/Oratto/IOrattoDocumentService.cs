using HoangZoho1.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Oratto
{

    public interface IOrattoDocumentService
    {

        ApiResultDto<List<string>> SplitPdfFile(string filePath);

        ApiResultDto<string> ConvertDocxToPdf(string filePath);

        ApiResultDto<string> ConvertPptxToPdf(string filePath);

    }

}
