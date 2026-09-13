namespace HoangZoho1.Models.ZoRaw.Freightcom
{

    public class RetrieveShipmentDetailsResponse
    {

        public FreightcomShipment shipment { get; set; }

    }

    public class FreightcomShipment
    {

        public string id { get; set; }

        public string unique_id { get; set; }

        public string state { get; set; }

        public string transaction_number { get; set; }

        public string primary_tracking_number { get; set; }

        public string[] tracking_numbers { get; set; }

        public string tracking_url { get; set; }

        public string return_tracking_number { get; set; }

        public string bolnumber { get; set; }

        public string pickup_confirmation_number { get; set; }

        public FreightcomShipmentDetails details { get; set; }

        public object transport_data { get; set; }

        public FreightcomLabel[] labels { get; set; }

        public string customs_invoice_url { get; set; }

        public FreightcomRate rate { get; set; }

        public string order_source { get; set; }

    }

    public class FreightcomLabel
    {

        public string size { get; set; }

        public string format { get; set; }

        public string url { get; set; }

        public bool? padded { get; set; }

    }

}
