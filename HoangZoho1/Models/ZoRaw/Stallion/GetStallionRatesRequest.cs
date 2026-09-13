using Newtonsoft.Json;
using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.Stallion
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GetStallionRatesRequest
    {

        public GetStallionRatesRequest()
        {

            items = new List<StallionRateItem>();

        }

        public To_Address to_address { get; set; }

        public Return_Address return_address { get; set; }

        public bool? is_return { get; set; }

        public string weight_unit { get; set; }

        public decimal? weight { get; set; }

        public decimal? length { get; set; }

        public decimal? width { get; set; }

        public decimal? height { get; set; }

        public string size_unit { get; set; }

        public List<StallionRateItem> items { get; set; }

        public string package_type { get; set; }

        public object[] postage_types { get; set; }

        public bool? signature_confirmation { get; set; }

        public bool? insured { get; set; }

        public object region { get; set; }

        public Tax_Identifier tax_identifier { get; set; }

    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class To_Address
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

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Return_Address
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

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Tax_Identifier
    {

        public string tax_type { get; set; }

        public string number { get; set; }

        public string issuing_authority { get; set; }

    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StallionRateItem
    {

        public string description { get; set; }

        public string sku { get; set; }

        public decimal? quantity { get; set; }

        public decimal? value { get; set; }

        public string currency { get; set; }

        public string country_of_origin { get; set; }

        public string hs_code { get; set; }

        public string manufacturer_name { get; set; }

        public string manufacturer_address1 { get; set; }

        public string manufacturer_city { get; set; }

        public string manufacturer_province_code { get; set; }

        public string manufacturer_postal_code { get; set; }

        public string manufacturer_country_code { get; set; }

    }

}
