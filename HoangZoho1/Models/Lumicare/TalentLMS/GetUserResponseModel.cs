using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Lumicare.TalentLMS
{

    public class GetUserResponseModel
    {

        public string id { get; set; }

        public string login { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string email { get; set; }

        public string restrict_email { get; set; }

        public string user_type { get; set; }

        public string timezone { get; set; }

        public string language { get; set; }

        public string status { get; set; }

        public string deactivation_date { get; set; }

        public string level { get; set; }

        public string points { get; set; }

        public string created_on { get; set; }

        public string last_updated { get; set; }

        public string last_updated_timestamp { get; set; }

        public string avatar { get; set; }

        public string bio { get; set; }

        public string login_key { get; set; }

        public UserCourse[] courses { get; set; }

        public object[] branches { get; set; }

        public object[] groups { get; set; }

        public object[] certifications { get; set; }

        public object[] badges { get; set; }

    }

    public class UserCourse
    {

        public string id { get; set; }

        public string name { get; set; }

        public string role { get; set; }

        public string enrolled_on { get; set; }

        public string enrolled_on_timestamp { get; set; }

        public string completed_on { get; set; }

        public object completed_on_timestamp { get; set; }

        public string completion_status { get; set; }

        public string completion_status_formatted { get; set; }

        public string completion_percentage { get; set; }

        public string expired_on { get; set; }

        public object expired_on_timestamp { get; set; }

        public string total_time { get; set; }

        public int total_time_seconds { get; set; }

        public string last_accessed_unit_url { get; set; }

    }

}
