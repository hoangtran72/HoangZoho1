using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.Sakari
{


    public class FetchContactsResponse
    {

        public bool success { get; set; }
        
        public Pagination pagination { get; set; }
        
        public Error error { get; set; }
        
        public ContactData[] data { get; set; }
    
    }

    public class Pagination
    {

        public int? totalCount { get; set; }
        
        public int? limit { get; set; }
        
        public int? offset { get; set; }
    
    }

    public class Error
    {

        public string code { get; set; }
        
        public string message { get; set; }
    
    }

    public class ContactData
    {

        public string id { get; set; }
        
        public string email { get; set; }
        
        public string firstName { get; set; }
        
        public string lastName { get; set; }
        
        public ContactMobile mobile { get; set; }
        
        public ContactTag[] tags { get; set; }
        
        public Attributes attributes { get; set; }
        
        public bool? valid { get; set; }
        
        public ContactError error { get; set; }
        
        public ContactCreated created { get; set; }
        
        public ContactUpdated updated { get; set; }

    }

    public class ContactMobile
    {

        public string country { get; set; }

        public string number { get; set; }

    }

    public class Attributes
    {
    }

    public class ContactError
    {

        public string code { get; set; }

        public string description { get; set; }

    }

    public class ContactCreated
    {

        public DateTime? at { get; set; }
        
        public ContactBy by { get; set; }
    
    }

    public class ContactBy
    {

        public string id { get; set; }
        
        public string firstName { get; set; }
        
        public string lastName { get; set; }
    
    }

    public class ContactUpdated
    {
        
        public DateTime? at { get; set; }
        
        public ContactBy by { get; set; }
    
    }

    public class ContactTag
    {

        public string tag { get; set; }
        
        public bool visible { get; set; }
    
    }

}
