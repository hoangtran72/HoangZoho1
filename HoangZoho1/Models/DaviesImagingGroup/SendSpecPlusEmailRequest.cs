using System;

namespace HoangZoho1.Models.DaviesImagingGroup
{

    public class SendSpecPlusEmailRequest
    {

        public string ProjectName { get; set; }
        
        public string SalesOffice { get; set; }
       
        public string CmName { get; set; }
        
        public string CmPhone { get; set; }
        
        public int? SiteCount { get; set; }
        
        public string Keycode { get; set; }
        
        public Agent[] Agents { get; set; }
        
        public Site[] Sites { get; set; }
        
        public string AccountId { get; set; }
        
        public DateTime? SubmissionDate { get; set; }
    
    }

    public class Agent
    {

        public string Name { get; set; }
        
        public string Phone { get; set; }
    
    }

    public class Site
    {

        public string Address { get; set; }
        
        public string Id { get; set; }
        
        public string Service { get; set; }
        
        public bool? Drone { get; set; }
        
        public bool? Matterport { get; set; }
        
        public decimal? Sqft { get; set; }
    
    }

}
