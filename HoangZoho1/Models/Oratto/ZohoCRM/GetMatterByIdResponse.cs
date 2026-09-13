using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.ZohoCRM
{

    public class GetMatterByIdResponse
    {
        public MatterData[] data { get; set; }
    }

    public class MatterData
    {

        public Owner Owner { get; set; }

        public string Email { get; set; }

        public string Client_Preferred_Phone { get; set; }

        public string Solicitor_Matter_Update_Notes1 { get; set; }

        public string Matter_Status_Solicitor { get; set; }

        public DateTime? Solicitor_Email_Click_Date_Time { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public string Name { get; set; }

        public Matter_Sub_Categories Matter_Sub_Categories { get; set; }

        public string Lead_Preferred_Communication { get; set; }

        public DateTime? Solicitor_Last_Updated_Date_Time { get; set; }

        public string Currency { get; set; }

        public string Client_Company_Name { get; set; }

        public decimal? Days_since_matter_assigned { get; set; }

        public string id { get; set; }

        public Leads_Referred Leads_Referred { get; set; }

        public DateTime? Matter_Owner_Assigned_Date_Time { get; set; }

        public DateTime? Created_Time { get; set; }

        public string Client_City { get; set; }

        public string Oratto_Lawyer_Rejection_Reason { get; set; }

        public bool? Lawyer_Rejected { get; set; }

        public string Lead_Preferred_Time_to_Speak { get; set; }

        public string Matter_Reference_Number { get; set; }

        public decimal? Hours_to_Solicitor_First_Client_Contact { get; set; }

        public Created_By Created_By { get; set; }

        public string Oratto_New_Solicitor_Email { get; set; }

        public string Client_Conversation_Transcript { get; set; }

        public decimal? Risk_Weighted_Matter_Value { get; set; }

        public string Solicitor_Law_Firm { get; set; }

        public string Individual_Company { get; set; }

        public DateTime? Solicitor_First_Client_Contact_DateTime { get; set; }

        public string Not_Interested_Reason { get; set; }

        public string Law_Firm_Legal_Name { get; set; }

        public string Matter_Lost_Reason { get; set; }

        public Review_Process review_process { get; set; }

        public Referred_Solicitors Referred_Solicitors { get; set; }

        public decimal? Est_Client_Ability_to_Pay { get; set; }

        public string New_Solicitor_Email { get; set; }

        public string First_Name { get; set; }

        public decimal? Est_Client_Willingness_to_Pay { get; set; }

        public Modified_By Modified_By { get; set; }

        public decimal? Duration_to_Close_the_matter { get; set; }

        public string Weekly_Notification_Date { get; set; }

        public DateTime? Date_Time_3 { get; set; }

        public DateTime? Date_Time_4 { get; set; }

        public object Matter_Converted_Client_Instructed_DateTime { get; set; }

        public string Matters_Lost_Reason { get; set; }

        public DateTime? Modified_Time { get; set; }

        public DateTime? Status_Changed_Date_and_Time { get; set; }

        public string Lead_Full_Name { get; set; }

        public object Client_Fixed_Line_Phone_No { get; set; }

        public object Est_Prob_of_Conversion { get; set; }

        public object Unsubscribed_Time { get; set; }

        public decimal? Hours_until_Solicitor_clicked_email_link { get; set; }

        public decimal? Days_since_solicitor_last_updated { get; set; }

        public string Oratto_New_Solicitor_Handover_Notes { get; set; }

        public DateTime? Matter_Closed_Time { get; set; }

        public decimal? Hours_to_Solicitor_first_touch { get; set; }

        public string Last_Name { get; set; }

        public string Client_Country { get; set; }

        public string Matter_Summary_For_Solicitor1 { get; set; }

        public decimal? Est_Prof_Fee_Potential { get; set; }

        public bool? System_Fields { get; set; }

        public decimal? Formula_1 { get; set; }

        public decimal? Hours_to_Matter_Conversion { get; set; }

        public object[] Tag { get; set; }

        public string Client_Mobile_Phone_No { get; set; }

        public string Confirm_submitting_matter_for_Oratto_Verification1 { get; set; }

        public DateTime? Lead_Sent_to_Solicitor_Date { get; set; }

        public DateTime? Solicitor_First_Touch_Date_Time { get; set; }

        public string Solicitor_Matter_Status { get; set; }

    }

    public class Owner
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

    public class Matter_Sub_Categories
    {

        public string name { get; set; }

        public string id { get; set; }

    }

    public class Leads_Referred
    {

        public string name { get; set; }

        public string id { get; set; }

    }

    public class Created_By
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

    public class Referred_Solicitors
    {

        public string name { get; set; }

        public string id { get; set; }

    }

    public class Modified_By
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

    public class Lead_Owner1
    {

        public string name { get; set; }

        public string id { get; set; }

    }

}
