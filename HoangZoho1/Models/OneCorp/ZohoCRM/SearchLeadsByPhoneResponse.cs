using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.OneCorp.ZohoCRM
{

    public class SearchLeadsByPhoneResponse
    {

        public LeadData[] data { get; set; }

        public Info info { get; set; }

    }

    public class LeadData
    {

        public string id { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Full_Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Owner Owner { get; set; }

        
    }

}
