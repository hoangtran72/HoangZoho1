using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.ZohoCRM
{

    public class SearchContactsByPhoneResponse
    {

        public ContactData[] data { get; set; }

        public Info info { get; set; }

    }

    public class ContactData
    {

        public string Account { get; set; }

        public Owner Owner { get; set; }

        public string GCLID { get; set; }

        public string Mailing_State { get; set; }

        public object Other_Country { get; set; }

        public object Department { get; set; }

        public string id { get; set; }

        public string Status { get; set; }

        public Approval approval { get; set; }

        public decimal? Cost_per_Click { get; set; }

        public string First_Visited_URL { get; set; }

        public DateTime? Created_Time { get; set; }

        public string Ad_Click_Date { get; set; }

        public string Country { get; set; }

        public Created_By Created_By { get; set; }

        public object Phone_2 { get; set; }

        public bool? Quote_Received { get; set; }

        public object Ad { get; set; }
        public string Description { get; set; }

        public bool? Seller_Replied { get; set; }

        public Review_Process review_process { get; set; }

        public string Other_Zip { get; set; }

        public string Website { get; set; }

        public string Mailing_Street { get; set; }

        public string Salutation { get; set; }

        public string Full_Name { get; set; }

        public string zohoworkdriveforcrm__Workdrive_Folder_URL { get; set; }

        public string Skype_ID { get; set; }

        public Account_Name Account_Name { get; set; }

        public bool? Email_Opt_Out { get; set; }

        public string Custom_Layout_No { get; set; }

        public string Other_Street { get; set; }

        public string Mobile { get; set; }

        public Layout Layout { get; set; }

        public string Ad_Campaign_Name { get; set; }

        public string Lead_Source { get; set; }

        public string Email { get; set; }

        public bool? Main_Contact { get; set; }

        public string Other_Phone { get; set; }

        public string Other_State { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public string Mailing_Country { get; set; }

        public string Whatsapp { get; set; }

        public string Reporting_To { get; set; }

        public object Days_Visited { get; set; }

        public object Click_Type { get; set; }
        
        public string Other_City { get; set; }

        public string City { get; set; }

        public string Campaign_Status { get; set; }

        public string Home_Phone { get; set; }

        public bool? RFQ_Form_Completed { get; set; }

        public string zohoworkdriveforcrm__Workdrive_Folder_ID { get; set; }

        public string Secondary_Email { get; set; }

        public string Message { get; set; }

        public string Vendor_Name { get; set; }

        public string Mailing_Zip { get; set; }

        public string Twitter { get; set; }

        public bool? Invoice_Received { get; set; }

        public string First_Name { get; set; }

        public string Asst_Phone { get; set; }

        public Modified_By Modified_By { get; set; }

        public string Communication_Stage { get; set; }

        public string Phone { get; set; }

        public DateTime? Modified_Time { get; set; }

        public string Mailing_City { get; set; }

        public string Device_Type { get; set; }

        public string Title { get; set; }

        public string Last_Name { get; set; }

    }

}
