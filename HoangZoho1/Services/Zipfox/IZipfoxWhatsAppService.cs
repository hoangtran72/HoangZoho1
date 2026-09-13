using HoangZoho1.Models.Common;
using HoangZoho1.Models.Zipfox.WhatsApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Services.Zipfox
{

    public interface IZipfoxWhatsAppService
    {

        Task<ApiResultDto<SendWhatsAppMessageResponse>> SendWhatsAppMessageByTemplate(string phoneNumberId, SendWhatsAppMessageByTemplateRequest whatsAppRequest);

        Task<ApiResultDto<SendWhatsAppMessageResponse>> SendWhatsAppMessageByText(string phoneNumberId, SendWhatsAppMessageByTextRequest whatsAppRequest);

        Task<ApiResultDto<GetWhatsAppTemplatesResponse>> GetWhatsAppTemplates();

    }

}
