using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Lumicare.TalentLMS
{

    public class UserSignupResponse
    {
        public int? id { get; set; }

        public string login { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string email { get; set; }

        public string restrict_email { get; set; }

        public string user_type { get; set; }

        public string timezone { get; set; }

        public string language { get; set; }

        public string status { get; set; }

        public string level { get; set; }

        public string points { get; set; }

        public string created_on { get; set; }

        public string last_updated { get; set; }

        public int? last_updated_timestamp { get; set; }

        public string avatar { get; set; }

        public object bio { get; set; }

        public string login_key { get; set; }

    }

}
