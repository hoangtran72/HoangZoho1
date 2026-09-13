using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.Gemini
{

    public class UploadFileResponse
    {

        public FileData file { get; set; }
    
    }

    public class FileData
    {

        public string name { get; set; }
        
        public string mimeType { get; set; }
        
        public string sizeBytes { get; set; }
        
        public DateTime? createTime { get; set; }
        
        public DateTime? updateTime { get; set; }
        
        public string expirationTime { get; set; }
        
        public string sha256Hash { get; set; }
        
        public string uri { get; set; }
        
        public string state { get; set; }
    
    }

}
