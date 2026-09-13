using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneBudget.ZohoCRM
{

    public class SearchBookingResponse
    {
        
        public BookingDetails[] data { get; set; }
        
        public Info info { get; set; }
    
    }

    public class Info
    {
        
        public int per_page { get; set; }
        
        public int count { get; set; }
        
        public int page { get; set; }
        
        public bool more_records { get; set; }
    
    }

    public class BookingDetails
    {

        public Owner Owner { get; set; }

        public string Customer_Name { get; set; }

        public string Booking_Status { get; set; }

        public string Customer_Guests { get; set; }

        public string Name { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public string Event_Type { get; set; }

        public string Customer_Additional_Information { get; set; }

        public DateTime? Booking_Last_Updated_Time { get; set; }

        public string id { get; set; }

        public string Rescheduled_Booking_Id { get; set; }

        public string Booking_Page { get; set; }

        public string Customer_Company { get; set; }

        public string Booking_Subject { get; set; }

        public string Customer_Phone { get; set; }

        public DateTime? Created_Time { get; set; }

        public string Cancel_Reschedule_Information { get; set; }

        public string Customer_Email { get; set; }

        public string Booking_Join_URL { get; set; }

        public int? Booking_Duration_minutes { get; set; }

        public string External_Calendar { get; set; }

        public Created_By Created_By { get; set; }

        public DateTime? Booking_Created_Time { get; set; }

        public string Booking_Owner { get; set; }

        public string Tracking_Id { get; set; }

        public string Cancel_Reschedule_URL { get; set; }

        public string Conversation { get; set; }

        public DateTime? Booking_Starting_Time { get; set; }

        public Modified_By Modified_By { get; set; }

        public string Customer_Mobile { get; set; }

        public string Booking_Id { get; set; }

        public DateTime? Modified_Time { get; set; }

        public string Master_Page { get; set; }

        public string Booking_Location { get; set; }

        public string Customer_Note { get; set; }

    }

    public class Owner
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

    public class Created_By
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

    public class Modified_By
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

}
