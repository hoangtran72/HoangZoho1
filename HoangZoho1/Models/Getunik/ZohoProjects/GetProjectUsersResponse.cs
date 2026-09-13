using System;
using System.Collections.Generic;

namespace HoangZoho1.Models.Getunik.ZohoProjects
{

    public class GetProjectUsersResponse
    {

        public GetProjectUsersResponse()
        {
            
            users = new List<ProjectUser>();

        }

        public User_Page_Info page_info { get; set; }
        
        public List<ProjectUser> users { get; set; }
    
    }

    public class User_Page_Info
    {

        public int? per_page { get; set; }

        public bool? has_next_page { get; set; }
        
        public int? count { get; set; }
        
        public int? page { get; set; }
    
    }

    public class ProjectUser
    {
        
        public Business_Hours business_hours { get; set; }
        
        public DateTime? updated_time { get; set; }
        
        public DateTime? added_time { get; set; }
        
        public bool? is_active { get; set; }
        
        public Role role { get; set; }
        
        public DateTime? time_of_request { get; set; }
        
        public bool? is_confirmed { get; set; }
        
        public Profile profile { get; set; }
        
        public string last_name { get; set; }
        
        public Reporting_To reporting_to { get; set; }
        
        public string display_name { get; set; }
        
        public string zuid { get; set; }
        
        public DateTime? last_accessed_on { get; set; }
        
        public string full_name { get; set; }
        
        public string user_type { get; set; }
        
        public string id { get; set; }
        
        public string invoice { get; set; }
        
        public string first_name { get; set; }
        
        public string email { get; set; }
        
        public string status { get; set; }
        
        public Budget budget { get; set; }
        
        public string deactivated_by { get; set; }
    
    }

    public class Business_Hours
    {
        
        public object[] working_hours { get; set; }
        
        public string name { get; set; }
        
        public string id { get; set; }
        
        public string version { get; set; }
    
    }

    public class Role
    {
        
        public string name { get; set; }
        
        public string id { get; set; }
        
        public string type { get; set; }
    
    }

    public class Profile
    {
        
        public string name { get; set; }
        
        public string id { get; set; }
        
        public string type { get; set; }
        
        public bool? is_default { get; set; }
    
    }

    public class Reporting_To
    {
        
        public string full_name { get; set; }
        
        public string last_name { get; set; }
        
        public string id { get; set; }
        
        public string first_name { get; set; }
        
        public string zuid { get; set; }
    
    }

    public class Budget
    {
        
        public string rate_per_hour { get; set; }
        
        public string cost_per_hour { get; set; }
    
    }

}
