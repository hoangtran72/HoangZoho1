using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.ZohoCRM
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class EmailForUpdation
    {

        public string Status { get; set; }

        public int Prompt_Tokens { get; set; }

        public int Completion_Tokens { get; set; }

        public string Content_1 { get; set; }

        public string Content_2 { get; set; }
        
        public string Content_3 { get; set; }

        public string Content_4 { get; set; }

        public string Content_5 { get; set; }

        public string Content_6 { get; set; }
        
        public string Content_7 { get; set; }

        public string Content_8 { get; set; }

        public string Content_9 { get; set; }

        public string Content_10 { get; set; }

    }

}
