using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.Stallion
{

    public class GetStallionRatesResponse
    {

        public GetStallionRatesResponse()
        {
            
            rates = new List<StallionRate>();

        }

        public bool? success { get; set; }

        public List<StallionRate> rates { get; set; }

    }

    public class StallionRate
    {

        public StallionRate()
        {
            
            add_ons = new List<StallionAddOn>();

        }

        public int? postage_type_id { get; set; }

        public string postage_type { get; set; }

        public bool? trackable { get; set; }

        public string package_type { get; set; }

        public float? base_rate { get; set; }

        public List<StallionAddOn> add_ons { get; set; }

        public float? rate { get; set; }

        public float? gst { get; set; }

        public float? pst { get; set; }

        public float? hst { get; set; }

        public float? qst { get; set; }

        public float? tax { get; set; }

        public float? total { get; set; }

        public string currency { get; set; }

        public string delivery_days { get; set; }

        public GetRatesInductionAddress induction_address { get; set; }

        public GetRatesReturnAddress return_address { get; set; }

    }

    public class GetRatesInductionAddress
    {

        public string name { get; set; }

        public string company { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string province_code { get; set; }

        public string postal_code { get; set; }

        public string country_code { get; set; }

        public string phone { get; set; }

        public object email { get; set; }

        public bool? is_residential { get; set; }

    }

    public class GetRatesReturnAddress
    {

        public string name { get; set; }

        public string company { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string province_code { get; set; }

        public string postal_code { get; set; }

        public string country_code { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public bool? is_residential { get; set; }

    }

    public class StallionAddOn
    {

        public string name { get; set; }

        public string type { get; set; }

        public float? cost { get; set; }

        public string currency { get; set; }

    }

}
