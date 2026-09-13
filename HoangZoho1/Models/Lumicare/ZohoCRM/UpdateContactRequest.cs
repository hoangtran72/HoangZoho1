using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Lumicare.ZohoCRM
{
    public class UpdateContactRequest
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LMS_User_Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LMS_Username { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LMS_Email_Type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string User_Creation_Date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? Send_LMS_Email { get; set; }

    }
}
