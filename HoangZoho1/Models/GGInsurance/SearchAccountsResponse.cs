using System;
using System.Collections.Generic;

namespace HoangZoho1.Models.GGInsurance
{

    public class SearchAccountsResponse
    {

        public SearchAccountsResponse()
        {
            
            data = new List<AccountData>();

        }

        public List<AccountData> data { get; set; }
        
        public Info info { get; set; }

    }

    public class Info
    {
        public int per_page { get; set; }
        public int count { get; set; }
        public string sort_by { get; set; }
        public int page { get; set; }
        public string sort_order { get; set; }
        public bool more_records { get; set; }
    }

    public class AccountData
    {
        
        public Owner Owner { get; set; }
        
        public Layout Layout { get; set; }
        
        public Layout_Id layout_id { get; set; }

        public DateTime? Created_Time { get; set; }

        public DateTime? Modified_Time { get; set; }
        
        public string id { get; set; }
        
        public Created_By Created_By { get; set; }

        public Modified_By Modified_By { get; set; }

    }

    public class Owner
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

    public class Layout
    {
        public string display_label { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Layout_Id
    {
        public string display_label { get; set; }
        public string name { get; set; }
        public string id { get; set; }
    }

    public class Created_By
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }

    public class Modified_By
    {
        public string name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
    }
    
}
