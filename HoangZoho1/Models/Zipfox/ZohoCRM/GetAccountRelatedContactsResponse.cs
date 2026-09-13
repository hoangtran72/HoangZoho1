using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.ZohoCRM
{

    public class GetAccountRelatedContactsResponse
    {

        public RelatedContactData[] data { get; set; }

        public Info info { get; set; }

    }

    public class RelatedContactData
    {

        public string Account { get; set; }

        public Owner Owner { get; set; }

        public object Products { get; set; }

        public string Email { get; set; }

        public string GCLID { get; set; }

        public object Visitor_Score { get; set; }

        public bool? Main_Contact { get; set; }

        public object Companies { get; set; }

        public object Next_Email_Campaign_Date { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public string Department { get; set; }

        public object[] Main_Category { get; set; }

        public object Ad_Network { get; set; }

        public string id { get; set; }

        public string Whatsapp { get; set; }

        public string WhatsApp_OK { get; set; }

        public object Conversion_Exported_On { get; set; }

        public string Status { get; set; }

        public object[] Sub_Category { get; set; }

        public Approval approval { get; set; }

        public int? Cost_per_Click { get; set; }

        public string Enrich_Status__s { get; set; }

        public object First_Visited_URL { get; set; }

        public object Days_Visited { get; set; }

        public object Click_Type { get; set; }

        public DateTime? Created_Time { get; set; }

        public object Change_Log_Time__s { get; set; }

        public object City { get; set; }

        public object AdGroup_Name { get; set; }

        public object Estimated_Quantity { get; set; }

        public object Ad_Click_Date { get; set; }

        public bool? RFQ_Form_Completed { get; set; }

        public string zohoworkdriveforcrm__Workdrive_Folder_ID { get; set; }

        public string Country { get; set; }

        public object Last_Visited_Time { get; set; }

        public Created_By Created_By { get; set; }

        public string zia_owner_assignment { get; set; }

        public object Secondary_Email { get; set; }

        public bool Quote_Received { get; set; }

        public object Ad { get; set; }

        public object Message { get; set; }

        public object Number_Of_Chats { get; set; }

        public bool? Seller_Replied { get; set; }

        public object Search_Partner_Network { get; set; }

        public Review_Process review_process { get; set; }

        public object Average_Time_Spent_Minutes { get; set; }

        public object Salutation { get; set; }

        public bool? Invoice_Received { get; set; }

        public string First_Name { get; set; }

        public string Full_Name { get; set; }

        public object Conversion_Export_Status { get; set; }

        public int? Cost_per_Conversion { get; set; }

        public Modified_By Modified_By { get; set; }

        public string zohoworkdriveforcrm__Workdrive_Folder_URL { get; set; }

        public object Last_Contact_Date { get; set; }

        public string Phone { get; set; }

        public object Search_Value { get; set; }

        public Account_Name Account_Name { get; set; }

        public object Buyer_Layout_No { get; set; }

        public bool? Email_Opt_Out { get; set; }

        public object Email_Number { get; set; }

        public DateTime? Modified_Time { get; set; }

        public object Keyword { get; set; }

        public object Unsubscribed_Time { get; set; }

        public object Device_Type { get; set; }

        public string Custom_Layout_No { get; set; }

        public string Mobile { get; set; }

        public object Import_Id { get; set; }

        public object Send_Whatsapp { get; set; }

        public object First_Visited_Time { get; set; }

        public string Last_Name { get; set; }

        public Layout Layout { get; set; }

        public object Send_Invitation { get; set; }

        public object Ad_Campaign_Name { get; set; }

        public object Referrer { get; set; }

        public bool Locked__s { get; set; }

        public string Lead_Source { get; set; }

        public object[] Tag { get; set; }

        public object Reason_for_Conversion_Failure { get; set; }

        public object Last_Enriched_Time__s { get; set; }

        public object Product_Not_Found_Description { get; set; }

    }

    public class Info
    {

        public int? per_page { get; set; }

        public int? count { get; set; }

        public int? page { get; set; }

        public bool? more_records { get; set; }

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

    public class Review_Process
    {

        public bool? approve { get; set; }

        public bool? reject { get; set; }

        public bool? resubmit { get; set; }

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

    public class Layout
    {

        public string name { get; set; }

        public string id { get; set; }

    }

}
