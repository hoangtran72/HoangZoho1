using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneBudget.ZohoCRM
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class BookingForCreation
    {

        public string Customer_Name { get; set; }

        public string Customer_First_Name { get; set; }

        public string Customer_Last_Name { get; set; }

        public string Booking_Status { get; set; }

        public string Customer_Guests { get; set; }

        public string Event_Type { get; set; }

        public string Customer_Additional_Information { get; set; }

        public string Rescheduled_Booking_Id { get; set; }

        public string Booking_Page { get; set; }

        public string Customer_Company { get; set; }

        public string Booking_Subject { get; set; }

        public string Customer_Phone { get; set; }

        public string Cancel_Reschedule_Information { get; set; }

        public string Customer_Email { get; set; }

        public string Booking_Join_URL { get; set; }

        public string Booking_Join_Text { get; set; }

        public int? Booking_Duration_minutes { get; set; }

        public string External_Calendar { get; set; }

        public string Booking_Created_Time { get; set; }

        public string Booking_Owner { get; set; }

        public string Booking_Owner_Name { get; set; }

        public string Booking_Owner_First_Name { get; set; }

        public string Booking_Owner_Last_Name { get; set; }

        public string Booking_Owner_Email { get; set; }

        public string Tracking_Id { get; set; }

        public string Cancel_Reschedule_URL { get; set; }

        public string Conversation { get; set; }

        public string Booking_Starting_Time { get; set; }

        public string Booking_Last_Updated_Time { get; set; }

        public string Customer_Mobile { get; set; }

        public string Booking_Id { get; set; }

        public string Master_Page { get; set; }

        public string Booking_Location { get; set; }

        public string Customer_Note_1 { get; set; }

        public string Agent_Booking_Start_Time { get; set; }

        public string Agent_Time_Zone { get; set; }

        public string Client_Booking_Start_Time { get; set; }

        public string Client_Time_Zone { get; set; }

        public string Owner { get; set; }

    }

}
