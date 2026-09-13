using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Signarama.Custom
{
    public class HandleEstimateResponse
    {

        public string Estimate_Number { get; set; }

        public decimal? Subtotal { get; set; }

        public decimal? Taxes { get; set; }

        public DateTime? Created_Date { get; set; }

    }
}
