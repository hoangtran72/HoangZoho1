using System;

namespace HoangZoho1.Models.Getunik.ZohoProjects
{

    public class CreateTaskResponse
    {
        
        public CreateTaskProject project { get; set; }
        
        public CreateTaskMilestone milestone { get; set; }
        
        public CreateTaskTasklist tasklist { get; set; }
        
        public string id { get; set; }
        
        public string prefix { get; set; }
        
        public string name { get; set; }
        
        public string description { get; set; }
        
        public CreateTaskStatus status { get; set; }
        
        public string priority { get; set; }
        
        public CreateTaskOwners_And_Work owners_and_work { get; set; }
        
        public DateTime? start_date { get; set; }
        
        public DateTime? end_date { get; set; }
        
        public CreateTaskDuration duration { get; set; }
        
        public decimal? completion_percentage { get; set; }
        
        public CreateTaskSequence sequence { get; set; }
        
        public decimal? depth { get; set; }
        
        public DateTime? created_time { get; set; }
        
        public DateTime? last_modified_time { get; set; }
        
        public bool? is_completed { get; set; }
        
        public string created_via { get; set; }
        
        public CreateTaskCreated_By created_by { get; set; }
        
        public string billing_type { get; set; }
        
        public CreateTaskBudget_Info budget_info { get; set; }
        
        public CreateTaskLog_Hours log_hours { get; set; }
        
        public CreateTaskAssociation_Info association_info { get; set; }
        
        public string account_number { get; set; }
        
        public string billing_method { get; set; }
        
        public bool? autocreated_from_crm { get; set; }
        
        public string zcrm_product_id { get; set; }
    
    }

    public class CreateTaskProject
    {

        public string id { get; set; }
        
        public string name { get; set; }
    
    }

    public class CreateTaskMilestone
    {

        public string id { get; set; }
        
        public string name { get; set; }
    
    }

    public class CreateTaskTasklist
    {
        
        public string id { get; set; }
        
        public string name { get; set; }
    
    }

    public class CreateTaskStatus
    {
        public string id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string color_hexcode { get; set; }
        public bool is_closed_type { get; set; }
    }

    public class CreateTaskOwners_And_Work
    {

        public string work_type { get; set; }
        
        public string total_work { get; set; }
        
        public string unit { get; set; }
        
        public bool? copy_task_duration { get; set; }
        
        public CreateTaskOwner[] owners { get; set; }
        
        public bool? refresh_business_hours { get; set; }
    
    }

    public class CreateTaskOwner
    {

        public int? zuid { get; set; }
        
        public string zpuid { get; set; }
        
        public string name { get; set; }
        
        public string email { get; set; }
        
        public string first_name { get; set; }
        
        public string last_name { get; set; }
        
        public string work_values { get; set; }
    
    }

    public class CreateTaskDuration
    {
        
        public string value { get; set; }
        
        public string type { get; set; }
    
    }

    public class CreateTaskSequence
    {
        
        public int? sequence { get; set; }
    
    }

    public class CreateTaskCreated_By
    {

        public int? zuid { get; set; }
        
        public string zpuid { get; set; }
        
        public string name { get; set; }
        
        public string email { get; set; }
        
        public string first_name { get; set; }
        
        public string last_name { get; set; }
    
    }

    public class CreateTaskBudget_Info
    {
        
        public string budget { get; set; }
        
        public string threshold { get; set; }
        
        public string currency_code { get; set; }
        
        public CreateTaskCost_Budget_Info cost_budget_info { get; set; }
        
        public string hourly_budget { get; set; }
        
        public string hourly_budget_threshold { get; set; }
        
        public CreateTaskHourly_Budget_Info hourly_budget_info { get; set; }
        
        public string revenue_budget { get; set; }
        
        public CreateTaskRevenue_Budget_Info revenue_budget_info { get; set; }
        
        public string cost_rate_per_hour { get; set; }
        
        public string rate_per_hour { get; set; }
        
        public CreateTaskEvm_Budget_Info evm_budget_info { get; set; }
    
    }

    public class CreateTaskCost_Budget_Info
    {
        
        public string planned_cost { get; set; }
        
        public string actual_cost { get; set; }
        
        public string forecasted_cost { get; set; }
        
        public string balance { get; set; }
    
    }

    public class CreateTaskHourly_Budget_Info
    {
        
        public string actual_hours { get; set; }
        
        public string planned_hours { get; set; }
        
        public string difference { get; set; }
        
        public string forecasted_hours { get; set; }
    
    }

    public class CreateTaskRevenue_Budget_Info
    {
        
        public string planned_revenue { get; set; }
        
        public string actual_revenue { get; set; }
        
        public string forecasted_revenue { get; set; }
    
    }

    public class CreateTaskEvm_Budget_Info
    {
        
        public string earned_value { get; set; }
    
    }

    public class CreateTaskLog_Hours
    {
        
        public string billable_hours { get; set; }
        
        public string non_billable_hours { get; set; }
        
        public string total_hours { get; set; }
    
    }

    public class CreateTaskAssociation_Info
    {
        
        public bool? has_reminder { get; set; }
        
        public bool? has_recurrence { get; set; }
        public bool? has_comments { get; set; }
        
        public bool? has_attachments { get; set; }
        
        public bool? has_forums { get; set; }
        
        public bool? has_subtasks { get; set; }
    
    }

}
