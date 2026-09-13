using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.ZohoCRM
{

    public class GetSolicitorByIdResponse
    {

        public SolicitorData[] data { get; set; }

    }

    public class SolicitorData
    {

        public string Agreement_Signed { get; set; }

        public Owner Owner { get; set; }

        public string Email { get; set; }

        public string Matter_Expertise { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public bool Oratto_Profile_Active { get; set; }

        public object SRA_Number { get; set; }

        public object Unsubscribed_Mode { get; set; }

        public string Street { get; set; }

        public string Postal_Code { get; set; }

        public string id { get; set; }

        public bool? Legal_Aid { get; set; }

        public bool? approved { get; set; }

        // public string Matter_Experts { get; set; }

        public object Leads_Referred { get; set; }

        public string Mass_Update { get; set; }

        public string Main_Testimonial { get; set; }

        public DateTime? Created_Time { get; set; }

        public decimal? Lead_NPS_Score { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string Note { get; set; }

        public string Country { get; set; }

        public Created_By Created_By { get; set; }

        public string Solicitor_Testimonials2 { get; set; }

        public string Hide_Unhide { get; set; }

        public string Customer_Books_ID { get; set; }

        public bool? Oratto_Lawyer_Shortlist { get; set; }

        public string Salutation { get; set; }

        public string Linkedin_Profile { get; set; }

        public string First_Name { get; set; }

        public string Full_Name { get; set; }

        public string Record_Image { get; set; }

        public Modified_By Modified_By { get; set; }

        public string Assistant_Name { get; set; }

        public string Solicitor_Summary_Profile2 { get; set; }

        public string Phone { get; set; }

        public Account_Name Account_Name { get; set; }

        public DateTime? Qualified_Date { get; set; }

        public decimal? Engaged_Client_NPS_Score { get; set; }

        public DateTime? Modified_Time { get; set; }

        public DateTime? Date_of_Birth { get; set; }

        public string Billing_POC_Email { get; set; }

        public string Title { get; set; }

        public string Billing_POC_Name { get; set; }

        public string Mobile { get; set; }

        public string smsmagic4__LeadIdCPY { get; set; }

        public string smsmagic4__Plain_Phone { get; set; }

        public string Law_Society_Profile { get; set; }

        public string Weekly_Email_Date { get; set; }

        public string Last_Name { get; set; }

        public string Sub_Categories { get; set; }

        public object[] Tag { get; set; }

        public string House_Number { get; set; }

        public bool? Marketing_Emails_Opt_Out { get; set; }

        public string Oratto_Lawyer { get; set; }

    }

    public class Approval
    {
        public bool _delegate { get; set; }
        public bool approve { get; set; }
        public bool reject { get; set; }
        public bool resubmit { get; set; }
    }

    public class Review_Process
    {
        public bool approve { get; set; }
        public bool reject { get; set; }
        public bool resubmit { get; set; }
    }

    public class Account_Name
    {
        public string name { get; set; }
        public string id { get; set; }
    }


}
