using System;

namespace HoangZoho1.Models.Getunik.ZohoProjects
{

    public class GetProjectTasklistsResponse
    {

        public Page_Info page_info { get; set; }
        
        public Tasklist[] tasklists { get; set; }
    
    }

    public class Page_Info
    {

        public int? page { get; set; }
        
        public int? per_page { get; set; }
        
        public int? page_count { get; set; }
        
        public bool? has_next_page { get; set; }
    
    }

    public class Tasklist
    {

        public string id { get; set; }
        
        public string name { get; set; }
        
        public Milestone milestone { get; set; }
        
        public string flag { get; set; }
        
        public string status { get; set; }
        
        public string created_via { get; set; }
        
        public DateTime? created_time { get; set; }
        
        public DateTime? last_updated_time { get; set; }
        
        public Created_By created_by { get; set; }
        
        public Sequence sequence { get; set; }
        
        public Project project { get; set; }
        
        public Meta_Info meta_info { get; set; }
    
    }

    public class Milestone
    {
        
        public string id { get; set; }
        
        public string name { get; set; }
    
    }

    public class Sequence
    {
        
        public int? milestone_sequence { get; set; }
        
        public int? project_sequence { get; set; }
    
    }

    public class Project
    {

        public string id { get; set; }
        
        public string name { get; set; }
    
    }

    public class Meta_Info
    {

        public bool? is_completed { get; set; }
        
        public bool? is_rolled { get; set; }
        
        public bool? is_general { get; set; }
        
        public bool? has_comments { get; set; }
        
        public bool? is_none_milestone_tasklist { get; set; }
        
        public Count_Info count_info { get; set; }
    
    }

    public class Count_Info
    {

        public int? filtered_task_count { get; set; }
    
    }

}
