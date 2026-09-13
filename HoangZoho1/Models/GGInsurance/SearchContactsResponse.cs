using HoangZoho1.Models.Lumicare.ZohoCRM;
using System;
using System.Collections.Generic;

namespace HoangZoho1.Models.GGInsurance
{

    public class SearchContactsResponse
    {

        public SearchContactsResponse()
        {

            data = new List<ContactData>();

        }

        public List<ContactData> data { get; set; }
        
        public Info info { get; set; }
    
    }

    public class ContactData
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

}
