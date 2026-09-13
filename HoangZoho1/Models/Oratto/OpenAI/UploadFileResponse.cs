using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.OpenAI
{

    public class UploadFileResponse
    {

        [JsonProperty("object")]
        public string _object { get; set; }
        
        public string id { get; set; }
        
        public string purpose { get; set; }
        
        public string filename { get; set; }
        
        public decimal? bytes { get; set; }
        
        public decimal? created_at { get; set; }
        
        public string status { get; set; }
        
        public object status_details { get; set; }
    
    }


}
