using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Lumicare.ZohoCRM
{

    public class SearchContactsResponse
    {

        public ContactData[] data { get; set; }

        public Info info { get; set; }

    }

    public class Info
    {
        public int per_page { get; set; }
        public int count { get; set; }
        public int page { get; set; }
        public string sort_by { get; set; }
        public string sort_order { get; set; }
        public bool more_records { get; set; }
    }

    public class ContactData
    {

        public Owner Owner { get; set; }

        public string Email { get; set; }

        public object Suburb { get; set; }

        public object Visitor_Score { get; set; }

        public string Country_select { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public object Institution_name { get; set; }

        public object Department { get; set; }

        public string Street { get; set; }

        public string id { get; set; }

        public object Reporting_To { get; set; }

        public object Enrich_Status__s { get; set; }

        public object First_Visited_URL { get; set; }

        public object Days_Visited { get; set; }

        public object Conference_attended { get; set; }

        public DateTime? Created_Time { get; set; }

        public string Postcode { get; set; }

        public object Specialty { get; set; }

        public object Last_Visited_Time { get; set; }

        public Created_By Created_By { get; set; }

        public object Secondary_Email { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public object Vendor_Name { get; set; }

        public object Number_Of_Chats { get; set; }

        public object Phone_extension { get; set; }

        public bool? Onboarding_Course_Finished { get; set; }

        public object Average_Time_Spent_Minutes { get; set; }

        public string Salutation { get; set; }

        public string First_Name { get; set; }

        public string Full_Name { get; set; }

        public object Record_Image { get; set; }

        public Modified_By Modified_By { get; set; }

        public string Phone { get; set; }

        public Account_Name Account_Name { get; set; }

        public string End_customer_Category { get; set; }

        public DateTime? Modified_Time { get; set; }

        public string Title { get; set; }

        public string Mobile { get; set; }

        public string LMS_User_Id { get; set; }

        public string State_or_Province { get; set; }

        public string Last_Name { get; set; }

        public object Referrer { get; set; }

        public string[] Tag { get; set; }

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

    public class Account_Name
    {

        public string name { get; set; }

        public string id { get; set; }

    }

}
