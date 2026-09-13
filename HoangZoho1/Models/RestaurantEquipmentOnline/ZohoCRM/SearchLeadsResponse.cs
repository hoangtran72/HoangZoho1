using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class SearchLeadsResponse
    {
        public List<LeadData> data { get; set; }
        public SearchInfo info { get; set; }
    }

    public class LeadData
    {

        public string Shipping_Last_Name { get; set; }

        public Owner Owner { get; set; }

        public string Company { get; set; }

        public string Email { get; set; }

        public string GCLID { get; set; }

        public DateTime? Last_Activity_Time { get; set; }

        public string Shipping_Telephone { get; set; }

        public float? Exchange_Rate { get; set; }

        public string Currency { get; set; }

        public string Billing_Country { get; set; }

        public string Ad_Network { get; set; }

        public string id { get; set; }

        public float? Cost_per_Click { get; set; }

        public string Click_Type { get; set; }

        public string Billing_Street { get; set; }

        public DateTime? Created_Time { get; set; }

        public string Shipping_Company { get; set; }

        public string Shipping_Zip_Postal_Code { get; set; }

        public string Billing_Code { get; set; }

        public string AdGroup_Name { get; set; }

        public string Shipping_City { get; set; }

        public string Shipping_Country { get; set; }

        public string Billing_City { get; set; }

        public string Ad_Click_Date { get; set; }

        public string Customer_ID { get; set; }

        public Created_By Created_By { get; set; }

        public string Secondary_Email { get; set; }

        public string Shipping_Street { get; set; }

        public string Ad { get; set; }

        public string Search_Partner_Network { get; set; }

        public string Website { get; set; }

        public string Salutation { get; set; }

        public object Shipping_First_Name { get; set; }

        public string Lead_Status { get; set; }

        public string First_Name { get; set; }

        public string Full_Name { get; set; }

        public string Conversion_Export_Status { get; set; }

        public float? Cost_per_Conversion { get; set; }

        public Modified_By Modified_By { get; set; }

        public string Phone { get; set; }

        public DateTime? Modified_Time { get; set; }

        public string Keyword { get; set; }

        public string Device_Type { get; set; }

        public string Mobile { get; set; }

        public string Shipping_State_Province { get; set; }

        public string Last_Name { get; set; }

        public string Ad_Campaign_Name { get; set; }

        public string Referrer { get; set; }

        public string Lead_Source { get; set; }

        public string Billing_State { get; set; }

        public List<string> Business_Types { get; set; }

        public List<string> Tag { get; set; }

        public string Fax { get; set; }

        public string Reason_for_Conversion_Failure { get; set; }

        public string Lost_Lead_Reasons { get; set; }

    }

    public class Owner
    {

        public string name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

    }

    public class SearchInfo
    {

        public int? per_page { get; set; }

        public int? count { get; set; }

        public int? page { get; set; }

        public bool? more_records { get; set; }

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
