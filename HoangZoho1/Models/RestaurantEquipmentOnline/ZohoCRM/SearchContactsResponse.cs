using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class SearchContactsResponse
    {
        public ContactData[] data { get; set; }
        public SearchInfo info { get; set; }
    }

    public class ContactData
    {

        public Owner Owner { get; set; }

        public string Agile_ID { get; set; }

        public string Email { get; set; }

        public string GCLID { get; set; }

        public string Xero_ID { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public string Department { get; set; }

        public float? Exchange_Rate { get; set; }

        public string Currency { get; set; }

        public string Billing_Country { get; set; }

        public string Ad_Network { get; set; }

        public string id { get; set; }

        public string Conversion_Exported_On { get; set; }

        public float? Cost_per_Click { get; set; }

        public string Billing_Street { get; set; }

        public string First_Visited_URL { get; set; }

        public string Days_Visited { get; set; }

        public string Click_Type { get; set; }

        public DateTime? Created_Time { get; set; }

        public string Billing_Code { get; set; }

        public string AdGroup_Name { get; set; }

        public string Shopify_ID { get; set; }

        public string Home_Phone { get; set; }

        public string Shipping_City { get; set; }

        public string Shipping_Country { get; set; }

        public string Product_SKUs { get; set; }

        public string Shipping_Code { get; set; }

        public string Billing_City { get; set; }

        public string Ad_Click_Date { get; set; }

        public Created_By Created_By { get; set; }

        public string Secondary_Email { get; set; }

        public string Shipping_Street { get; set; }

        public string Ad { get; set; }

        public string Search_Partner_Network { get; set; }

        public string Shipping_State { get; set; }

        public float? Average_Time_Spent_Minutes { get; set; }

        public string Salutation { get; set; }

        public string First_Name { get; set; }

        public string Full_Name { get; set; }

        public string Conversion_Export_Status { get; set; }

        public float? Cost_per_Conversion { get; set; }

        public Modified_By Modified_By { get; set; }

        public string Phone { get; set; }

        public Account_Name Account_Name { get; set; }

        public bool? Email_Opt_Out { get; set; }

        public DateTime? Modified_Time { get; set; }

        public string Date_of_Birth { get; set; }

        public string Keyword { get; set; }

        public string Device_Type { get; set; }

        public string Title { get; set; }

        public string Mobile { get; set; }

        public string Last_Name { get; set; }

        public string Ad_Campaign_Name { get; set; }

        public string Lead_Source { get; set; }

        public string Billing_State { get; set; }

        public List<string> Tag { get; set; }

        public string Fax { get; set; }

        public string Reason_for_Conversion_Failure { get; set; }

    }

    public class Account_Name
    {

        public string name { get; set; }

        public string id { get; set; }

    }

}
