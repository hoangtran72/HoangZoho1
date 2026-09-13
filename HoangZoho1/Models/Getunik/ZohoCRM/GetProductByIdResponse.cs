using System;
using System.Collections.Generic;

namespace HoangZoho1.Models.Getunik.ZohoCRM
{

    public class GetProductByIdResponse
    {

        public ProductDetails[] data { get; set; }
    
    }

    public class ProductDetails
    {

        public decimal? Stunden_Max { get; set; }
        
        public string AN_1 { get; set; }
        
        public bool? Sysfield_isProject { get; set; }
        
        public Owner Owner { get; set; }
        
        public string currency_symbol { get; set; }
        
        public object field_states { get; set; }
        
        public object NPO_Preis { get; set; }
        
        public string Abrechnung { get; set; }

        public string Pricing_Information { get; set; }
        
        public string sharing_permission { get; set; }

        public string Product_vs_Article { get; set; }
        
        public bool? Product_Active { get; set; }
        
        public DateTime? Last_Activity_Time { get; set; }
        
        public string Product_Number_Lookup { get; set; }
        
        public List<DeliverablesTask> Deliverables_Task { get; set; }

        public string state { get; set; }
        
        // public bool? process_flow { get; set; }
        
        public string Manufacturer { get; set; }
        
        // public bool locked_for_me { get; set; }
        
        public string id { get; set; }
        
        public int? Stunden_Min { get; set; }
        
        public string Status { get; set; }
        
        public string Circle { get; set; }
        
        public Approval approval { get; set; }
        
        public bool? Optional { get; set; }
        
        public bool? Sysfield_isSLA { get; set; }
        
        public DateTime? Created_Time { get; set; }
        
        public string Product_Name { get; set; }
        
        public bool? taxable { get; set; }
        
        public bool? editable { get; set; }
        
        public object Artikelnummer { get; set; }
        
        public string Consider_for_pricing { get; set; }
        
        public object Default_NPO_discount { get; set; }
        
        public string Task_Description { get; set; }
        
        public object Vertrag_Laufzeit_min { get; set; }
        
        public object[] Pain_Points { get; set; }
        
        public string DE_Accounting_Number { get; set; }
        
        public bool? Sysfield_isRetainer { get; set; }
        
        public Created_By Created_By { get; set; }
        
        public object Product_Category { get; set; }
        
        public object Installationspreis_einmalig { get; set; }
        
        public string Description { get; set; }
        
        public object[] Erwartete_Benefits { get; set; }
        
        public string CH_Accounting_Number { get; set; }
        
        public Review_Process review_process { get; set; }
        
        public object Ertrag { get; set; }
        
        public Layout_Id layout_id { get; set; }
        
        public object Record_Image { get; set; }
        
        public string Deliverables { get; set; }
        
        public Modified_By Modified_By { get; set; }
        
        public object review { get; set; }
        
        public object Product_Code { get; set; }

        public object Vertrag_Verl_ngerung { get; set; }
        
        public object Preis_Euro { get; set; }
        
        public Is_Part_Of Is_part_of { get; set; }
        
        public bool? Exclude_from_pricing_table { get; set; }
        
        public string Final_Import { get; set; }
        
        // public object zia_visions { get; set; }
        
        public object Vertrag_K_ndigungsfrist { get; set; }
        
        public DateTime? Modified_Time { get; set; }
        
        public string Article_Code { get; set; }
        
        public object Comments { get; set; }
        
        public object[] Product_assets { get; set; }
        
        public string Typ { get; set; }
        
        public string Record_Status__s { get; set; }
        
        // public bool orchestration { get; set; }
        
        public object Preis { get; set; }
        
        public object Usage_Unit { get; set; }
        
        public object W_hrung { get; set; }
        
        public Layout Layout { get; set; }
        
        public bool? in_merge { get; set; }
        
        public bool? Locked__s { get; set; }
        
        public object Subcontractor { get; set; }
        
        public bool? Sysfield_isLicense { get; set; }
        
        public object Product_Number1 { get; set; }
        
        public object[] Tag { get; set; }
        
        public object Email_Product_Owner { get; set; }
        
        public string[] Circle_Owner { get; set; }
        
        // public string approval_state { get; set; }
        
        public bool? pathfinder { get; set; }
        
        public decimal? Unit_Price { get; set; }
        
        public Has_More has_more { get; set; }
    
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
        
        public bool? takeover { get; set; }
        
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

    public class Layout_Id
    {

        public string display_label { get; set; }
        
        public string name { get; set; }
        
        public string id { get; set; }
    
    }

    public class Modified_By
    {
        
        public string name { get; set; }
        
        public string id { get; set; }
        
        public string email { get; set; }
    
    }

    public class Is_Part_Of
    {

        public string name { get; set; }
        
        public string id { get; set; }
    
    }

    public class Layout
    {

        public string display_label { get; set; }
        
        public string name { get; set; }
        
        public string id { get; set; }
    
    }

    public class Has_More
    {

        public bool? Deliverables_Task { get; set; }
        
        public bool? Product_assets { get; set; }
    
    }

    public class DeliverablesTask
    {
        
        public DeliverablesTaskDetails Deliverables_Task { get; set; }
        
        public string id { get; set; }

    }

    public class DeliverablesTaskDetails
    {

        public string name { get; set; }
        
        public string id { get; set; }
    
    }

}
