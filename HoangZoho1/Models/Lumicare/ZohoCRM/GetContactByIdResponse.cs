using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Lumicare.ZohoCRM
{

    public class GetContactByIdResponse
    {
        public ContactDetails[] data { get; set; }
    }

    public class ContactDetails
    {

        public Owner Owner { get; set; }

        public string Email { get; set; }

        public object Suburb { get; set; }

        public object Visitor_Score { get; set; }

        public string Country_select { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public object Institution_name { get; set; }

        public object Department { get; set; }

        public object Street { get; set; }

        public string id { get; set; }

        public string LMS_Username { get; set; }

        public object Reporting_To { get; set; }

        public object Enrich_Status__s { get; set; }

        public object First_Visited_URL { get; set; }

        public object Days_Visited { get; set; }

        public object Conference_attended { get; set; }

        public DateTime? Created_Time { get; set; }

        public object Postcode { get; set; }

        public object Specialty { get; set; }

        public string Company_Name { get; set; }

        public object Last_Visited_Time { get; set; }

        public Created_By Created_By { get; set; }

        public string Secondary_Email { get; set; }

        public string LMS_Email_Type { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public object Vendor_Name { get; set; }

        public object Number_Of_Chats { get; set; }

        public object Account_Names { get; set; }

        public object Phone_extension { get; set; }

        public bool Onboarding_Course_Finished { get; set; }

        public object Average_Time_Spent_Minutes { get; set; }

        public bool Send_LMS_Email { get; set; }

        public string Salutation { get; set; }

        public string First_Name { get; set; }

        public string Full_Name { get; set; }

        public Modified_By Modified_By { get; set; }

        public string Phone { get; set; }

        public object Account_Name { get; set; }

        public bool? Email_Opt_Out { get; set; }

        public object End_customer_Category { get; set; }

        public DateTime? Modified_Time { get; set; }

        public object Unsubscribed_Time { get; set; }

        public string Title { get; set; }

        public string Mobile { get; set; }

        public string LMS_User_Id { get; set; }

        public object First_Visited_Time { get; set; }

        public object State_or_Province { get; set; }

        public string Last_Name { get; set; }

        public object Referrer { get; set; }

        public object[] Tag { get; set; }

        public object Last_Enriched_Time__s { get; set; }

    }

}
