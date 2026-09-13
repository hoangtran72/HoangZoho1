using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.ZohoDesk
{

    public class CreateTicketRequest
    {

        public CreateTicketRequest()
        {

            departmentId = "477788000000006907";

            channel = "Zipfox Search";

            classification = "Product Not Found";

            priority = "High";

            assigneeId = "477788000000200001";

        }

        public string subject { get; set; }

        public string departmentId { get; set; }

        public DeskContact contact { get; set; }
        
        public string channel { get; set; }
        
        public string priority { get; set; }
        
        public string classification { get; set; }
        
        public Cf cf { get; set; }
        
        public string assigneeId { get; set; }
        
        public string description { get; set; }
        
        public string email { get; set; }
    
    }

    public class DeskContact
    {

        public string firstName { get; set; }
        
        public string lastName { get; set; }
        
        public string email { get; set; }
    
    }

    public class Cf
    {

        public string cf_product_not_found { get; set; }
        
        public string cf_estimated_quantity { get; set; }
    
    }

}
