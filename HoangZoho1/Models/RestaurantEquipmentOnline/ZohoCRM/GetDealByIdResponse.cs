using System;

namespace HoangZoho1.Models.RestaurantEquipmentOnline.ZohoCRM
{

    public class GetDealByIdResponse
    {

        public DealData[] data { get; set; }
    
    }

    public class DealData
    {

        public Owner Owner { get; set; }

        public object Agile_ID { get; set; }
        
        public object GCLID { get; set; }
        
        public string currency_symbol { get; set; }
        
        public object Pause_Blueprint_Date { get; set; }
        
        public object field_states { get; set; }
        
        public DateTime? Quote_Sent_Date { get; set; }
        
        public object followers { get; set; }
        
        public DateTime? Last_Activity_Time { get; set; }
        
        public object Prefers_Call_or_Email { get; set; }
        
        public string state { get; set; }
        
        public bool? process_flow { get; set; }
        
        public string Deal_Name { get; set; }
        
        public int? Exchange_Rate { get; set; }
        
        public object Metro { get; set; }
        
        public string Currency { get; set; }
        
        public object Ad_Network { get; set; }
        
        public string Stage { get; set; }
        
        public object Hobart_Products { get; set; }
        
        public bool? locked_for_me { get; set; }
        
        public string id { get; set; }
        
        public bool? approved { get; set; }
        
        public object Conversion_Exported_On { get; set; }
        
        public Approval approval { get; set; }
        
        public object Territory { get; set; }
        
        public decimal? Cost_per_Click { get; set; }
        
        public object Click_Type { get; set; }
        
        public bool? Wait_for_Approval { get; set; }
        
        public object Contact_Mobile { get; set; }
        
        public DateTime? Created_Time { get; set; }
        
        public bool? followed { get; set; }
        
        public bool? editable { get; set; }
        
        public object AdGroup_Name { get; set; }
        
        public string Product_SKUs { get; set; }
        
        public object Product_Use_Front_of_house_Kitchen { get; set; }
        
        public string Previous_Owner_First_Name { get; set; }
        
        public object Contact_Email { get; set; }
        
        public object Ad_Click_Date { get; set; }
        
        public object Customer_ID { get; set; }
        
        public object Note { get; set; }
        
        public string Deal_Stage_before_Lost { get; set; }
        
        public Created_By Created_By { get; set; }
        
        public object zia_owner_assignment { get; set; }
        
        public object Ad { get; set; }
        
        public string Business_Type { get; set; }
        
        public object Search_Partner_Network { get; set; }
        
        public Review_Process review_process { get; set; }
        
        public object Equipment_Required { get; set; }
        
        public Layout_Id layout_id { get; set; }
        
        public string Closing_Date { get; set; }
        
        public object Conversion_Export_Status { get; set; }
        
        public string Quote_No { get; set; }
        
        public decimal? Cost_per_Conversion { get; set; }
        
        public Modified_By Modified_By { get; set; }
        
        public object review { get; set; }
        
        public decimal? Lead_Conversion_Time { get; set; }
        
        public decimal? Overall_Sales_Duration { get; set; }
        
        public Deal_Account_Name Account_Name { get; set; }
        
        public DateTime? Modified_Time { get; set; }
        
        public object Keyword { get; set; }
        
        public decimal? Amount { get; set; }
        
        public object Device_Type { get; set; }
        
        public object Finance_Stage { get; set; }
        
        public decimal? Probability { get; set; }
        
        public bool? orchestration { get; set; }
        
        public Deal_Contact_Name Contact_Name { get; set; }
        
        public int? Sales_Cycle_Duration { get; set; }
        
        public bool? Deal_Re_Engage_Klaviyo { get; set; }
        
        public object Quote_Confirmed_Date { get; set; }
        
        public bool? in_merge { get; set; }
        
        public object Ad_Campaign_Name { get; set; }
        
        public bool? Locked__s { get; set; }
        
        public string Considering_Finance { get; set; }
        
        public string Lead_Source { get; set; }
        
        public object[] Tag { get; set; }
        
        public string approval_state { get; set; }
        
        public object Reason_for_Conversion_Failure { get; set; }
        
        public object Location { get; set; }
    
    }

    public class Approval
    {
        
        public bool? _delegate { get; set; }
        
        public bool? takeover { get; set; }
        
        public bool? approve { get; set; }
        
        public bool? reject { get; set; }
        
        public bool? resubmit { get; set; }
    
    }

    public class Review_Process
    {
        
        public bool? approve { get; set; }
        
        public bool? reject { get; set; }
        
        public bool? resubmit { get; set; }
    
    }

    public class Layout_Id
    {
        
        public string name { get; set; }
        
        public string id { get; set; }
    
    }

    public class Deal_Account_Name
    {
        
        public string name { get; set; }
        
        public string id { get; set; }
    
    }

    public class Deal_Contact_Name
    {
        
        public string name { get; set; }
        
        public string id { get; set; }
    
    }

}
