using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Lumicare.TalentLMS
{

    public class CourseModel
    {

        public string id { get; set; }

        public string name { get; set; }

        public string code { get; set; }

        public object category_id { get; set; }

        public string description { get; set; }

        public string price { get; set; }

        public string status { get; set; }

        public string creation_date { get; set; }

        public string last_update_on { get; set; }

        public string creator_id { get; set; }

        public string hide_from_catalog { get; set; }

        public string time_limit { get; set; }

        public object start_datetime { get; set; }

        public object expiration_datetime { get; set; }

        public object level { get; set; }

        public string shared { get; set; }

        public string shared_url { get; set; }

        public string avatar { get; set; }

        public string big_avatar { get; set; }

        public string certification { get; set; }

        public string certification_duration { get; set; }

        public CourseUser[] users { get; set; }

        public Unit[] units { get; set; }

        public string[] rules { get; set; }

        public object[] prerequisites { get; set; }

        public object[] prerequisite_rule_sets { get; set; }

    }

    public class CourseUser
    {

        public string id { get; set; }

        public string name { get; set; }

        public string role { get; set; }

        public string enrolled_on { get; set; }

        public string enrolled_on_timestamp { get; set; }

        public string completed_on { get; set; }

        public string completed_on_timestamp { get; set; }

        public string completion_percentage { get; set; }

        public string expired_on { get; set; }

        public object expired_on_timestamp { get; set; }

        public string total_time { get; set; }

        public int total_time_seconds { get; set; }

    }

    public class Unit
    {

        public string id { get; set; }

        public string type { get; set; }

        public string name { get; set; }

        public string url { get; set; }

    }

}
