using Newtonsoft.Json;
using System.Collections.Generic;

namespace HoangZoho1.Models.ZoRaw.Freightcom
{

    public class RequestRateEstimateRequest
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> services { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> excluded_services { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomShipmentDetails details { get; set; }

    }

    public class FreightcomShipmentDetails
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomOrigin origin { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomDestination destination { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomExpectedShipDate expected_ship_date { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string packaging_type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomPackagingProperties packaging_properties { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomInsurance insurance { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] reference_codes { get; set; }

    }

    public class FreightcomOrigin
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomAddress address { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? residential { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? tailgate_required { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string instructions { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string contact_name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomPhoneNumber phone_number { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> email_addresses { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? receives_email_updates { get; set; }

    }

    public class FreightcomAddress
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string address_line_1 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string address_line_2 { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string unit_number { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string city { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string region { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string country { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string postal_code { get; set; }

    }

    public class FreightcomPhoneNumber
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string number { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string extension { get; set; }

    }

    public class FreightcomDestination
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomAddress address { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? residential { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? tailgate_required { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string instructions { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string contact_name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomPhoneNumber phone_number { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> email_addresses { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? receives_email_updates { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomReadyAt ready_at { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomReadyUntil ready_until { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string signature_requirement { get; set; }

    }

    public class FreightcomReadyAt
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? hour { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? minute { get; set; }

    }

    public class FreightcomReadyUntil
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? hour { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? minute { get; set; }

    }

    public class FreightcomExpectedShipDate
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? year { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? month { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? day { get; set; }

    }

    public class FreightcomPackagingProperties
    {

        public bool includes_return_label { get; set; }

        public bool has_dangerous_goods { get; set; }

        public List<FreightcomPackage> packages { get; set; }

    }

    public class FreightcomPallet
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomMeasurements measurements { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string freight_class { get; set; }

    }

    public class FreightcomPackage
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomMeasurements measurements { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? special_handling_required { get; set; }

    }

    public class FreightcomMeasurements
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomWeight weight { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomCuboid cuboid { get; set; }

    }

    public class Emergency_Contact_Phone_Number
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string number { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string extension { get; set; }

    }

    public class FreightcomWeight
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string unit { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? value { get; set; }

    }

    public class FreightcomCuboid
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string unit { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? l { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? w { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? h { get; set; }

    }

    public class FreightcomInsurance
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FreightcomTotalCost total_cost { get; set; }

    }

    public class FreightcomTotalCost
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string value { get; set; }

    }

}
