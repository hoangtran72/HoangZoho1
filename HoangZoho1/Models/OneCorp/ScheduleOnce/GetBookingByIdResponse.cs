using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ScheduleOnce
{

    public class GetBookingByIdResponse
    {

        public string _object { get; set; }

        public string id { get; set; }

        public string tracking_id { get; set; }

        public string subject { get; set; }

        public string status { get; set; }

        public bool? in_trash { get; set; }

        public DateTime? creation_time { get; set; }

        public DateTime? starting_time { get; set; }

        public DateTime? last_updated_time { get; set; }

        public string owner { get; set; }
        
        public decimal? duration_minutes { get; set; }
        
        public Virtual_Conferencing virtual_conferencing { get; set; }
        
        public string location_description { get; set; }

        public string rescheduled_booking_id { get; set; }
        
        public Cancel_Reschedule_Information cancel_reschedule_information { get; set; }

        public string cancel_reschedule_url { get; set; }

        public string customer_timezone { get; set; }
        
        public Form_Submission form_submission { get; set; }
        
        public string booking_page { get; set; }
        
        public string master_page { get; set; }
        
        public string event_type { get; set; }
        
        public External_Calendar external_calendar { get; set; }
        
        public string conversation { get; set; }
        
        public Utm_Params utm_params { get; set; }

    }

    public class Utm_Params
    {

        public string source { get; set; }
        
        public string medium { get; set; }
        
        public string campaign { get; set; }
        
        public string term { get; set; }
        
        public string content { get; set; }
    
    }

}
