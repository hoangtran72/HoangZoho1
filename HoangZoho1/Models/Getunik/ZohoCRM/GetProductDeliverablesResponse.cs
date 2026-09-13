using System;

namespace HoangZoho1.Models.Getunik.ZohoCRM
{

    public class GetProductDeliverablesResponse
    {

        public ProductDeliverablesData[] data { get; set; }
        
        public Info info { get; set; }
    
    }

    public class Info
    {

        public int? per_page { get; set; }
        
        public int? count { get; set; }
        
        public int? page { get; set; }
        
        public bool? more_records { get; set; }
    
    }

    public class ProductDeliverablesData
    {

        public Owner Owner { get; set; }
        
        public object Email { get; set; }
        
        public string currency_symbol { get; set; }
        
        public object field_states { get; set; }
        
        public object review_process { get; set; }
        
        public string Name { get; set; }
        
        public object Last_Activity_Time { get; set; }
        
        public Deliverables_Task Deliverables_Task { get; set; }
        
        public Modified_By Modified_By { get; set; }
        
        public object review { get; set; }
        
        public string state { get; set; }
        
        public object Unsubscribed_Mode { get; set; }
        
        public bool? converted { get; set; }
        
        public bool? process_flow { get; set; }
        
        public decimal? Exchange_Rate { get; set; }
        
        public string Currency { get; set; }
        
        public string id { get; set; }
        
        public bool? Email_Opt_Out { get; set; }
        
        public bool? approved { get; set; }
        
        public Approval approval { get; set; }
        
        public DateTime? Modified_Time { get; set; }
        
        public DateTime? Created_Time { get; set; }
        
        public object Unsubscribed_Time { get; set; }
        
        public bool? editable { get; set; }
        
        public bool? orchestration { get; set; }
        
        public Product_Deliverables Product_Deliverables { get; set; }
        
        public bool? in_merge { get; set; }
        
        public Created_By Created_By { get; set; }
        
        public object Secondary_Email { get; set; }
        
        public string approval_state { get; set; }
    
    }

    public class Deliverables_Task
    {

        public string name { get; set; }
        
        public string id { get; set; }
    
    }

    public class Product_Deliverables
    {

        public string name { get; set; }
        
        public string id { get; set; }
    
    }

}
