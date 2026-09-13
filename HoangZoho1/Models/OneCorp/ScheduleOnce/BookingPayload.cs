using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ScheduleOnce
{

    public class BookingPayload
    {

        public string id { get; set; }

        [JsonProperty(PropertyName = "object")]
        public string _object { get; set; }

        public DateTime? creation_time { get; set; }

        public string type { get; set; }

        public string api_version { get; set; }

        public BookingData data { get; set; }

    }

    public class BookingData
    {

        [JsonProperty(PropertyName = "object")]

        public string _object { get; set; }

        public string id { get; set; }

        public string tracking_id { get; set; }

        public string subject { get; set; }

        public string status { get; set; }

        public DateTime? creation_time { get; set; }

        public DateTime? starting_time { get; set; }

        public string customer_timezone { get; set; }

        public DateTime? last_updated_time { get; set; }

        public string owner { get; set; }

        public decimal? duration_minutes { get; set; }

        public Virtual_Conferencing virtual_conferencing { get; set; }

        public string location_description { get; set; }

        public string rescheduled_booking_id { get; set; }

        public string cancel_reschedule_url { get; set; }

        public Cancel_Reschedule_Information cancel_reschedule_information { get; set; }

        public Form_Submission form_submission { get; set; }

        public string booking_page { get; set; }

        public string master_page { get; set; }

        public string event_type { get; set; }

        public External_Calendar external_calendar { get; set; }

        public string conversation { get; set; }

    }

    public class Virtual_Conferencing
    {

        public string join_url { get; set; }

    }

    public class Cancel_Reschedule_Information
    {

        public string reason { get; set; }

        public string actioned_by { get; set; }

        public string user_id { get; set; }

    }

    public class Form_Submission
    {

        public string name { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string mobile_phone { get; set; }

        public string note { get; set; }

        public string company { get; set; }

        public string[] guests { get; set; }

        public Custom_Fields[] custom_fields { get; set; }

    }

    public class Custom_Fields
    {

        public string name { get; set; }

        public string value { get; set; }

    }

    public class External_Calendar
    {

        public string type { get; set; }

        public string name { get; set; }

        public string id { get; set; }

        public string event_id { get; set; }

    }

}

