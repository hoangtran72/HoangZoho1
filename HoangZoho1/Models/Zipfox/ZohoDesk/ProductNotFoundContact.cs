using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Zipfox.ZohoDesk
{

    public class ProductNotFoundContact
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string ProductDescription { get; set; }

        public decimal? EstimatedQuantity { get; set; }

        public string SearchValue { get; set; }

        public string Source { get; set; }

        public string AddedTime { get; set; }

        public string CRMStatus { get; set; }

        public string ReferrerName { get; set; }

        public string TaskOwner { get; set; }

    }

}
