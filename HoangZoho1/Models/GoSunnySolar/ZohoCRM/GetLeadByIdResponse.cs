using HoangZoho1.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoSunnySolar.ZohoCRM
{

    public class GetLeadByIdResponse
    {

        public LeadDetail[] data { get; set; }

    }

    public class LeadDetail
    {

        public Owner Owner { get; set; }

        public string Homeowner { get; set; }

        public string Email { get; set; }

        public string currency_symbol { get; set; }

        public string Street_Number { get; set; }

        public object Visitor_Score { get; set; }

        public object Sales_Contract { get; set; }

        public object gdriveextension__Drive_Folder_ID { get; set; }

        public string GHL_Link { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public string Street_Type { get; set; }

        public string Appointment_Call_Notes { get; set; }

        public string Bill_Amount { get; set; }

        public object Unsubscribed_Mode { get; set; }

        public int Exchange_Rate { get; set; }

        public DateTime? Lead_TIme_Stamp { get; set; }

        public string Currency { get; set; }

        public string Street { get; set; }

        public string Zip_Code { get; set; }

        public string id { get; set; }

        public Approval approval { get; set; }

        public string First_Visited_URL { get; set; }

        public object Days_Visited { get; set; }

        public DateTime? Created_Time { get; set; }

        public string Gift_Card_to_Referrer { get; set; }

        public string City { get; set; }

        public object Gift_Card_Amount { get; set; }

        public string gdriveextension__OldDrive_Folder_ID { get; set; }

        public string Sales_Person { get; set; }

        public string State { get; set; }

        public DateTime? Last_Visited_Time { get; set; }

        public Created_By Created_By { get; set; }

        public decimal? Number_Of_Chats { get; set; }

        public decimal? Average_Time_Spent_Minutes { get; set; }

        public string Salutation { get; set; }

        public string First_Name { get; set; }

        public string Full_Name { get; set; }

        public string Lead_Status { get; set; }

        public string Phone { get; set; }

        public bool? Email_Opt_Out { get; set; }

        public string gdriveextension__Drive_URL { get; set; }

        public object Referred_By { get; set; }

        public object Unsubscribed_Time { get; set; }

        public string Appointment_Setter { get; set; }

        public object ACN { get; set; }

        public string smsmagic4__LeadIdCPY { get; set; }

        public string smsmagic4__Plain_Phone { get; set; }

        public DateTime? First_Visited_Time { get; set; }

        public string Last_Name { get; set; }

        public string Referrer { get; set; }

        public string Lead_Source { get; set; }

        public ModuleTag[] Tag { get; set; }

    }

    public class ModuleTag
    {

        public string name { get; set; }

        public string id { get; set; }

        public string color_code { get; set; }

    }

    public class Owner
    {

        public string name { get; set; }
        
        public string id { get; set; }
        
        public string email { get; set; }
    
    }

    public class Approval
    {

        public bool? _delegate { get; set; }

        public bool? approve { get; set; }

        public bool? reject { get; set; }

        public bool? resubmit { get; set; }

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

    }

    public class Review_Process
    {

        public bool? approve { get; set; }

        public bool? reject { get; set; }

        public bool? resubmit { get; set; }

    }

}
