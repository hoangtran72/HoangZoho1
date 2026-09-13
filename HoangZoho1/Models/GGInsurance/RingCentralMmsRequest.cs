using System.Collections.Generic;

namespace HoangZoho1.Models.GGInsurance
{
    public class RingCentralMmsRequest
    {

        public string AccessToken { get; set; }

        public string FromNumber { get; set; }

        public string ToNumber { get; set; }

        public string Text { get; set; }

        public string TokenId { get; set; }

        public List<MmsAttachmentDto> Attachments { get; set; }
        
    }

    public class MmsAttachmentDto
    {

        public string Name { get; set; }

        public string Base64 { get; set; }

        public long Size { get; set; }

    }
}
