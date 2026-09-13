using System;

namespace HoangZoho1.Models.OneBudget.ZohoCRM
{

    public class GetUserByIdResponse
    {
        public User[] users { get; set; }
    }

    public class User
    {

        public string country { get; set; }

        public Role role { get; set; }

        public string city { get; set; }

        public string sort_order_preference { get; set; }

        public string language { get; set; }

        public bool? microsoft { get; set; }

        public string locale { get; set; }

        public bool? Isonline { get; set; }

        public Modified_By Modified_By { get; set; }

        public object shift_effective_from { get; set; }

        public string street { get; set; }

        public object current_shift { get; set; }

        public string alias { get; set; }

        public string id { get; set; }

        public string state { get; set; }

        public string fax { get; set; }

        public string country_locale { get; set; }

        public bool? sandboxDeveloper { get; set; }

        public string first_name { get; set; }

        public string email { get; set; }

        public string zip { get; set; }

        public string decimal_separator { get; set; }

        public object status_reason__s { get; set; }

        public DateTime? created_time { get; set; }

        public DateTime? Modified_Time { get; set; }

        public object website { get; set; }

        public string time_format { get; set; }

        public object next_shift { get; set; }

        public int? offset { get; set; }

        public Profile profile { get; set; }

        public string mobile { get; set; }

        public string last_name { get; set; }

        public string time_zone { get; set; }

        public string number_separator { get; set; }

        public Created_By created_by { get; set; }

        public string zuid { get; set; }

        public bool? confirm { get; set; }

        public string full_name { get; set; }

        public string Booking_Link { get; set; }

        public string phone { get; set; }

        public string dob { get; set; }

        public string date_format { get; set; }

        public string category { get; set; }

        public bool? Pearl_Tag { get; set; }

        public string status { get; set; }

        public object type__s { get; set; }

    }

    public class Role
    {

        public string name { get; set; }

        public string id { get; set; }

    }

    public class Profile
    {

        public string name { get; set; }

        public string id { get; set; }

    }

}
