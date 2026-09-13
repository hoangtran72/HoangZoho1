using System;

namespace HoangZoho1.Models.Getunik.ZohoProjects
{

    public class GetProjectByIdResponse
    {

        public string id { get; set; }
        
        public string key { get; set; }
        
        public string name { get; set; }
        
        public string project_type { get; set; }
        
        public string description { get; set; }
        
        public Owner owner { get; set; }
        
        public bool? is_public_project { get; set; }
        
        public string start_date { get; set; }
        
        public string end_date { get; set; }
        
        public bool? is_strict_project { get; set; }
        
        public DateTime? created_time { get; set; }
        
        public Created_By created_by { get; set; }
        
        public DateTime? modified_time { get; set; }
        
        public Updated_By updated_by { get; set; }
        
        public Status status { get; set; }
        
        public Layout layout { get; set; }
        
        public string business_hours_id { get; set; }
        
        public bool? is_rollup_project { get; set; }
        
        public Budget_Info budget_info { get; set; }

        public Project_Group project_group { get; set; }
        
        public decimal? percent_complete { get; set; }
        
        public Tasks tasks { get; set; }
        
        public Issues issues { get; set; }
        
        public Milestones milestones { get; set; }
        
        public bool? is_completed { get; set; }
        
        public string website { get; set; }
        
        public string marketing_automation_rate { get; set; }
        
        public string website_rate { get; set; }
        
        public Quote_Id quote_id { get; set; }
        
        public string online_marketing { get; set; }
        
        public bool? additionally_send_to_primary_contact { get; set; }

        public decimal? discount { get; set; }
        
        public string online_marketing_rate { get; set; }
        
        public string projects_cf_0001 { get; set; }
        
        public string recipient_emails { get; set; }
        
        public string csm_rate { get; set; }
        
        public string ertragskonto { get; set; }
        
        public bool? autocreated_from_crm { get; set; }
        
        public Monthly_Budget monthly_budget { get; set; }
        
        public string marketing_automation { get; set; }
        
        public string billing_office { get; set; }
        
        public string csm { get; set; }
    
    }

    public class Owner
    {
        
        public int? zuid { get; set; }
        
        public string zpuid { get; set; }
        
        public string name { get; set; }
        
        public string email { get; set; }
        
        public string first_name { get; set; }
        
        public string last_name { get; set; }
        
        public string full_name { get; set; }
    
    }

    public class Created_By
    {

        public int? zuid { get; set; }
        
        public string zpuid { get; set; }
        
        public string name { get; set; }
        
        public string email { get; set; }
        
        public string first_name { get; set; }
        
        public string full_name { get; set; }
    
    }

    public class Updated_By
    {
        
        public int? zuid { get; set; }
        
        public string zpuid { get; set; }
        
        public string name { get; set; }
        
        public string email { get; set; }
        
        public string first_name { get; set; }
        
        public string last_name { get; set; }
        
        public string full_name { get; set; }
    
    }

    public class Status
    {
        
        public string id { get; set; }
        
        public string name { get; set; }
        
        public string color { get; set; }
        
        public string color_hexcode { get; set; }
        
        public bool? is_closed_type { get; set; }
    
    }

    public class Layout
    {
     
        public string id { get; set; }
        
        public string name { get; set; }
        
        public bool? is_default { get; set; }
        
        public string type { get; set; }
    
    }

    public class Budget_Info
    {
        
        public string billing_method { get; set; }
        
        public string budget_type { get; set; }
        
        public string currency { get; set; }
        
        public Cost_Budget cost_budget { get; set; }
        
        public string hourly_budget { get; set; }
        
        public Rate_Per_Hour rate_per_hour { get; set; }
        
        public Cost_Budget_Threshold cost_budget_threshold { get; set; }
        
        public string hourly_budget_threshold { get; set; }
        
        public Fixed_Cost fixed_cost { get; set; }
        
        public string tracking_method { get; set; }
        
        public Cost_Per_Hour cost_per_hour { get; set; }
        
        public Revenue_Budget revenue_budget { get; set; }
        
        public Planned_Cost planned_cost { get; set; }
        
        public Actual_Cost actual_cost { get; set; }
        
        public Forecasted_Cost forecasted_cost { get; set; }
        
        public string planned_hours { get; set; }
        
        public string actual_hours { get; set; }
        
        public string remaining_hours { get; set; }
        
        public string forecasted_hours { get; set; }
        
        public Planned_Value planned_value { get; set; }
        
        public Earned_Value earned_value { get; set; }
        
        public Schedule_Variance schedule_variance { get; set; }
        
        public Cost_Variance cost_variance { get; set; }
        
        public Planned_Revenue planned_revenue { get; set; }
        
        public Actual_Revenue actual_revenue { get; set; }
        
        public Forecasted_Revenue forecasted_revenue { get; set; }
    
    }

    public class Cost_Budget
    {
        
        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Rate_Per_Hour
    {

        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Cost_Budget_Threshold
    {

        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Fixed_Cost
    {
        
        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Cost_Per_Hour
    {
        
        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Revenue_Budget
    {
        
        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Planned_Cost
    {
        public string currency_code { get; set; }
        public string formatted_amount { get; set; }
        public decimal? amount { get; set; }
    }

    public class Actual_Cost
    {
        
        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Forecasted_Cost
    {
        
        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Planned_Value
    {

        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Earned_Value
    {

        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Schedule_Variance
    {

        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Cost_Variance
    {
        
        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Planned_Revenue
    {
        
        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Actual_Revenue
    {
        
        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

    public class Forecasted_Revenue
    {
        public string currency_code { get; set; }
        public string formatted_amount { get; set; }
        public float amount { get; set; }
    }

    public class Project_Group
    {
        
        public string id { get; set; }
        
        public string name { get; set; }
        
        public string type { get; set; }
    
    }

    public class Tasks
    {
        
        public int? open_count { get; set; }
        
        public int? closed_count { get; set; }
    
    }

    public class Issues
    {
        
        public int? open_count { get; set; }
        
        public int? closed_count { get; set; }
    
    }

    public class Milestones
    {
        
        public int? open_count { get; set; }
        
        public int? closed_count { get; set; }
    
    }

    public class Quote_Id
    {
        
        public string record_id { get; set; }
        
        public string value { get; set; }
        
        public string url { get; set; }
    
    }

    public class Monthly_Budget
    {
        
        public string currency_code { get; set; }
        
        public string formatted_amount { get; set; }
        
        public decimal? amount { get; set; }
    
    }

}
