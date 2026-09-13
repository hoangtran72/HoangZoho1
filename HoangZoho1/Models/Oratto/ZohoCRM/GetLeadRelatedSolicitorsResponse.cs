using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Oratto.ZohoCRM
{

    public class GetLeadRelatedSolicitorsResponse
    {

        public GetLeadRelatedSolicitorsResponse()
        {

            data = new List<LeadSolicitorData>();

        }

        public List<LeadSolicitorData> data { get; set; }

        public Info info { get; set; }

    }
    
    public class LeadSolicitorData
    {

        public Owner Owner { get; set; }

        public string Email { get; set; }

        public string Not_Interested_Reason { get; set; }

        public Referred_Solicitors Referred_Solicitors { get; set; }
        
        public string Name { get; set; }

        public Modified_By Modified_By { get; set; }

        public string id { get; set; }

        public DateTime? Modified_Time { get; set; }

        public Leads_Referred Leads_Referred { get; set; }

        public DateTime? Created_Time { get; set; }

        public Created_By Created_By { get; set; }

    }

}
