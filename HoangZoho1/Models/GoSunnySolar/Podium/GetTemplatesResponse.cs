using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.GoSunnySolar.Podium
{

    public class GetTemplatesResponse
    {

        public TemplateData[] data { get; set; }
        
        public Metadata metadata { get; set; }
    
    }

    public class Metadata
    {
        
        public string url { get; set; }
    
    }

    public class TemplateData
    {
        
        public string accessLevel { get; set; }
        
        public string attachmentUrl { get; set; }
        
        public object createdAt { get; set; }
       
        public object deletedAt { get; set; }
        
        public object isFavorite { get; set; }
        
        public object lastUsedAt { get; set; }
        
        public Location location { get; set; }
        
        public bool? nonDeletable { get; set; }
        
        public Organization organization { get; set; }
        
        public string subject { get; set; }
        
        public TemplateItem[] templateItems { get; set; }
        
        public string text { get; set; }
        
        public string title { get; set; }
        
        public string type { get; set; }
        
        public string uid { get; set; }
        
        public object updatedAt { get; set; }
        
        public User user { get; set; }
        
        public string[] variables { get; set; }
    
    }

    public class Location
    {
        
        public string uid { get; set; }
    
    }

    public class Organization
    {
        
        public string uid { get; set; }
    
    }

    public class User
    {
        
        public string uid { get; set; }
    
    }

    public class TemplateItem
    {
        
        public string applicationUid { get; set; }
        
        public string applicationUrl { get; set; }
        
        public string attachmentContentType { get; set; }
        
        public string attachmentUrl { get; set; }
        
        public string body { get; set; }
        
        public object data { get; set; }
        
        public object iconUrl { get; set; }
        
        public object keys { get; set; }
        
        public object subtitle { get; set; }
        
        public object title { get; set; }
        
        public string type { get; set; }
        
        public string uid { get; set; }
    
    }

}
