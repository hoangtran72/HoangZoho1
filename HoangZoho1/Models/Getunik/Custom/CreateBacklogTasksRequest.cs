using System.Collections.Generic;

namespace HoangZoho1.Models.Getunik.Custom
{

    public class CreateBacklogTasksRequest
    {

        public string projectId { get; set; }

        public string userEmail { get; set; }

        public TaskDetails taskDetails { get; set; }
    
    }

    public class TaskDetails
    {

        public string parentId { get; set; }
        
        public string parent { get; set; }
        
        public List<string> subtasks { get; set; }
    
    }

}
